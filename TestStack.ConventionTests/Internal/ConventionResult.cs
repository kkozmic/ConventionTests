namespace TestStack.ConventionTests.Internal
{
    public class ConventionResult
    {
        public ConventionResult(object[] result, string conventionTitle, string dataDescription, bool hasApprovedExceptions)
        {
            HasApprovedExceptions = hasApprovedExceptions;
            Result = result;
            ConventionTitle = conventionTitle;
            DataDescription = dataDescription;
        }

        public object[] Result { get; private set; }
        public string ConventionTitle { get; private set; }
        public string DataDescription { get; private set; }
        public bool HasApprovedExceptions { get; private set; }
    }
}