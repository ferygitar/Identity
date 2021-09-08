using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.ViewModels.Role;

namespace Identity.Repositories
{
    public interface IUtilities
    {
        public IList<ActionAndControllerName> AreaAndActionAndControllerNamesList();
        public IList<string> GetAllAreasNames();
        public string DataBaseRoleValidationGuid();
    }
}
