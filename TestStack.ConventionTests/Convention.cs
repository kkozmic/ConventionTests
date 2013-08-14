namespace TestStack.ConventionTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using TestStack.ConventionTests.Internal;
    using TestStack.ConventionTests.Reporting;

    public static class Convention
    {
        static readonly HtmlReportRenderer HtmlRenderer = new HtmlReportRenderer(AssemblyDirectory);
        static readonly List<ConventionResult> Reports = new List<ConventionResult>();

        static Convention()
        {
            Formatters = new List<IReportDataFormatter>
            {
                new TypeDataFormatter(),
                new ProjectReferenceFormatter(),
                new ProjectFileFormatter(),
                new MethodInfoDataFormatter(),
                new StringDataFormatter()
            };
        }

        public static IEnumerable<ConventionResult> ConventionReports
        {
            get { return Reports; }
        }

        public static IList<IReportDataFormatter> Formatters { get; set; }

        static string AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Is(convention, data, new ConventionResultExceptionReporter());
        }

        public static void Is<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IConventionReportRenderer reporter)
            where TDataSource : IConventionData
        {
            Execute(convention, data, new[]
            {
                new ConventionReportTraceRenderer(),
                HtmlRenderer,
                reporter
            }, hasApprovedExceptions: false);
        }

        public static void IsWithApprovedExeptions<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            Execute(convention, data, new IConventionReportRenderer[]
            {
                new ConventionReportTraceRenderer(),
                HtmlRenderer,
                new ConventionResultExceptionReporter()
            }, hasApprovedExceptions: true);
        }

        static void Execute<TDataSource>(IConvention<TDataSource> convention, TDataSource data,
            IConventionReportRenderer[] renderers, bool hasApprovedExceptions)
            where TDataSource : IConventionData
        {
            var context = new ConventionContext(data.Description, Formatters, renderers, hasApprovedExceptions);
            context.Execute(convention, data);
        }

        // http://stackoverflow.com/questions/52797/c-how-do-i-get-the-path-of-the-assembly-the-code-is-in#answer-283917
    }
}