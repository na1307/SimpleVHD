using System.Reflection;

namespace ProjectV;

public static class AssemblyProperties {
    public static string AssemblyTitle {
        get {
            var attribute = GetAttribute<AssemblyTitleAttribute>();

            return attribute != null && !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
        }
    }

    public static string VersionNumber {
        get {
            var attribute = GetAttribute<AssemblyFileVersionAttribute>();
            var version = Version.Parse(attribute != null ? attribute.Version : GetAttribute<AssemblyVersionAttribute>()!.Version);

            return version.ToString(2) + (version.Build == 0 ? string.Empty : "." + version.Build.ToString());
        }
    }

    public static string AssemblyInformationalVersion {
        get {
            var attribute = GetAttribute<AssemblyInformationalVersionAttribute>();

            return attribute != null ? attribute.InformationalVersion : VersionNumber;
        }
    }

    public static string AssemblyDescription {
        get {
            var attribute = GetAttribute<AssemblyDescriptionAttribute>();

            return attribute != null ? attribute.Description : string.Empty;
        }
    }

    public static string AssemblyProduct {
        get {
            var attribute = GetAttribute<AssemblyProductAttribute>();

            return attribute != null ? attribute.Product : string.Empty;
        }
    }

    public static string AssemblyCopyright {
        get {
            var attribute = GetAttribute<AssemblyCopyrightAttribute>();

            return attribute != null ? attribute.Copyright : string.Empty;
        }
    }

    public static string AssemblyCompany {
        get {
            var attribute = GetAttribute<AssemblyCompanyAttribute>();

            return attribute != null ? attribute.Company : string.Empty;
        }
    }

    private static T? GetAttribute<T>() where T : Attribute => (T?)Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), typeof(T));
}