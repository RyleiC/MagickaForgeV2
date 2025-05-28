using ContentCompiler.Misc;

namespace ContentCompiler.Data
{
    public class VerifyResult
    {
        public int ErrorCount { get; private set; }
        public int WarningCount { get; private set; }

        public List<string> Errors { get; private init; }
        public List<string> Warnings { get; private init; }

        public VerifyResult()
        {
            Errors = [];
            Warnings = [];
        }

        public void Print()
        {
            foreach (var error in Errors)
            {
                Logger.WriteError(error);
            }
            foreach (var warning in Warnings)
            {
                Logger.WriteWarning(warning);
            }
        }

        public void AddWarning(string warning)
        {
            Warnings.Add("\tWarning: " + warning);
            WarningCount++;
        }

        public void AddError(string error)
        {
            Errors.Add("\tError: " + error);
            ErrorCount++;
        }

        public bool IsValidCompilation => ErrorCount == 0;
    }
}
