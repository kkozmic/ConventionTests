namespace TestStack.ConventionTests.Reporting
{
    using System.Linq;
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTextRenderer : IConventionReportRenderer
    {
        public void Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            var stringBuilder = new StringBuilder();

            foreach (var conventionReport in conventionResult)
            {
                var title = string.Format("{0}: '{1}' for '{2}'",
                    conventionReport.Result.Any() ? "Failed" : "Success",
                    conventionReport.ConventionTitle,
                    conventionReport.DataDescription);
                stringBuilder.AppendLine(title);
                stringBuilder.AppendLine(string.Empty.PadRight(title.Length, '-'));
                stringBuilder.AppendLine();

                if (conventionReport.HasApprovedExceptions)
                {
                    stringBuilder.AppendLine("With approved exceptions:");
                    RenderItems(conventionReport, stringBuilder, context);
                    stringBuilder.AppendLine();
                }

                RenderItems(conventionReport, stringBuilder, context);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
            }

            Output = stringBuilder.ToString().TrimEnd();
        }

        public string Output { get; private set; }

        static void RenderItems(ConventionResult resultInfo, StringBuilder stringBuilder, IConventionFormatContext context)
        {
            foreach (var item in resultInfo.Result)
            {
                stringBuilder.Append("\t");
                stringBuilder.AppendLine(string.Join(", ",context.Describe(item)));
            }
        }
    }
}