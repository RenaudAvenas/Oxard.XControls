# Oxard.XControls
Oxard XControls (for Xamarin controls) is available on nuget.org (https://www.nuget.org/packages/Oxard.XControls)

## 2.3.12.16

- Algorithm (layouts)

Add of Grid and UniformGrid algorithms for MultiFormatLayout.

- Layouts

You can create a layout that is based on one of LayoutAlgorithm by inherited from BaseLayout class. ZStackLayout and UniformGrid are implemented and use BaseLayout.

## 2.3.11.16

- TouchManager

Bug fix : TouchCancel cancel long press.

- BackgroundEffect

Bug fix : Background is applied on effect control if exists otherwise on effect container.

## 2.3.11.15

- DrawingBrush

Check null reference on AttachedBrush property changed

## 2.3.11.14

- TouchManager

Clicked event was called before TouchUp. Now it is reversed.

- Brushes

All brushes are implemented IClonable. This is usefull to create styles with brushes and change there properties for one element and not for all elements.
Particulary if you want a drawing brush and change there properties with attached properties in a Trigger.
The AttachedFill attached property allow you to attached a clone of brush describe in on each element that use your style.

## 2.3.10.13

- Spelling

Rename algorythm into algorithm

- CheckBox

Add Checked and Unchecked event.

- TouchManager

There was a bug causes a clicked when touch up but touch left.

## 2.2.9.12

- DrawingBrush bug

There was a null reference exception if a drawing brush attached property was set on element that has no BackgroundEffect or ContentControl that has no DrawingBrush for its Background

## 2.2.9.11

- MultiFormatLayout and LayoutAlgorythms

Adding a new Layout that can take an external algorythm for measurement and layout opertion. So you can use it to change dynamically disposition of element. Its needed often when you change from portrait to landscape view when you just want to change layout but not children.

ZStackAlgorythm allowed you to stack children on top of each other.

StackAlgorythm stack children horizontally or vertically.

- Bug fixed

Null reference not checked on ContentControl bindable properties changed.

## v2.2.8.10

- DrawingBrush

Adding attached properties to specified changes on a DrawingBrush without create a new DrawingBrush

## v2.2.7.10

- ShapeRenderer

Fixed a bug on FastRenderer for shapes that doesn't override Dispose correctly and add GeometryChanged event subscritpion.

## v2.2.7.9

- Shape and there renderer

Shapes are treated by fast renderer for Android. The ShapeRenderer already exists but all oxard shapes are managed by FastShapeRenderer.

## v2.2.6.9

- TouchManager

There was a bug on long press when tapping many times before long press time that causes simple tap considered as long press tap.

## v2.2.6.8

- Xamarin.Forms

The package and solution now reference Xamarin.Forms 4.0.0.425677

- XmlnsDefinition

Oxard controls are now under a XmlnsDefinition : https://github.com/RenaudAvenas/Oxard.XControls

- Shapes

Measurement of shapes are updated to match with new measurement of Xamarin 4. StrokeThickness now update the geometry.

Property IsClosed was added to Geometry class to specify if your shape is closed or not.

## v1.2.6.8

- Android ContentControlRenderer

Fix a bug when OnElementChanged called but renderer already disposed.

Fix a bug with background changement which didn't take care about Xamarin Forms Element size.

## v1.2.6.7

- MeasureExtensions

Adding a class of extensions on Xamarin Forms View to help standard measurement of components.

- Shape

Take care about measures from base shape. Override OnMeasure if you want to come back to Xamarin Forms basic measures or reimplement it tu customize your shape size.

- Geometry

Again a bug on pixel delay when StrokeThickness is greater than 1.

- ContentControl

There is a new Renderer for ContentControl which allows to manage Background property without defining a ControlTemplate with a Shape.
UWP support DrawingBrush in ContentControl inherited classes. Use IsBackgroundManagedByStyle property to enable or disable this feature (default value is False).

## v1.1.5.6

- Geometry

Better fix for the bug due to precision loss which causes stroke to be clipped.

## v1.1.5.5
- Button

Fix bug : IsPressed is not set to True when Button is disable.

- Geometry

Constants are created in geometry ratio to fix a bug with strokeThickness inferior than 1 which causes stroke to be clipped

## v1.1.5.4
- RadioButton

Property GroupeName is replaced by GroupName.
- Button

Command property was not called when we click on the button. That is fixed.

On UWP, Pointer are captured by clickable controls when touching down and release when touching up.

## v1.0.4.4
- ObjectDisposedException bug

Some access to native control caused ObjectDisposedException in ShapeRenderer.

## v1.0.4.0
- Background effect

You can now use Brush to set background on native Xamarin controls with the BackgroundEffect.

- DrawingBrush

This is a new Brush that can be defined like a shape. Very helpful to draw a cornered rectangle as background!
Unfortunately, this type of brush can be used only on Android now because UWP does not support other brush than LinearGradient or SolidColor brushes.
So you have to register an IBrushInterpretor in InterpretorManager to translate DrawingBrush into one of brushes above or use ControlTemplate with Shape.


## v1.0.3.0
- LongPressButton

New control which allowed you to manage a simple button or a long press on button with two commands and events.

## v1.0.2.0
- ItemsControl

This new component allow you to use an items source on a specific panel. All items can be templated by ItemTemplate or ItemTemplateSelector.
ItemsControl is designed to be extended with possibilities to inherited from IsItemItsOwnContainerOverride and GetContainerForItemOverride.

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
