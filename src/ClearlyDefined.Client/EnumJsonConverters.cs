namespace ClearlyDefined.Client;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

internal sealed class ComponentTypeJsonConverter : JsonConverter<ComponentType>
{
    public override ComponentType Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => EnumExtensions.ToComponentType(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        ComponentType value,
        JsonSerializerOptions options
    ) => writer.WriteStringValue(value.ToApiString());
}

internal sealed class ComponentProviderJsonConverter : JsonConverter<ComponentProvider>
{
    public override ComponentProvider Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options
    ) => EnumExtensions.ToComponentProvider(reader.GetString()!);

    public override void Write(
        Utf8JsonWriter writer,
        ComponentProvider value,
        JsonSerializerOptions options
    ) => writer.WriteStringValue(value.ToApiString());
}
