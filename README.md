# Oxard.XControls
Oxard XControls (for Xamarin controls) is available on nuget.org (https://www.nuget.org/packages/Oxard.XControls)

## Comming soon
Next project is to include an ItemsControl

## v1.0.1.0
- Events - TouchManager

This is a way to control touches on a control without gesture but directly via touches points (multitouch is not managed yet).
- Buttons

You can now have customizable buttons which inherits from ContentControl. Button is a good example of how to use TouchManager.
CheckBox and RadioButton have been added and inherit from Button class.
- Shapes

Ellipse is added.
- Extensions

Extensions to navigate in Xamarin "VisualTree".

## v1.0.0.0
- Basics

Basics for future controls creation. You can use Shapes to draw what you want with a Geometry.
Recangle is already created if you want an exemple.
- Colors

You can use Brushes instead of Colors (ex : Linear gradient, radial gradient). Brushes can be setted in Shape.Fill property
- ContentControl

This control can be templated by data with ContentTemplate and ContentTemplateSelector. It bring some basics properties like Background (use Brush and not Color like BackgroundColor), Foreground, ...
