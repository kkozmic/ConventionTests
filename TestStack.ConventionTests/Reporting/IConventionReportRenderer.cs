namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public interface IConventionReportRenderer
    {
        string Render(IConventionFormatContext context, params ConventionResult[] conventionResult);
    }
}