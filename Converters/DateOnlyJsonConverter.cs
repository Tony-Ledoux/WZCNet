using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WZCNet.Converters;

public class DateOnlyJsonConverter: JsonConverter<DateOnly>
{
    private static readonly string[] _formats = ["yyyy-MM-dd","dd/MM/yyyy", "MM/dd/yyyy"];
    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        foreach (var format in _formats)
        {
            if(DateOnly.TryParseExact(value,format,out var date)) return date;
        }
        return default;
    }
    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
    }
}
