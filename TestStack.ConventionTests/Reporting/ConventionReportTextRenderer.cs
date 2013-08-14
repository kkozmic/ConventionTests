namespace TestStack.ConventionTests.Reporting
{
    using System.Text;
    using TestStack.ConventionTests.Internal;

    public class ConventionReportTextRenderer : IConventionReportRenderer
    {
        public string Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            var stringBuilder = new StringBuilder();

            foreach (var conventionReport in conventionResult)
            {
                var title = string.Format("'{0}' for '{1}'",
                    conventionReport.ConventionTitle,
                    conventionReport.DataDescription);
                stringBuilder.AppendLine(title);
                stringBuilder.AppendLine(string.Empty.PadRight(title.Length, '-'));
                stringBuilder.AppendLine();

                if (conventionReport.HasApprovedExceptions)
                {
                    stringBuilder.AppendLine("With approved exceptions:");
                    stringBuilder.AppendLine();
                }

                RenderItems(conventionReport, stringBuilder, context);
                stringBuilder.AppendLine();
                stringBuilder.AppendLine();
            }

            Output = stringBuilder.ToString().TrimEnd();
            return Output;
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