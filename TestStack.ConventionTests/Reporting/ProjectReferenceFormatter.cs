namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.ConventionData;
    using TestStack.ConventionTests.Internal;

    public class ProjectReferenceFormatter : IReportDataFormatter
    {
        public bool CanFormat(object failingData)
        {
            return failingData is ProjectReference;
        }

        public string[] Format(object failingData, IConventionFormatContext context)
        {
            return new[] {((ProjectReference) failingData).ReferencedPath};
        }
    }
}