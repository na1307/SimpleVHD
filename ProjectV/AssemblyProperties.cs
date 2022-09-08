using System.Reflection;

namespace ProjectV;

public static class AssemblyProperties {
    public static string AssemblyTitle {
        get {
            var attribute = (AssemblyTitleAttribute?)GetAttribute(typeof(AssemblyTitleAttribute));

            return attribute != null && !string.IsNullOrEmpty(attribute.Title) ? attribute.Title : Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
        }
    }

    public static string VersionNumber {
        get {
            var attribute = (AssemblyFileVersionAttribute?)GetAttribute(typeof(AssemblyFileVersionAttribute));

            if (attribute != null) {
                var version = Version.Parse(attribute.Version);

                return version.ToString(2) + (version.Build == 0 ? string.Empty : "." + version.Build.ToString());
            } else {
                var version = Version.Parse(((AssemblyVersionAttribute?)GetAttribute(typeof(AssemblyVersionAttribute)))!.Version);

                return version.ToString(2) + (version.Build == 0 ? string.Empty : "." + version.Build.ToString());
            }
        }
    }

    public static string AssemblyInformationalVersion {
        get {
            var attribute = (AssemblyInformationalVersionAttribute?)GetAttribute(typeof(AssemblyInformationalVersionAttribute));

            return attribute != null ? attribute.InformationalVersion : VersionNumber;
        }
    }

    public static string AssemblyDescription {
        get {
            var attribute = (AssemblyDescriptionAttribute?)GetAttribute(typeof(AssemblyDescriptionAttribute));

            return attribute != null ? attribute.Description : string.Empty;
        }
    }

    public static string AssemblyProduct {
        get {
            var attribute = (AssemblyProductAttribute?)GetAttribute(typeof(AssemblyProductAttribute));

            return attribute != null ? attribute.Product : string.Empty;
        }
    }

    public static string AssemblyCopyright {
        get {
            var attribute = (AssemblyCopyrightAttribute?)GetAttribute(typeof(AssemblyCopyrightAttribute));

            return attribute != null ? attribute.Copyright : string.Empty;
        }
    }

    public static string AssemblyCompany {
        get {
            var attribute = (AssemblyCompanyAttribute?)GetAttribute(typeof(AssemblyCompanyAttribute));

            return attribute != null ? attribute.Company : string.Empty;
        }
    }

    private static Attribute? GetAttribute(Type type) => Attribute.GetCustomAttribute(Assembly.GetEntryAssembly(), type);
}