using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Represent an attached triggers collection
    /// </summary>
    public class TriggerCollection : ICollection<TriggerBase>
    {
        private readonly List<TriggerBase> triggers = new List<TriggerBase>();

        /// <summary>
        /// Gets the number of elements contained in the collection.
        /// </summary>
        public int Count => this.triggers.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        internal void AttachTo(BindableObject bindable)
        {
            var attachedCollection = new AttachedTriggerCollection();
            attachedCollection.AttachTo(this, bindable);
        }

        internal void DetachTo()
        {
        }

        /// <summary>
        /// Adds an item to the collection.
        /// </summary>
        /// <param name="item">The object to add to the collection.</param>
        public void Add(TriggerBase item) 
        { 
            this.triggers.Add(item);
        }

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            this.triggers.Clear();
        }

        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="item">The object to locate in the collection.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> is found in the collection; otherwise, false.
        /// </returns>
        public bool Contains(TriggerBase item) => this.triggers.Contains(item);

        /// <summary>
        /// Copies the elements of the collection to an <see cref="System.Array"></see>, starting at a particular <see cref="System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"></see> that is the destination of the elements copied from collection. The <see cref="System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(TriggerBase[] array, int arrayIndex) => this.triggers.CopyTo(array, arrayIndex);

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<TriggerBase> GetEnumerator() => this.triggers.GetEnumerator();

        /// <summary>
        /// Removes the first occurrence of a specific object from the collection.
        /// </summary>
        /// <param name="item">The object to remove from the collection.</param>
        /// <returns>
        /// true if <paramref name="item">item</paramref> was successfully removed from the collection; otherwise, false. This method also returns false if <paramref name="item">item</paramref> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        public bool Remove(TriggerBase item)
        {
            return this.triggers.Remove(item);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An collection object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
