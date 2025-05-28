namespace ContentCompiler.Data
{
    internal static class CompilingHelper
    {
        public static bool IsDirectory(string path)
        {
            FileAttributes fileAttributes = File.GetAttributes(path);
            return fileAttributes.HasFlag(FileAttributes.Directory);
        }
    }
}
