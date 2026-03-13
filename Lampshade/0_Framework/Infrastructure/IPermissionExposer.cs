using System.Collections.Generic;

namespace _0_Framework.Infrastructure;

public interface IPermissionExposer
{
    Dictionary<String, List<PermissionDto>> Expose();
}