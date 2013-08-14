namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public class StringDataFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is string;
        }

        public string[] Format(object failingData, IConventionFormatContext context)
        {
            return new[] {(string) failingData};
        }
    }
}