# Oxard.XControls
Oxard XControls (for Xamarin controls) is available on nuget.org (https://www.nuget.org/packages/Oxard.XControls)

To use components in your project with http://oxard.com/XControls shemas add this code to your App constructor (or somewhere in your assembly where you use oxard controls):

```csharp
public App() 
{ 
    Oxard.XControls.Initializer.Init(); 
    this.InitializeComponent(); 
}
```

You can also use Preserve attribute too anywhere like this :

```csharp
using Oxard.XControls;

[assembly:Preserve]
```

## 4.7.24.42

-DynamicResource

Oxard dynamic resource can be applied to XF Setter and Oxard Setter. Oxard dynamic resource system can be improved with implementing ExtendedDynamicResourceImplementation and register implementation in the markup extension (ExtendedDynamicResourceExtension.RegisterExtension)

-DrawableBrushes

Drawable brushes take care about Fill and Stroke property change to redraw native drawable. In inherited class, call OnSubPropertyChanged method to force redraw for property.

Bug fix : some properties were not cloned or not affected when using AttachedBrush (StrokeMiterLimit, StrokeDashOffset, ...).

## 4.7.23.41
- DynamicResource

Xamarin.Forms DynamicResource does not work on object that not implement internal XF interface IResource.
By example, if you want to set a dynamic resource for SolidColorBrush Color property, this code does not work :

```xml
<ResourceDictionary>
    <Color
        x:Key="BackgroundLabel">Blue</Color>
    <SolidColorBrush
        x:Key="BackgroundLabelBrush"
        Color="{DynamicResource BackgroundLabel}" />
</ResourceDictionary>
```

If you set BackgroundLabelBrush to a label background, its background will be not set.
Use this equivalent with Oxard offers a workaround :

```xml
<ResourceDictionary>
    <Color
        x:Key="BackgroundLabel">Blue</Color>
    <SolidColorBrush
        x:Key="OxardBackgroundLabelBrush"
        Color="{oxard:ExtendedDynamicResource BackgroundLabel, UseReflection=True}" />
</ResourceDictionary>
```

The background of label change and if you override the BackgroundLabel resource it takes effect.

You should set UseReflection to true to associate your dynamic resource to first visual parent element of the brush or you can set the Container property to indicate where resources will be found.
If UseReflection is set to false and Container is not set (this is the default values), Oxard takes Application.Current instance to find resources.

## 4.7.22.41
- Keyboard management

You can use KeyboardManager to have access to keyboard events. If you want to get keys on a specific control, attach a KeyboardHelper on each platform.
Don't forget to call KeyboardHelper.Initialize method in mainActivity for android and App.xaml.cs for UWP to get events from the whole application.
For android, you need to implement IKeyboardListener on a renderer or on your MainActivity.
Copy and paste this code :

```csharp

        // Renderer or mainactiviry class
        public override bool DispatchKeyEvent(KeyEvent e)
        {
            var androidKeyboardEventArgs = new AndroidKeyboardEventArgs(e);
            PreviewKeyEvent?.Invoke(this, androidKeyboardEventArgs);
            return androidKeyboardEventArgs.Handled == true ? true : base.DispatchKeyEvent(e);
        }

        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var androidKeyboardEventArgs = new AndroidKeyboardEventArgs(e);
            KeyEvent?.Invoke(this, androidKeyboardEventArgs);
            return androidKeyboardEventArgs.Handled == true ? true : base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var androidKeyboardEventArgs = new AndroidKeyboardEventArgs(e);
            KeyEvent?.Invoke(this, androidKeyboardEventArgs);
            return androidKeyboardEventArgs.Handled == true ? true : base.OnKeyUp(keyCode, e);
        }

```

## 4.7.21.41
- Interactivity

Setters overrides original property value when applied but if the orginal value is from a binding, 
the binding was lost. In this version, you can set a OriginalValueBinding on setters to specify 
which binding setter must be used to set the original binding when it is unapplied.

- Virtualization

Add a ScrollToAsync(object item) method on VirtualizingItemsControl to scroll to an element of the 
ItemsSource even if the view for item is not generated yet.

## 4.7.20.41
- Virtualization

Fix a bug with Oxard Triggers which ignored changement on original value of a bindable property.
If you affect a property to a value "A" via Oxard Setter and then you change this property to "B" 
in your code, the "B" value is stored as original value and setter is reapplied and property value 
is "A" until setter be deactivated.

## 4.7.20.40
- Virtualization

First version of UI virtualization with VirtualizingItemsControl and VirtualizingStackLayout. For now, VirtualizingItemsControl is a fork of ItemsControl.

## 4.7.19.40
- TriggerCollection

Fix a bug if attach to non visible object that causes thread memory leak.

## 4.7.19.39
- OrientedLine

Adding OrientedLine to draw lines horizontally or vertically.

![OrientedLine](Documentation/orientedLines.png "Vertical and horizontal lines")

## 4.7.18.39
- ListBox

Adding new ListBox component.

## 4.7.17.39
- ContentControl

If ContentData is null at beginning, ContentTemplate or ContentTemplateSelector was not applied.

## 4.7.17.38
- ContentControl

ContentData property added and ContentData is used to template content via ContentTemplate or ContentTemplateSelector properties

## 4.7.17.37
- WrapLayout

Fix a bug with some layout measurement.

## 4.7.17.36
- WrapLayout

A new ChildAlignment property is added on WrapLayout and WrapAlgorithm. It align child in rows or columns in WrapLayouts.

Example :

ChildAlignment : LeftOrTop

![WrapLayoutLeft](Documentation/wrapLayoutLeft.png "WrapLayout with LeftOrTop")

ChildAlignment : Center

![WrapLayoutCenter](Documentation/wrapLayoutCenter.png "WrapLayout with Center")

ChildAlignment : Justify

![WrapLayoutJustify](Documentation/wrapLayoutJustify.png "WrapLayout with Justify")

## 4.6.16.36
- Minimal Xamarin Forms version supported set to 4.8.0.1821 

## 4.5.16.36

- Android ContentControlRenderer

When renderer is disposed, the background was not disposed too. This version fixed it.

## 4.5.16.35

- Android DrawingBrush

DrawingBrush are not disposed correctly on android

## 4.5.16.34

- Android DrawingBrush

    - On android, drawing brush was not react to GeometryChanged event.
    - If StrokeThickness is 0 then stroke is not rendered.

## 4.5.16.33

- RectangleBrush

Same bug fix than 4.5.16.32 for RectangleBrush

## 4.5.16.32

- RoundedRectangle

Calculate it's size when stroke thickness changed

## 4.5.16.31

- RoundedRectangle

Fix a bug on android to calculate mesure not from Data property but from WhidthRequest and HeightRequest.

## 4.5.16.30

- Interactivity

Wait control to be loaded before applying setter and fix bugs when multiple trigger change the same property on same target

## 4.5.16.29

- Setter

Fix a bug when unapply a setter to an object when Target property is set.

## 4.5.16.28

- Triggers

Oxard triggers can have setters that have a different target than the object that declare the trigger.

In the following example, a trigger is declared on a Button but change Labels TextColor.

```xml
<Grid
    RowDefinitions="Auto, Auto, *">
    <Label
        Grid.Row="0"
        x:Name="Label1"
        Text="Foreground white when button pressed"
        TextColor="Red" />
    <Label
        Grid.Row="1"
        x:Name="Label2"
        Text="Foreground blue when button pressed"
        TextColor="Green" />

    <oxard:Button
        Grid.Row="2">
        <oxard:Interactivity.Triggers>
            <oxard:TriggerCollection>
                <oxard:Trigger
                    Property="{x:Static oxard:Button.IsPressedProperty}"
                    Value="True">
                    <oxard:Setter
                        Target="{x:Reference Label1}"
                        Property="{x:Static Label.TextColorProperty}"
                        Value="White" />
                    <oxard:Setter
                        Target="{x:Reference Label2}"
                        Property="{x:Static Label.TextColorProperty}"
                        Value="Blue" />
                </oxard:Trigger>
            </oxard:TriggerCollection>
        </oxard:Interactivity.Triggers>
        <Label
            Text="Click me!"
            TextColor="White" />
    </oxard:Button>
</Grid>
```

## 4.5.15.28

- Update to Xamarin.Forms 5

All oxard brushes are removed except DrawingBrush. Use XF brushes instead. BackgoundEffect has been deleted because Background property is supported by XF.
Renderers for ContentControl (and all inherits controls) support DrawingBrush.

Oxard shapes has been deleted except Rectangle that is renamed to RoundedRectangle

## 3.5.15.28

- UWP

Adding output files in nuget package to build on UWP.

## 3.5.15.27

- MultiFormatLayout

Fix : if algorithm was set durring measurment or layout, MultiFormatLayout made infinity loop.

## 3.5.15.26

- GridAlgoritm

Fix a bug when measure a grid in width or height inifinite with Column or Row with a GridLength.Star (yes again!)

- MultiFormatLayout

Prevent measurement and layout when algotihm change during measurement or layout.

OnMeasure and LayoutChildren are sealed. Use BeforeMeasure or BeforeLayoutChildren instead.

## 3.4.15.25
- XF update

Xamarin forms update to 4.3.0.991250

- GridAlgoritm

Fix a bug when measure a grid in width or height inifinite with Column or Row with a GridLength.Star

## 2.4.15.24

- Oxard.Interactivity

Fix on activation on already active trigger.

## 2.4.15.23

- Oxard.Interactivity

Add a new Triggers attached property to manage some bugs with Xamarin.Forms native Triggers
```xml
 <Style x:Key="InteractivityStyle" TargetType="oxard:RadioButton">
    <Setter Property="oxard:Interactivity.Triggers">
        <Setter.Value>
            <oxard:TriggerCollection>
                <oxard:Trigger
                    Property="{x:Static oxard:RadioButton.IsCheckedProperty}"
                    Value="True">
                    <oxard:Setter
                        Property="{x:Static oxard:RadioButton.BackgroundProperty}"
                        Value="{StaticResource ButtonPressedBackgroundBrush}" />
                </oxard:Trigger>
                <oxard:Trigger
                    Property="{x:Static oxard:RadioButton.IsEnabledProperty}"
                    Value="False">
                    <oxard:Setter
                        Property="{x:Static oxard:RadioButton.BackgroundProperty}"
                        Value="{StaticResource ButtonDisableBackgroundBrush}" />
                </oxard:Trigger>
            </oxard:TriggerCollection>
        </Setter.Value>
    </Setter>
</Style>
```

- CheckBox and RadioButton

Fix a bug when set IsChecked property by code

## 2.4.14.23

- ItemsControl

You can now use AlternationCount property to specify item container behavior depending on their value of AlternationIndex attached property. 

## 2.4.14.22

- LayoutAlgorithm

All Oxard LayoutAlgorithm inherited classes use BindableProperty instead of classical CLR properties and are now bindable.
Method OnMeasureLayoutRequested added to LayoutAlgorithm base class to simplify bindable properties declaration in inherited classes.

## 2.4.14.21

- ItemsControl

Loaded method added

## 2.4.14.20

- ItemsControl

Bug fix : GetContainerForItemOverride is overridable and have no parameter. If ItemTemplate or DataTemplateSelector is used, the generated item content is filled with template (if item generated is a content view)


## 2.4.14.19

- Xamarin.Forms update

Now the library is based on Xamarin.Forms 4.1.0.555618.

- XmlnsDefinition

Initializer class has been created to initialize the library and use xmlns namespace. Call the Init method if the namespace is not recognize by your project.
Furthermore namespace has been renamed to http://oxard.com/XControls

- UWP and TouchHelper

Bug fix : if you use a mouse, mouse over was considered as TouchEnter

## 2.3.14.18

- StackAlgorithm

Bu fix : problem on calculation of measurement that return the size of the last child instead of the whole size of children

## 2.3.14.17

- ItemsControl

Add GetViewForDataItem method to get generated item for a specific item in ItemsSource

## 2.3.13.17

- WrapLayout & WrapAlgorithm

WrapLayout and WrapAlgorithm have been added to controls.

## 2.3.12.17

- ZStackAlgorithm

Bug fix : this layout did not take care about not visible children.

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
