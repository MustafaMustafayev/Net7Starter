using System.ComponentModel;

namespace ENTITIES.Enums;

public enum UserType
{
    [Description("Baş inzibatçı")] SuperAdmin = 1,
    [Description("İnzibatçı")] Admin = 2,
    [Description("İstifadəçi")] User = 3,
    [Description("Qonaq")] Guest = 4
}