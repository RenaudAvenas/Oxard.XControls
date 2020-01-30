using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace Oxard.XControls.Interactivity
{
    /// <summary>
    /// Helps you to convert string into BindableProperty type
    /// </summary>
    public static class StringValueToPropertyConverter
    {
        private static readonly Type boolType = typeof(bool);
        private static readonly Type intType = typeof(int);
        private static readonly Type doubleType = typeof(double);
        private static readonly Type stringType = typeof(string);
        private static readonly List<Type> managedTypes = new List<Type>
        {
            boolType,
            intType,
            doubleType
        };

        public static object ConvertFor(this string stringValue, BindableProperty property)
        {
            if (property.ReturnType == stringType)
                return stringValue;
            
            if (managedTypes.Contains(property.ReturnType))
            {
                if (property.ReturnType == boolType)
                    return bool.TryParse(stringValue, out bool result) ? result : throw new ArgumentException($"Expected value must be a boolean but it is this string : {stringValue}", nameof(stringValue));
                if (property.ReturnType == intType)
                    return int.TryParse(stringValue, out int result) ? result : throw new ArgumentException($"Expected value must be an integer but it is this string : {stringValue}", nameof(stringValue));
                if (property.ReturnType == doubleType)
                    return double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double result) ? result : throw new ArgumentException($"Expected value must be an integer but it is this string : {stringValue}", nameof(stringValue));
                
            }
            else if (property.ReturnType.IsEnum)
                return Enum.Parse(property.ReturnType, stringValue);

            throw new ArgumentException($"Expected value must be a {property.ReturnType} but it is this string : {stringValue}", nameof(stringValue));
        }
    }
}
