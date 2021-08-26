using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Repositories
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims=new List<Claim>()
        {
            new Claim(ClaimTypesStore.EmployeeList,true.ToString()),
            new Claim(ClaimTypesStore.EmployeeDetails,true.ToString()),
            new Claim(ClaimTypesStore.EmployeeEdit,true.ToString()),
            new Claim(ClaimTypesStore.EmployeeAdd,true.ToString()),

        };
    }

    public static class ClaimTypesStore
    {
        public const string EmployeeList = "EmployeeList";
        public const string EmployeeDetails = "EmployeeDetails";
        public const string EmployeeEdit = "EmployeeEdit";
        public const string EmployeeAdd = "EmployeeAdd";
    }
}
