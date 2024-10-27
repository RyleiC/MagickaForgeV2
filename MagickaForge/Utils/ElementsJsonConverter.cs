using System.Text.Json.Serialization;

class JsonStringEnumAttribute : JsonConverterAttribute
{
    public JsonStringEnumAttribute() : base(typeof(JsonStringEnumConverter))
    {

    }
}