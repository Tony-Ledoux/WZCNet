using System.Text.Json;
using System.Text.Json.Serialization;

namespace WZCNet.Converters;

public sealed class ArrayConverter<T> : JsonConverter<T[]?>
{
    public override T[]? Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException(
                $"Expected a JSON array for '{typeToConvert.Name}'.");

        using var document = JsonDocument.ParseValue(ref reader);
        return JsonSerializer.Deserialize<T[]>(
            document.RootElement.GetRawText(), options);
    }

    public override void Write(
        Utf8JsonWriter writer,
        T[]? value,
        JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, options);
}