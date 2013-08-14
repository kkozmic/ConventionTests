namespace TestStack.ConventionTests.Reporting
{
    using TestStack.ConventionTests.Internal;

    public interface IConventionReportRenderer
    {
        void Render(IConventionFormatContext context, params ConventionResult[] conventionResult);
    }
}