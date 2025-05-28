using ContentCompiler.Misc;

namespace ContentCompiler.Data
{
    public class VerifyResult
    {
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }

        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }

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
