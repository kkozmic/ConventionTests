﻿namespace TestStack.ConventionTests.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TestStack.ConventionTests.Reporting;

    public class ConventionContext : IConventionResultContext
    {
        readonly List<ResultInfo> conventionResults;
        readonly string dataDescription;
        readonly IList<IReportDataFormatter> formatters;

        public ConventionContext(string dataDescription, IList<IReportDataFormatter> formatters)
        {
            this.formatters = formatters;
            this.dataDescription = dataDescription;
            conventionResults = new List<ResultInfo>();
        }

        public ResultInfo[] ConventionResults
        {
            get { return conventionResults.ToArray(); }
        }

        public void Is<T>(string resultTitle, IEnumerable<T> failingData)
        {
            // ReSharper disable PossibleMultipleEnumeration
            conventionResults.Add(new ResultInfo(
                failingData.None() ? TestResult.Passed : TestResult.Failed,
                resultTitle,
                dataDescription,
                failingData.Select(FormatData).ToArray()));
        }

        public void IsSymmetric<TResult>(
            string firstSetFailureTitle, IEnumerable<TResult> firstSetFailureData,
            string secondSetFailureTitle, IEnumerable<TResult> secondSetFailureData)
        {
            conventionResults.Add(new ResultInfo(
                firstSetFailureData.None() ? TestResult.Passed : TestResult.Failed,
                firstSetFailureTitle,
                dataDescription,
                firstSetFailureData.Select(FormatData).ToArray()));
            conventionResults.Add(new ResultInfo(
                secondSetFailureData.None() ? TestResult.Passed : TestResult.Failed,
                secondSetFailureTitle,
                dataDescription,
                secondSetFailureData.Select(FormatData).ToArray()));
        }

        public void IsSymmetric<TResult>(
            string firstSetFailureTitle,
            string secondSetFailureTitle,
            Func<TResult, bool> isPartOfFirstSet,
            Func<TResult, bool> isPartOfSecondSet,
            IEnumerable<TResult> allData)
        {
            var firstSetFailingData = allData.Where(isPartOfFirstSet).Unless(isPartOfSecondSet);
            var secondSetFailingData = allData.Where(isPartOfSecondSet).Unless(isPartOfFirstSet);

            IsSymmetric(
                firstSetFailureTitle, firstSetFailingData,
                secondSetFailureTitle, secondSetFailingData);
        }

        ConventionReportFailure FormatData<T>(T failingData)
        {
            var formatter = formatters.FirstOrDefault(f => f.CanFormat(failingData));
            if (formatter == null)
            {
                throw new NoDataFormatterFoundException(
                    typeof (T).Name +
                    " has no formatter, add one with `Convention.Formatters.Add(new MyDataFormatter());`");
            }

            return formatter.Format(failingData);
        }
    }
}