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

        //using var document = JsonDocument.ParseValue(ref reader);
        //return JsonSerializer.Deserialize<T[]>(
        //    document.RootElement.GetRawText(), options);
        using var document = JsonDocument.ParseValue(ref reader);
        var results = new List<T>();

        int index = 0;
        foreach (var element in document.RootElement.EnumerateArray())
        {
            try
            {
                var item = JsonSerializer.Deserialize<T>(element.GetRawText(), options) ?? throw new JsonException(
                        $"Please provide a valid {typeof(T).Name} element at index [{index}].");
                results.Add(item);
            }
            catch (JsonException)
            {
                throw new JsonException(
                    $"Please provide a valid {typeof(T).Name} element at index [{index}].");
            }

            index++;
        }

        return results.ToArray();
    }

    public override void Write(
        Utf8JsonWriter writer,
        T[]? value,
        JsonSerializerOptions options)
        => JsonSerializer.Serialize(writer, value, options);
}