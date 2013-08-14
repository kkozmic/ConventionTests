namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Conventions;
    using TestStack.ConventionTests.Reporting;

    public class ConventionContext : IConventionResultContext, IConventionFormatContext
    {
        readonly string dataDescription;
        readonly IList<IReportDataFormatter> formatters;
        readonly IList<IConventionReportRenderer> renderers;
        readonly IList<ConventionResult> results = new List<ConventionResult>();
        readonly bool hasApprovedExceptions;

        public ConventionContext(string dataDescription, IList<IReportDataFormatter> formatters,
            IList<IConventionReportRenderer> renderers, bool hasApprovedExceptions)
        {
            this.formatters = formatters;
            this.renderers = renderers;
            this.hasApprovedExceptions = hasApprovedExceptions;
            this.dataDescription = dataDescription;
        }

        public ConventionResult[] ConventionResults
        {
            get { return results.ToArray(); }
        }

        void IConventionResultContext.Is<T>(string resultTitle, IEnumerable<T> failingData)
        {
            // ReSharper disable PossibleMultipleEnumeration
            results.Add(new ConventionResult(
                failingData.ToObjectArray(),
                resultTitle,
                dataDescription,
                hasApprovedExceptions));
        }

        void IConventionResultContext.IsSymmetric<TResult>(
            string firstSetFailureTitle, IEnumerable<TResult> firstSetFailureData,
            string secondSetFailureTitle, IEnumerable<TResult> secondSetFailureData)
        {
            results.Add(new ConventionResult(
                firstSetFailureData.ToObjectArray(),
                firstSetFailureTitle,
                dataDescription,
                hasApprovedExceptions));
            results.Add(new ConventionResult(
                secondSetFailureData.ToObjectArray(),
                secondSetFailureTitle,
                dataDescription,
                hasApprovedExceptions));
        }

        void IConventionResultContext.IsSymmetric<TResult>(
            string firstSetFailureTitle,
            string secondSetFailureTitle,
            Func<TResult, bool> isPartOfFirstSet,
            Func<TResult, bool> isPartOfSecondSet,
            IEnumerable<TResult> allData)
        {
            var firstSetFailingData = allData.Where(isPartOfFirstSet).Unless(isPartOfSecondSet);
            var secondSetFailingData = allData.Where(isPartOfSecondSet).Unless(isPartOfFirstSet);

            (this as IConventionResultContext).IsSymmetric(
                firstSetFailureTitle, firstSetFailingData,
                secondSetFailureTitle, secondSetFailingData);
        }

        public void Execute<TDataSource>(IConvention<TDataSource> convention, TDataSource data)
            where TDataSource : IConventionData
        {
            if (!data.HasData)
                throw new ConventionSourceInvalidException(String.Format("{0} has no data", data.Description));

            convention.Execute(data, this);

            foreach (var renderer in renderers)
            {
                renderer.Render(this, ConventionResults);
            }
        }

        public string[] Describe(object item)
        {
            var formatter = formatters.FirstOrDefault(f => f.CanFormat(item));
            if (formatter == null)
            {
                throw new NoDataFormatterFoundException(
                    item.GetType().Name +
                    " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");
            }

            return formatter.Format(item, this);
        }
    }

    public interface IConventionFormatContext
    {
        string[] Describe(object item);
    }
}