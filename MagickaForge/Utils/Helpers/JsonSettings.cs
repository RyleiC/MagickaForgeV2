using System.Text.Json;

namespace MagickaForge.Utils.Helpers
{
    public static class JsonSettings
    {
        private readonly static JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true,
        };

        public static JsonSerializerOptions SerializerSettings
        {
            get
            {
                return JsonOptions;
            }
        }
    }
}
