using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gameworld.Domain;

/// <summary>
/// The json form of the domain model, for storage and for data transfer between backend and
/// frontend clients. Grid coordinates and identities are written as [i, j], world points as
/// [x, y] and enums as their names.
/// </summary>
public static class DomainJson
{
    public static JsonSerializerOptions Options { get; } = Create(indented: false);

    public static JsonSerializerOptions Indented { get; } = Create(indented: true);

    private static JsonSerializerOptions Create(bool indented) => new()
    {
        WriteIndented = indented,
        IndentSize = 2,
        NewLine = "\n",
        NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
        Converters =
        {
            new JsonStringEnumConverter(),
            new GridCoordConverter(),
            new IdConverter<RegionId>(c => new RegionId(c), id => id.Coord),
            new IdConverter<ZoneCellId>(c => new ZoneCellId(c), id => id.Coord),
            new IdConverter<PatchCellId>(c => new PatchCellId(c), id => id.Coord),
            new IdConverter<ZoneId>(c => new ZoneId(new ZoneCellId(c)), id => id.Coord),
            new IdConverter<PatchId>(c => new PatchId(new PatchCellId(c)), id => id.Coord),
            new WorldPointConverter(),
            new SeedConverter(),
            new FloatArrayConverter(),
        },
    };

    public static string Serialize<T>(T value, bool indented = false) =>
        JsonSerializer.Serialize(value, indented ? Indented : Options);

    public static T Deserialize<T>(string json) =>
        JsonSerializer.Deserialize<T>(json, Options) ?? throw new JsonException("The json is empty.");

    private static GridCoord ReadCoord(ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException("Expected [i, j].");
        reader.Read();
        var i = reader.GetInt32();
        reader.Read();
        var j = reader.GetInt32();
        reader.Read();
        if (reader.TokenType != JsonTokenType.EndArray) throw new JsonException("Expected [i, j].");
        return new GridCoord(i, j);
    }

    private static void WriteCoord(Utf8JsonWriter writer, GridCoord c)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(c.I);
        writer.WriteNumberValue(c.J);
        writer.WriteEndArray();
    }

    private sealed class GridCoordConverter : JsonConverter<GridCoord>
    {
        public override GridCoord Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) => ReadCoord(ref reader);
        public override void Write(Utf8JsonWriter writer, GridCoord value, JsonSerializerOptions options) => WriteCoord(writer, value);
    }

    private sealed class IdConverter<T>(Func<GridCoord, T> make, Func<T, GridCoord> coord) : JsonConverter<T>
    {
        public override T Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) => make(ReadCoord(ref reader));
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => WriteCoord(writer, coord(value));
    }

    private sealed class WorldPointConverter : JsonConverter<WorldPoint>
    {
        public override WorldPoint Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException("Expected [x, y].");
            reader.Read();
            var x = reader.GetDouble();
            reader.Read();
            var y = reader.GetDouble();
            reader.Read();
            return new WorldPoint(x, y);
        }

        public override void Write(Utf8JsonWriter writer, WorldPoint value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
    }

    /// <summary>A raster of floats is written as base64 of its little-endian bytes.</summary>
    private sealed class FloatArrayConverter : JsonConverter<float[]>
    {
        public override float[] Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            var bytes = reader.GetBytesFromBase64();
            var values = new float[bytes.Length / sizeof(float)];
            for (var i = 0; i < values.Length; i++)
                values[i] = System.Buffers.Binary.BinaryPrimitives.ReadSingleLittleEndian(bytes.AsSpan(i * sizeof(float)));
            return values;
        }

        public override void Write(Utf8JsonWriter writer, float[] value, JsonSerializerOptions options)
        {
            var bytes = new byte[value.Length * sizeof(float)];
            for (var i = 0; i < value.Length; i++)
                System.Buffers.Binary.BinaryPrimitives.WriteSingleLittleEndian(bytes.AsSpan(i * sizeof(float)), value[i]);
            writer.WriteBase64StringValue(bytes);
        }
    }

    /// <summary>A seed is its text; the number is made from it again on reading.</summary>
    private sealed class SeedConverter : JsonConverter<Seed>
    {
        public override Seed Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options) =>
            Seed.FromText(reader.GetString() ?? string.Empty);

        public override void Write(Utf8JsonWriter writer, Seed value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.Text);
    }
}
