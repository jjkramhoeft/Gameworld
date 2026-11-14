using System.Text.Json;
using System.Text.Json.Serialization;
using Gameworld.Domain.Model;

namespace Gameworld.App;

/// <summary>Reads and writes a parameter set as a JSON file, so it can be edited by hand.</summary>
internal static class ParameterFile
{
    private static readonly JsonSerializerOptions Json = new()
    {
        WriteIndented = true,
        RespectRequiredConstructorParameters = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public static ParameterSet Load(string path)
    {
        using var file = File.OpenRead(path);
        return JsonSerializer.Deserialize<ParameterSet>(file, Json)
            ?? throw new JsonException($"'{path}' does not hold a parameter set.");
    }

    public static void Save(ParameterSet parameters, string path)
    {
        CreateFolderOf(path);
        File.WriteAllText(path, JsonSerializer.Serialize(parameters, Json));
    }

    public static void CreateFolderOf(string path)
    {
        var folder = Path.GetDirectoryName(Path.GetFullPath(path));
        if (!string.IsNullOrEmpty(folder))
            Directory.CreateDirectory(folder);
    }
}
