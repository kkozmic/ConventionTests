namespace TestStack.ConventionTests.Reporting
{
    using System.Linq;
    using TestStack.ConventionTests.Internal;

    public class ConventionResultExceptionReporter : IConventionReportRenderer
    {
        public void Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Render(context, conventionResult);
            if (conventionResult.Any(r => r.Result.Any()))
            {
                throw new ConventionFailedException(conventionReportTextRenderer.Output);
            }
        }
    }
}