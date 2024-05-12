using System.ComponentModel;

namespace CORE.Enums;

public enum EUserType
{
    [Description("Baş inzibatçı")] SuperAdmin = 1,
    [Description("İnzibatçı")] Admin = 2,
    [Description("İstifadəçi")] User = 3,
    [Description("Qonaq")] Guest = 4
}