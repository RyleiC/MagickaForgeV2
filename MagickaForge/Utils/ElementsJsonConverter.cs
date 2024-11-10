using System.Text.Json.Serialization;

namespace MagickaForge.Utils
{
    class EnumeratedStringAttribute : JsonConverterAttribute
    {
        public EnumeratedStringAttribute() : base(typeof(JsonStringEnumConverter))
        {

        }
    }
}