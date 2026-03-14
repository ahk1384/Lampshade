namespace _0_Framework.Application;

public static class PermissionsCodes
{
    public static Dictionary<string, int> Codes = new()
    {
        { "Admin", 1 }
    };

    public static int GetCode(string policyName)
    {
        return Codes.TryGetValue(policyName, out var code) ? code : 0;
    }

    public static void AddCode(string policyName, int code)
    {
        Codes.Add(policyName, code);
    }
    
}