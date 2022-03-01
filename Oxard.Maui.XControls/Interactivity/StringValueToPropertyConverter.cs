using Microsoft.Maui.Graphics.Converters;
using System.Globalization;

namespace Oxard.Maui.XControls.Interactivity;

/// <summary>
/// Helps you to convert string into BindableProperty type
/// </summary>
public static class StringValueToPropertyConverter
{
    private static readonly Type boolType = typeof(bool);
    private static readonly Type intType = typeof(int);
    private static readonly Type doubleType = typeof(double);
    private static readonly Type stringType = typeof(string);
    private static readonly Type colorType = typeof(Color);
    private static readonly Type brushType = typeof(Brush);
    private static readonly List<Type> managedTypes = new List<Type>
    {
        boolType,
        intType,
        doubleType,
        colorType,
        brushType
    };

    /// <summary>
    /// Convert standard string to specific type
    /// </summary>
    /// <example>"True" => boolean true value. Red return <see cref="Xamarin.Forms.Color.Red"/></example>
    /// <param name="stringValue"></param>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public static object ConvertFor(this string stringValue, Type targetType)
    {
        if (targetType == stringType)
            return stringValue;
        
        if (managedTypes.Contains(targetType))
        {
            if (targetType == boolType)
                return bool.TryParse(stringValue, out bool result) ? result : throw new ArgumentException($"Expected value must be a boolean but it is this string : {stringValue}", nameof(stringValue));
            if (targetType == intType)
                return int.TryParse(stringValue, out int result) ? result : throw new ArgumentException($"Expected value must be an integer but it is this string : {stringValue}", nameof(stringValue));
            if (targetType == doubleType)
                return double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : throw new ArgumentException($"Expected value must be an integer but it is this string : {stringValue}", nameof(stringValue));
            if (targetType == colorType)
                return new ColorTypeConverter().ConvertFromInvariantString(stringValue);
            if (targetType == colorType)
                return new ColorTypeConverter().ConvertFromInvariantString(stringValue);
            if (targetType == brushType)
                return new BrushTypeConverter().ConvertFromInvariantString(stringValue);
        }
        else if (targetType.IsEnum)
            return Enum.Parse(targetType, stringValue);

        throw new ArgumentException($"Expected value must be a {targetType} but it is this string : {stringValue}", nameof(stringValue));
    }
}
