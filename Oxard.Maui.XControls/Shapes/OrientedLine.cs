using Microsoft.Maui.Controls.Shapes;

namespace Oxard.Maui.XControls.Shapes;

/// <summary>
/// Class that draw a vertical or horizontal line.
/// </summary>
public class OrientedLine : Shape
{
    private bool isLoaded;

    /// <summary>
    /// Identifies the Orientation dependency property.
    /// </summary>
    public static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(LineOrientation), typeof(OrientedLine), LineOrientation.Vertical, propertyChanged: OnOrientationPropertyChanged);

    /// <summary>
    /// Get or set the orientation of the line
    /// </summary>
    public LineOrientation Orientation
    {
        get => (LineOrientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// Called when <see cref="Orientation"/> property changed. By default, it call <see cref="RefreshGeometry"/>.
    /// </summary>
    protected virtual void OnOrientationChanged()
    {
    }

    private static void OnOrientationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        (bindable as OrientedLine)?.OnOrientationChanged();
    }

    public override PathF GetPath()
    {
        //Graphics.GeometryHelper.GetOrientedLine(this.StrokeThickness, this.Orientation, this.Orientation == LineOrientation.Vertical ? this.Height : this.Width, this.Orientation == LineOrientation.Vertical ? this.Width : this.Height);
        return null;
    }
}
