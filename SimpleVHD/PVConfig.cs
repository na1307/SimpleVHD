using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleVHD;

/// <summary>
/// SimpleVHD Config 클래스
/// </summary>
[XmlRoot("Config")]
public sealed class PVConfig : IXmlSerializable {
    private static readonly Lazy<PVConfig> _Instance = new(() => new());
    private readonly string xPath;
    private readonly Dictionary<DoAction, bool> shutdownAfter;
    private readonly Dictionary<GuidType, Guid> bcdGuids;
    private VhdFormat _VhdFormat;

    /// <summary>
    /// PVConfig의 현재 인스턴스
    /// </summary>
    public static PVConfig Instance => _Instance.Value;

    /// <summary>
    /// 현재 윈도우 버전
    /// </summary>
    public WindowsVersion WindowsVersion { get; private set; }

    /// <summary>
    /// 현재 운영 스타일
    /// </summary>
    public OperatingStyle OperatingStyle { get; set; }

    /// <summary>
    /// 현재 VHD 형식
    /// </summary>
    public VhdType VhdType { get; set; }

    /// <summary>
    /// 현재 VHD 포맷
    /// </summary>
    public VhdFormat VhdFormat {
        get => _VhdFormat;
        set {
            _VhdFormat = value;
            if (VhdFile != null) VhdFile = Regex.Match(VhdFile, "^(?<filename>.+\\.)vhdx?$", RegexOptions.IgnoreCase).Groups["filename"].Value + _VhdFormat.ToString().ToLower();
        }
    }

    /// <summary>
    /// 현재 VHD 디렉토리 경로
    /// </summary>
    public string VhdDirectory { get; private set; }

    /// <summary>
    /// 현재 VHD 파일 이름
    /// </summary>
    public string VhdFile { get; private set; }

    /// <summary>
    /// 현재 작업
    /// </summary>
    public DoAction Action { get; set; }

    /// <summary>
    /// 현재 임시 필드
    /// </summary>
    public string? Temp { get; set; }

    /// <summary>
    /// 현재 ShutdownAfterAction
    /// </summary>
    /// <param name="type">해당 작업 ShutdownType</param>
    /// <returns>해당 작업에서 다시 시작하는 대신 종료해야 하는지 여부</returns>
    public bool this[DoAction type] {
        get => shutdownAfter[type];
        set => shutdownAfter[type] = value;
    }

    /// <summary>
    /// 현재 BCD GUID
    /// </summary>
    /// <param name="type">해당 GUID 종류</param>
    /// <returns>GUID 문자열</returns>
    public string this[GuidType type] => bcdGuids[type].ToString("B");

    private PVConfig() {
        try {
            var drvs = from d in DriveInfo.GetDrives()
                       where d.CheckFixed() && File.Exists(d.Name + DirName + Path.DirectorySeparatorChar.ToString() + ConfigName)
                       select d.Name;

            xPath = drvs.Any() ? drvs.First() + DirName + Path.DirectorySeparatorChar.ToString() + ConfigName : throw new PVConfigException(ConfigFileNotFoundMessage);

            shutdownAfter = new(4) {
                { DoAction.DoBackup, default },
                { DoAction.DoRestore, default },
                { DoAction.DoRevert, default },
                { DoAction.DoMerge, default }
            };

            bcdGuids = new(5) {
                { GuidType.Parent, Guid.Empty },
                { GuidType.Child1, Guid.Empty },
                { GuidType.Child2, Guid.Empty },
                { GuidType.Ramdisk, Guid.Empty },
                { GuidType.PE, Guid.Empty }
            };

            ((IXmlSerializable)this).ReadXml(XmlReader.Create(xPath));

            if (VhdDirectory == null) throw new InvalidConfigException("설정 파일에 " + nameof(VhdDirectory) + " 항목이 없습니다.");
            if (VhdFile == null) throw new InvalidConfigException("설정 파일에" + nameof(VhdFile) + " 항목이 없습니다.");

            var emptyGuids = bcdGuids.Where(g => g.Value == Guid.Empty);

            if (emptyGuids.Any()) throw new InvalidConfigException("설정 파일에 " + emptyGuids.First().Key.ToString() + " Guid가 없습니다.");
        } catch (PVException) {
            throw;
        } catch (Exception ex) {
            throw new InvalidConfigException(ex.Message, ex);
        }
    }

    public override string ToString() {
        System.Text.StringBuilder sb = new(1000);

        ((IXmlSerializable)this).WriteXml(XmlWriter.Create(sb, new() { Indent = true }));

        return sb.ToString();
    }

    public void SaveConfig() => ((IXmlSerializable)this).WriteXml(XmlWriter.Create(xPath, new() { Indent = true }));

    System.Xml.Schema.XmlSchema? IXmlSerializable.GetSchema() => null;

    void IXmlSerializable.ReadXml(XmlReader reader) {
        if (reader == null) throw new ArgumentNullException(nameof(reader));

        reader.MoveToContent();

        while (reader.Read()) {
            if (reader.NodeType == XmlNodeType.Element) {
                switch (reader.LocalName) {
                    case "ShutdownAfterAction":
                        shutdownAfter[EnumParser.Parse<DoAction>("Do" + reader.GetAttribute("Type"), false)] = reader.ReadElementContentAsBoolean();
                        break;

                    case "Guid":
                        bcdGuids[EnumParser.Parse<GuidType>(reader.GetAttribute("Type"), false)] = Guid.Parse(reader.ReadElementContentAsString());
                        break;

                    default:
                        var p = GetType().GetProperty(reader.LocalName);
                        var v = reader.ReadElementContentAsString();

                        p.SetValue(this, !p.PropertyType.IsEnum ? v : Enum.Parse(p.PropertyType, v, true), null);
                        break;
                }
            }
        }

        reader.Close();
    }

    void IXmlSerializable.WriteXml(XmlWriter writer) {
        if (writer == null) throw new ArgumentNullException(nameof(writer));

        writer.WriteStartDocument();
        writer.WriteComment(ConfigComment);
        writer.WriteStartElement("Config");

        foreach (var p in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.Name != "Item")) {
            switch (p.Name) {
                case nameof(Temp):
                    if (Temp != null) writer.WriteElementString(nameof(Temp), Temp);
                    break;

                default:
                    writer.WriteElementString(p.Name, p.GetValue(this, null).ToString());
                    break;
            }
        }

        foreach (var shutdown in shutdownAfter) {
            writer.WriteStartElement("ShutdownAfterAction");
            writer.WriteAttributeString("Type", shutdown.Key.ToString().Substring(2));
            writer.WriteString(shutdown.Value.ToString().ToLower());
            writer.WriteEndElement();
        }

        foreach (var guid in bcdGuids) {
            writer.WriteStartElement("Guid");
            writer.WriteAttributeString("Type", guid.Key.ToString());
            writer.WriteString(guid.Value.ToString("B"));
            writer.WriteEndElement();
        }

        writer.WriteEndElement();
        writer.WriteEndDocument();
        writer.Close();
    }

    private class PVConfigException : PVException {
        public PVConfigException(string message) : base(message) { }
        public PVConfigException(string message, Exception innerException) : base(message, innerException) { }
    }

    private sealed class InvalidConfigException : PVConfigException {
        private const string dMessage = "설정 파일이 잘못되었습니다. " + ConfigName + " 파일을 살펴보세요.";

        public InvalidConfigException(string reason) : base(dMessage + "\r\n\r\n" + reason) { }
        public InvalidConfigException(string reason, Exception innerException) : base(dMessage + "\r\n\r\n" + reason, innerException) { }
    }
}