using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SimpleVhd;

public sealed class Settings {
    private static readonly SettingsConverter converter = new("..\\" + SettingsFileName);
    private static readonly Lazy<Settings> _instance = new(converter.Load);
    private static readonly JsonWriterOptions indentedWriterOptions = new() { Indented = true };

    private Settings() { }

    public static Settings Instance => _instance.Value;

    public required Vhd[] VhdInstances { get; init; }
    public required Guid RamdiskGuid { get; init; }
    public required Guid PEGuid { get; init; }
    public required WorkType? WorkType { get; set; }
    public required int? WorkInstance { get; set; }
    public required string? TempValue { get; set; }

    public void SaveSettings() => converter.Save(this);

    public override string ToString() {
        MemoryStream ms = new(1024);

        using (Utf8JsonWriter writer = new(ms, indentedWriterOptions)) {
            converter.Write(writer, this, new());
        }

        return Encoding.UTF8.GetString(ms.ToArray());
    }

    private sealed class SettingsConverter(string fileName) : JsonConverter<Settings> {
        public Settings Load() {
            Utf8JsonReader reader = new(File.ReadAllBytes(fileName));

            return Read(ref reader, typeof(Settings), new());
        }

        public void Save(Settings settings) {
            using FileStream fs = new(fileName, FileMode.Truncate, FileAccess.Write, FileShare.None);
            using Utf8JsonWriter writer = new(fs, indentedWriterOptions);

            Write(writer, settings, new());
        }

        public override Settings Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            JsonObject jo = JsonNode.Parse(ref reader)!.AsObject();

            return new() {
                VhdInstances = jo[nameof(VhdInstances)]!.AsArray().Cast<JsonObject>().Select(jo => new Vhd() {
                    Directory = jo[nameof(Vhd.Directory)]!.ToString(),
                    ParentFile = jo[nameof(Vhd.ParentFile)]!.ToString(),
                    Style = parseEnum<Style>(jo[nameof(Vhd.Style)]),
                    Type = parseEnum<VhdType>(jo[nameof(Vhd.Type)]),
                    Format = parseEnum<VhdFormat>(jo[nameof(Vhd.Format)]),
                    ParentGuid = parseGuid(jo[nameof(Vhd.ParentGuid)]),
                    Child1Guid = parseGuid(jo[nameof(Vhd.Child1Guid)]),
                    Child2Guid = parseGuid(jo[nameof(Vhd.Child2Guid)]),
                }).ToArray(),
                RamdiskGuid = parseGuid(jo[nameof(RamdiskGuid)]),
                PEGuid = parseGuid(jo[nameof(PEGuid)]),
                WorkType = Enum.TryParse(jo[nameof(WorkType)]?.ToString(), out WorkType wa) ? wa : null,
                WorkInstance = int.TryParse(jo[nameof(WorkInstance)]?.ToString(), out var i) ? i : null,
                TempValue = jo[nameof(TempValue)]?.ToString(),
            };

            static TEnum parseEnum<TEnum>(JsonNode? node) where TEnum : struct, Enum => Enum.TryParse<TEnum>(node?.ToString(), out TEnum value) ? value : default;
            static Guid parseGuid(JsonNode? node) => Guid.TryParse(node?.ToString(), out Guid value) ? value : Guid.Empty;
        }

        public override void Write(Utf8JsonWriter writer, Settings value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WriteStartArray(nameof(VhdInstances));

            foreach (var vhd in value.VhdInstances) {
                writer.WriteStartObject();
                writer.WriteString(nameof(Vhd.Directory), vhd.Directory);
                writer.WriteString(nameof(Vhd.ParentFile), vhd.ParentFile);
                writer.WriteString(nameof(Vhd.Style), vhd.Style.ToString());
                writer.WriteString(nameof(Vhd.Type), vhd.Type.ToString());
                writer.WriteString(nameof(Vhd.Format), vhd.Format.ToString());
                writer.WriteString(nameof(Vhd.ParentGuid), vhd.ParentGuid.ToString("B"));
                writer.WriteString(nameof(Vhd.Child1Guid), vhd.Child1Guid.ToString("B"));
                writer.WriteString(nameof(Vhd.Child2Guid), vhd.Child2Guid.ToString("B"));
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteString(nameof(RamdiskGuid), value.RamdiskGuid.ToString("B"));
            writer.WriteString(nameof(PEGuid), value.PEGuid.ToString("B"));

            if (value.WorkType != null) {
                writer.WriteString(nameof(WorkType), value.WorkType.Value.ToString());
            }

            if (value.WorkInstance != null) {
                writer.WriteNumber(nameof(WorkInstance), value.WorkInstance.Value);
            }

            if (value.TempValue != null) {
                writer.WriteString(nameof(TempValue), value.TempValue);
            }

            writer.WriteEndObject();
        }
    }
}
