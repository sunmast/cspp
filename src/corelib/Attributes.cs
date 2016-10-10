namespace System
{
    [Imported]
    public enum AttributeTargets
    {
        Assembly = 0x0001,
        Module = 0x0002,
        Class = 0x0004,
        Struct = 0x0008,
        Enum = 0x0010,
        Constructor = 0x0020,
        Method = 0x0040,
        Property = 0x0080,
        Field = 0x0100,
        Event = 0x0200,
        Interface = 0x0400,
        Parameter = 0x0800,
        Delegate = 0x1000,
        ReturnValue = 0x2000,
        GenericParameter = 0x4000,
        Type = Class | Struct | Enum | Interface | Delegate,
        All = Assembly | Module | Class | Struct | Enum | Constructor |
              Method | Property | Field | Event | Interface | Parameter |
              Delegate | ReturnValue | GenericParameter,
    }

    [Imported, AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class AttributeUsageAttribute : Attribute
    {
        public AttributeTargets ValidOn { get; private set; }

        public bool AllowMultiple { get; set; }

        public bool Inherited { get; set; }

        public AttributeUsageAttribute(AttributeTargets validOn)
        {
            this.ValidOn = validOn;
            this.Inherited = true;
        }
    }
}

#region System.Reflection

namespace System.Reflection
{
    [Imported]
    public sealed class DefaultMemberAttribute
    {
        private string _memberName;

        public DefaultMemberAttribute(string memberName)
        {
            _memberName = memberName;
        }

        public string MemberName
        {
            get
            {
                return _memberName;
            }
        }
    }

    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyCopyrightAttribute : Attribute
    {

        private string _copyright;

        public AssemblyCopyrightAttribute(string copyright)
        {
            _copyright = copyright;
        }

        public string Copyright
        {
            get
            {
                return _copyright;
            }
        }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyTrademarkAttribute : Attribute
    {

        private string _trademark;

        public AssemblyTrademarkAttribute(string trademark)
        {
            _trademark = trademark;
        }

        public string Trademark
        {
            get
            {
                return _trademark;
            }
        }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyProductAttribute : Attribute
    {

        private string _product;

        public AssemblyProductAttribute(string product)
        {
            _product = product;
        }

        public string Product
        {
            get
            {
                return _product;
            }
        }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyCompanyAttribute : Attribute
    {

        private string _company;

        public AssemblyCompanyAttribute(string company)
        {
            _company = company;
        }

        public string Company
        {
            get
            {
                return _company;
            }
        }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyDescriptionAttribute : Attribute
    {

        private string _description;

        public AssemblyDescriptionAttribute(string description)
        {
            _description = description;
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyTitleAttribute : Attribute
    {

        private string _title;

        public AssemblyTitleAttribute(string title)
        {
            _title = title;
        }

        public string Title
        {
            get
            {
                return _title;
            }
        }
    }

    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyConfigurationAttribute : Attribute
    {

        private string _configuration;

        public AssemblyConfigurationAttribute(string configuration)
        {
            _configuration = configuration;
        }

        public string Configuration
        {
            get
            {
                return _configuration;
            }
        }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyFileVersionAttribute : Attribute
    {

        private string _version;

        public AssemblyFileVersionAttribute(string version)
        {
            _version = version;
        }

        public string Version
        {
            get
            {
                return _version;
            }
        }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyInformationalVersionAttribute : Attribute
    {
        public AssemblyInformationalVersionAttribute(string informationalVersion)
        {
            this.InformationalVersion = informationalVersion;
        }

        public string InformationalVersion { get; private set; }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyCultureAttribute : Attribute
    {
        public AssemblyCultureAttribute(string culture)
        {
            this.Culture = culture;
        }

        public string Culture { get; private set; }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyVersionAttribute : Attribute
    {
        public AssemblyVersionAttribute(string version)
        {
            this.Version = version;
        }

        public string Version { get; private set; }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyKeyFileAttribute : Attribute
    {
        public AssemblyKeyFileAttribute(string keyFile)
        {
            this.KeyFile = keyFile;
        }

        public string KeyFile { get; private set; }
    }


    [Imported, AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
    public sealed class AssemblyDelaySignAttribute : Attribute
    {
        public AssemblyDelaySignAttribute(bool delaySign)
        {
            this.DelaySign = delaySign;
        }

        public bool DelaySign { get; private set; }
    }
}

#endregion