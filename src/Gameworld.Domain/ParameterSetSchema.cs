using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;

namespace Gameworld.Domain;

/// <summary>
/// The JSON Schema of a Parameter Set json file, generated from the <see cref="ParameterSet"/>
/// types: the description and range of every parameter, the allowed values of the enums and
/// the default of every value. docs/parameter-set.schema.json is this schema; an editor that
/// knows it shows the descriptions as help text and flags unknown names and values out of range.
/// </summary>
public static class ParameterSetSchema
{
    public const string FileName = "parameter-set.schema.json";

    public static JsonNode Build()
    {
        var exporterOptions = new JsonSchemaExporterOptions
        {
            TreatNullObliviousAsNonNullable = true,
            TransformSchemaNode = Describe,
        };
        ParameterSet.JsonOptions.MakeReadOnly(populateMissingResolver: true);
        var schema = (JsonObject)ParameterSet.JsonOptions.GetJsonSchemaAsNode(typeof(ParameterSet), exporterOptions);

        // The exporter points repeated types at their first use; write every use out in full,
        // so each one can carry its own default.
        Inline(schema, schema);
        SetDefaults(schema, JsonNode.Parse(ParameterSet.Default.ToJson())!);

        var properties = (JsonObject)schema["properties"]!;
        properties.Insert(0, "$schema", new JsonObject
        {
            ["description"] = "The address of this schema, so an editor can show help for the file. Ignored when the file is loaded.",
            ["type"] = "string",
        });
        schema.Insert(0, "$schema", "https://json-schema.org/draft/2020-12/schema");
        schema.Insert(1, "title", "Gameworld Parameter Set");
        return schema;
    }

    public static string ToJson() =>
        Build().ToJsonString(new JsonSerializerOptions
        {
            WriteIndented = true,
            IndentSize = 2,
            NewLine = "\n",
            // Write §, × and → as they are: the file is read by people too.
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        });

    public static void Save(string path) => File.WriteAllText(path, ToJson() + "\n");

    private static JsonNode Describe(JsonSchemaExporterContext context, JsonNode node)
    {
        if (node is not JsonObject schema) return node;

        ICustomAttributeProvider? source = context.PropertyInfo is { } property
            ? property.AttributeProvider
            : context.Path.Length == 0 ? context.TypeInfo.Type : null;

        if (source?.GetCustomAttributes(typeof(RangeAttribute), inherit: false).FirstOrDefault() is RangeAttribute range)
        {
            var min = Convert.ToDouble(range.Minimum);
            var max = Convert.ToDouble(range.Maximum);
            if (!double.IsNegativeInfinity(min)) schema["minimum"] = JsonValue.Create(min);
            // An upper bound at the largest number of the type means "no upper bound".
            if (!double.IsPositiveInfinity(max) && max < int.MaxValue) schema["maximum"] = JsonValue.Create(max);
        }

        // The description of the parameter; for an item of a list, the description of its type.
        var description =
            source?.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false).FirstOrDefault() as DescriptionAttribute
            ?? (context.PropertyInfo is null ? context.TypeInfo.Type.GetCustomAttribute<DescriptionAttribute>() : null);
        if (description != null) schema.Insert(0, "description", description.Description);

        // A name that is not a parameter is most likely a typo.
        if (schema["properties"] is JsonObject) schema["additionalProperties"] = false;
        return node;
    }

    /// <summary>Replaces every "$ref" into this document with a copy of what it points at.</summary>
    private static void Inline(JsonObject root, JsonNode node)
    {
        if (node is JsonArray array)
        {
            foreach (var item in array)
                if (item != null) Inline(root, item);
            return;
        }
        if (node is not JsonObject obj) return;

        if (obj["$ref"] is JsonValue reference && reference.GetValue<string>() is { } pointer && pointer.StartsWith('#'))
        {
            JsonNode target = root;
            foreach (var part in pointer.TrimStart('#').Split('/', StringSplitOptions.RemoveEmptyEntries))
                target = target is JsonArray a
                    ? a[int.Parse(part)]!
                    : target[part.Replace("~1", "/").Replace("~0", "~")]!;
            var copy = (JsonObject)target.DeepClone();
            var own = obj.Where(p => p.Key != "$ref").Select(p => (p.Key, Value: p.Value?.DeepClone())).ToArray();
            obj.Clear();
            // What the use says about itself (its description) comes first and wins over the copy.
            foreach (var (key, value) in own) obj[key] = value;
            foreach (var (key, value) in copy)
                if (!obj.ContainsKey(key)) obj[key] = value?.DeepClone();
        }

        foreach (var (_, child) in obj.ToArray())
            if (child != null) Inline(root, child);
    }

    /// <summary>Writes the default of every number, text, flag and list of numbers into the schema.</summary>
    private static void SetDefaults(JsonObject schema, JsonNode defaults)
    {
        if (schema["properties"] is not JsonObject properties || defaults is not JsonObject values) return;
        foreach (var (name, propertySchema) in properties)
        {
            if (propertySchema is not JsonObject property || values[name] is not { } value) continue;
            switch (value)
            {
                case JsonValue:
                    property["default"] = value.DeepClone();
                    break;
                case JsonArray list when list.All(item => item is JsonValue):
                    property["default"] = value.DeepClone();
                    break;
                case JsonObject:
                    SetDefaults(property, value);
                    break;
            }
        }
    }
}
