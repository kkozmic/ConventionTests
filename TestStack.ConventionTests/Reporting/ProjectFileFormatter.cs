namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class ProjectFileFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is ProjectFile;
        }

        public string[] Format(object failingData, IConventionFormatContext context)
        {
            return new[] {((ProjectFile) failingData).FilePath};
        }
    }
}