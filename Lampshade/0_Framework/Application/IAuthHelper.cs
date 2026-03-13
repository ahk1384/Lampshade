namespace _0_Framework.Application;

public interface IAuthHelper
{
    void SignOut();
    bool IsAuthenticated();
    void Signin(AuthViewModel account);
    string CurrentAccountRole();
    AuthViewModel CurrentAccountInfo();
    List<int> GetPermissions();
    List<string> GetPermissionsStrings();
    long CurrentAccountId();
    string CurrentAccountMobile();
}