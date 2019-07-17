using Oxard.XControls.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Components
{
    /// <summary>
    /// Inherits from <see cref="CheckBox"/> but check is exclusive foreach other radio button on the same layout or <see cref="GroupName"/>
    /// </summary>
    public class RadioButton : CheckBox
    {
        private static readonly Dictionary<string, List<WeakReference<RadioButton>>> GroupNamedRadioButtons = new Dictionary<string, List<WeakReference<RadioButton>>>();
        private string groupName;

        /// <summary>
        /// Get or set a value indicates if the radio button can be unselected by clicking on it when it is already selected
        /// </summary>
        public bool IsUncheckable { get; set; }

        /// <summary>
        /// Get or set the group name where the radio button is
        /// </summary>
        public string GroupName
        {
            get => this.groupName;
            set
            {
                if (this.groupName != null)
                    throw new InvalidOperationException("GroupName property is already initialized");

                this.groupName = value;
                RegisterInGroupName(this);
            }
        }

        /// <summary>
        /// Invoke <see cref="Button.Clicked"/> event and call Execute method of <see cref="Command"/> property if not checked and unselect other radio buttons
        /// </summary>
        protected override void OnClicked()
        {
            if (this.IsChecked && !this.IsUncheckable)
                return;

            base.OnClicked();

            if (this.IsChecked)
                this.ManageIsChecked();
        }

        private static void RegisterInGroupName(RadioButton button)
        {
            if (!GroupNamedRadioButtons.ContainsKey(button.GroupName))
                GroupNamedRadioButtons.Add(button.GroupName, new List<WeakReference<RadioButton>> { new WeakReference<RadioButton>(button) });
            else
            {
                CleanGroupName(button.GroupName);
                GroupNamedRadioButtons[button.GroupName].Add(new WeakReference<RadioButton>(button));
            }
        }

        private static void CleanGroupName(string groupName)
        {
            GroupNamedRadioButtons[groupName].RemoveAll(reference => !reference.TryGetTarget(out RadioButton target));
        }

        private static List<RadioButton> GetRadioButtonForGroupName(string groupeName)
        {
            List<RadioButton> result = new List<RadioButton>();
            var weakReferences = GroupNamedRadioButtons[groupeName];
            for (int i = weakReferences.Count - 1; i >= 0; i--)
            {
                var weakReference = weakReferences[i];
                if (weakReference.TryGetTarget(out var radioButton))
                    result.Add(radioButton);
                else
                    weakReferences.RemoveAt(i);
            }

            return result;
        }

        private void ManageIsChecked()
        {
            IEnumerable<RadioButton> radioButtons;

            if(this.groupName == null)
            {
                var panel = this.FindParent<Layout<View>>();
                radioButtons = panel.FindChildren<RadioButton>().Where(c => c != this);
            }
            else
                radioButtons = GetRadioButtonForGroupName(this.groupName).Where(c => c != this);

            foreach (var radioButton in radioButtons)
                radioButton.IsChecked = false;
        }
    }
}
