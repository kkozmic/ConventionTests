namespace TestStack.ConventionTests.Reporting
{
    using System;
    using TestStack.ConventionTests.Internal;

    public class TypeDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is Type;
        }

        public string[] Format(object failingData, IConventionFormatContext context)
        {
            return new[] {((Type) failingData).FullName};
        }
    }
}