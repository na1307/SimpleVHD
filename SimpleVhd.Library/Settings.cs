using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SimpleVhd;

public sealed class Settings {
    private static readonly SettingsConverter converter = new(Path.Combine(SVPath, SettingsFileName));
    private static readonly Lazy<Settings> _instance = new(converter.Load);
    private static readonly JsonWriterOptions indentedWriterOptions = new() { Indented = true };

    private Settings() { }

    public static Settings Instance => _instance.Value;

    public required Vhd[] Instances { get; init; }
    public required Guid RamdiskGuid { get; init; }
    public required Guid PEGuid { get; init; }
    public required OperationType? OperationType { get; set; }
    public required int? InstanceToOperationOn { get; set; }
    public required string? OperationTempValue { get; set; }

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
            var jo = JsonNode.Parse(ref reader)!.AsObject();

            return new() {
                Instances = jo[nameof(Instances)]!.AsArray().Cast<JsonObject>().Select(jo => new Vhd() {
                    Name = jo[nameof(Vhd.Name)]!.ToString(),
                    Directory = jo[nameof(Vhd.Directory)]!.ToString(),
                    FileName = jo[nameof(Vhd.FileName)]!.ToString(),
                    Style = parseEnum<Style>(jo[nameof(Vhd.Style)]),
                    Type = parseEnum<VhdType>(jo[nameof(Vhd.Type)]),
                    Format = parseEnum<VhdFormat>(jo[nameof(Vhd.Format)]),
                    ParentGuid = parseGuid(jo[nameof(Vhd.ParentGuid)]),
                }).ToArray(),
                RamdiskGuid = parseGuid(jo[nameof(RamdiskGuid)]),
                PEGuid = parseGuid(jo[nameof(PEGuid)]),
                OperationType = Enum.TryParse(jo[nameof(OperationType)]?.ToString(), out OperationType wa) ? wa : null,
                InstanceToOperationOn = int.TryParse(jo[nameof(InstanceToOperationOn)]?.ToString(), out var i) ? i : null,
                OperationTempValue = jo[nameof(OperationTempValue)]?.ToString(),
            };

            static TEnum parseEnum<TEnum>(JsonNode? node) where TEnum : struct, Enum => Enum.TryParse<TEnum>(node?.ToString(), out TEnum value) ? value : default;
            static Guid parseGuid(JsonNode? node) => Guid.TryParse(node?.ToString(), out var value) ? value : Guid.Empty;
        }

        public override void Write(Utf8JsonWriter writer, Settings value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WriteString("$schema", SettingsSchemaUrl);
            writer.WriteStartArray(nameof(Instances));

            foreach (var vhd in value.Instances) {
                writer.WriteStartObject();
                writer.WriteString(nameof(Vhd.Name), vhd.Name);
                writer.WriteString(nameof(Vhd.Directory), vhd.Directory);
                writer.WriteString(nameof(Vhd.FileName), vhd.FileName);
                writer.WriteString(nameof(Vhd.Style), vhd.Style.ToString());
                writer.WriteString(nameof(Vhd.Type), vhd.Type.ToString());
                writer.WriteString(nameof(Vhd.Format), vhd.Format.ToString());
                writer.WriteString(nameof(Vhd.ParentGuid), vhd.ParentGuid.ToString("B"));
                writer.WriteEndObject();
            }

            writer.WriteEndArray();
            writer.WriteString(nameof(RamdiskGuid), value.RamdiskGuid.ToString("B"));
            writer.WriteString(nameof(PEGuid), value.PEGuid.ToString("B"));

            if (value.OperationType != null) {
                writer.WriteString(nameof(OperationType), value.OperationType.Value.ToString());
            }

            if (value.InstanceToOperationOn != null) {
                writer.WriteNumber(nameof(InstanceToOperationOn), value.InstanceToOperationOn.Value);
            }

            if (value.OperationTempValue != null) {
                writer.WriteString(nameof(OperationTempValue), value.OperationTempValue);
            }

            writer.WriteEndObject();
        }
    }
}
