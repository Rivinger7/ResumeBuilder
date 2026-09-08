using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ResumeBuilder.Domain.Common;

[JsonConverter(typeof(OptionalJsonConverterFactory))]
public readonly struct Optional<T>
{
    public bool IsSpecified { get; }
    public T? Value { get; }

    public Optional(T? value, bool isSpecified)
    {
        Value = value;
        IsSpecified = isSpecified;
    }

    public static Optional<T> Unspecified => new(default, false);

    public static implicit operator Optional<T>(T? value) => new(value, true);
}

internal sealed class OptionalJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType) return false;
        return typeToConvert.GetGenericTypeDefinition() == typeof(Optional<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type valueType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(OptionalJsonConverter<>).MakeGenericType(valueType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}

internal sealed class OptionalJsonConverter<T> : JsonConverter<Optional<T>>
{
    public override Optional<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Deserialize the value (this will handle null and non-null tokens and advance the reader)
        T? value = JsonSerializer.Deserialize<T?>(ref reader, options);
        return new Optional<T>(value, true);
    }

    public override void Write(Utf8JsonWriter writer, Optional<T> value, JsonSerializerOptions options)
    {
        if (!value.IsSpecified)
        {
            // If not specified, write nothing – serialize as null to preserve expected behavior in outputs
            writer.WriteNullValue();
            return;
        }

        JsonSerializer.Serialize(writer, value.Value, options);
    }
}
