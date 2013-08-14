namespace TestStack.ConventionTests.Reporting
{
    using System.Collections;
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class CsvReporter : IConventionReportRenderer
    {
        public void Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            foreach (var result in conventionResult)
            {
                Build(result.Result, context);
            }
        }

        string Build(IEnumerable results, IConventionFormatContext formatter)
        {
            var message = new StringBuilder();
            foreach (var result in results)
            {
                message.AppendLine(string.Join(",", formatter.Describe(result)));
            }
            return message.ToString();
        }
    }
}