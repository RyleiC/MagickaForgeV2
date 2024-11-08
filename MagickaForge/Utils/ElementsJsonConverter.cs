using System.Text.Json.Serialization;

class EnumeratedStringAttribute : JsonConverterAttribute
{
    public EnumeratedStringAttribute() : base(typeof(JsonStringEnumConverter))
    {

    }
}