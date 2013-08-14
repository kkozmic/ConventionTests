namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using TestStack.ConventionTests.Internal;

    public class DefaultFormatter : IReportDataFormatter
    {
        readonly IDictionary<Type, PropertyInfo[]> typeToProperties = new Dictionary<Type, PropertyInfo[]>();

        public bool CanFormat(object failingData)
        {
            return true;
        }

        public string[] Format(object failingData, IConventionFormatContext context)
        {
            return DesribeItem(failingData, GetProperties(failingData.GetType()), context);
        }

        // TODO: this is a very crappy name for a method
        public string[] DesribeType(Type type)
        {
            var props = GetProperties(type);
            return props.Select(Describe).ToArray();
        }

        string Describe(PropertyInfo property)
        {
            return property.Name.Replace('_', ' ');
        }

        string[] DesribeItem(object result, PropertyInfo[] props, IConventionFormatContext context)
        {
            return props.Select(p => GetItemValue(result, p, context)).ToArray();
        }

        static string GetItemValue(object result, PropertyInfo p, IConventionFormatContext context)
        {
            var value = context.Describe(p.GetValue(result, null));
            // NOTE: supporting trivial values for now only
            return value[0];
        }

        PropertyInfo[] GetProperties(Type type)
        {
            PropertyInfo[] props;
            if (typeToProperties.TryGetValue(type, out props) == false)
            {
                props = type.GetProperties();
                typeToProperties[type] = props;
            }
            return props;
        }
    }
}