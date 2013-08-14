namespace TestStack.ConventionTests.Reporting
{
    using System.Reflection;
    using TestStack.ConventionTests.Internal;

    public class MethodInfoDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is MethodInfo;
        }

        public string[] Format(object failingData, IConventionFormatContext context)
        {
            var methodInfo = (MethodInfo) failingData;

            return new[] {methodInfo.DeclaringType + "." + methodInfo.Name};
        }
    }
}