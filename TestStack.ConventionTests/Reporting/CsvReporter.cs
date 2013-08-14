namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class CsvReporter : IConventionReportRenderer
    {
        public string Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            return string.Join(Environment.NewLine, conventionResult.Select(result => Build(result.Result, context)));
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