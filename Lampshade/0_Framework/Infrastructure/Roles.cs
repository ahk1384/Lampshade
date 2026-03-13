namespace _0_Framework.Infrastructure;

public static class Roles
{
    public const string Administrator = "3";
    public const string SystemUser = "5";
    public const string ContentUploader = "4";
    public const string ColleagueUser = "10002";

    public static string GetRoleBy(long id)
    {
        switch (id)
        {
            case 1:
                return "مدیرسیستم";
            case 3:
                return "محتوا گذار";
            default:
                return "";
        }
    }
}