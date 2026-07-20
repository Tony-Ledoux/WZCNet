using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WZCNet.src.Application.Converters;

public sealed class ArrayConverterFactory:JsonConverterFactory
{
public override bool CanConvert(Type typeToConvert)
        => typeToConvert.IsArray && !typeToConvert.GetElementType()!.IsAbstract;

    public override JsonConverter? CreateConverter(
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        var elementType = typeToConvert.GetElementType()!;
        var converterType = typeof(ArrayConverter<>).MakeGenericType(elementType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}
