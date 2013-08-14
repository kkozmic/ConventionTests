namespace TestStack.ConventionTests.Reporting
{
    using System.Diagnostics;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTraceRenderer : IConventionReportRenderer
    {
        public string Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            var conventionReportTextRenderer = new ConventionReportTextRenderer();
            conventionReportTextRenderer.Render(context, conventionResult);
            Trace.WriteLine(conventionReportTextRenderer.Output);
            return conventionReportTextRenderer.Output;
        }
    }
}