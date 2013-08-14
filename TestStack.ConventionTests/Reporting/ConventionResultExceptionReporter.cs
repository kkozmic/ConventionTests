namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Linq;
    using ApprovalTests;
    using TestStack.ConventionTests.Internal;

    public class ConventionResultExceptionReporter : IConventionReportRenderer
    {
        public string Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Render(context, conventionResult);
            if (conventionResult.Any(x => x.HasApprovedExceptions))
            {
                try
                {
                    Approvals.Verify(conventionReportTextRenderer.Output);
                }
                catch (Exception e)
                {
                    throw new ConventionFailedException(conventionReportTextRenderer.Output, e);
                }
            }
            else if (conventionResult.Any(r => r.Result.Any()))
            {
                throw new ConventionFailedException(conventionReportTextRenderer.Output);
            }
            return conventionReportTextRenderer.Output;
        }
    }
}