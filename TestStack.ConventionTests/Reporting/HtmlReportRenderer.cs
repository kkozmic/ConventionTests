namespace TestStack.ConventionTests.Reporting
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using TestStack.ConventionTests.Internal;

    public class HtmlReportRenderer : IConventionReportRenderer
    {
        readonly string file;

        public HtmlReportRenderer(string assemblyDirectory)
        {
            file = Path.Combine(assemblyDirectory, "Conventions.htm");
        }

        public void Render(IConventionFormatContext context, params ConventionResult[] conventionResult)
        {
            var sb = new StringBuilder();
            var html = new HtmlTextWriter(new StringWriter(sb));
            html.WriteLine("<!DOCTYPE html>");
            html.RenderBeginTag(HtmlTextWriterTag.Html);  // <html>
            html.RenderBeginTag(HtmlTextWriterTag.Head);  // <head>
            html.RenderEndTag();                          // </head>
            html.WriteLine();
            html.RenderBeginTag(HtmlTextWriterTag.Body);  // <body>

            html.RenderBeginTag(HtmlTextWriterTag.H1);
            html.Write("Project Conventions");
            html.RenderEndTag();

            foreach (var conventionReport in conventionResult)
            {
                html.RenderBeginTag(HtmlTextWriterTag.P);
                html.RenderBeginTag(HtmlTextWriterTag.Div);
                html.RenderBeginTag(HtmlTextWriterTag.Strong);
                html.Write(conventionReport.Result.Any() ? "Failed:" : "Success:");
                html.RenderEndTag();
                var title = string.Format("{0} for {1}", conventionReport.ConventionTitle, conventionReport.DataDescription);
                html.Write(title);
                if (conventionReport.HasApprovedExceptions)
                {
                    html.RenderBeginTag(HtmlTextWriterTag.Div);
                    html.RenderBeginTag(HtmlTextWriterTag.Strong);
                    html.WriteLine("With approved exceptions:");
                    html.RenderEndTag();
                    html.RenderEndTag();
                }
                
                html.RenderBeginTag(HtmlTextWriterTag.Ul);

                if (conventionReport.HasApprovedExceptions)
                {
                    html.RenderBeginTag(HtmlTextWriterTag.Li);
                    html.WriteLine(Describe(conventionReport,context));
                    html.RenderEndTag();
                }

                foreach (var conventionFailure in conventionReport.Result)
                {
                    html.RenderBeginTag(HtmlTextWriterTag.Li);
                    html.Write(conventionFailure.ToString());
                    html.RenderEndTag();
                }

                html.RenderEndTag();
                html.RenderEndTag();
                html.RenderEndTag();
            }

            html.RenderEndTag();                          // </body>
            html.RenderEndTag();                          // </html>
            html.Flush();

            File.WriteAllText(file, sb.ToString());
        }

        string Describe(ConventionResult conventionReport, IConventionFormatContext context)
        {
            return string.Join(Environment.NewLine, conventionReport.Result.Select(i => Describe(i, context)));
        }

        string Describe(object item, IConventionFormatContext context)
        {
            return string.Join(", ", context.Describe(item));
        }
    }
}