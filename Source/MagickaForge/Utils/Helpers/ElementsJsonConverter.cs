using System.Text.Json.Serialization;

namespace MagickaForge.Utils.Helpers
{
    class EnumeratedStringAttribute : JsonConverterAttribute
    {
        public EnumeratedStringAttribute() : base(typeof(JsonStringEnumConverter))
        {

        }
    }
}