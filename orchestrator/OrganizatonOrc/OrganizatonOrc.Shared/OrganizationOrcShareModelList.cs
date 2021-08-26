using OrganizatonOrc.Shared.Model;
using System;
using System.Collections.Generic;

namespace OrganizationOrc.Share
{
    public static class OrganizationOrcShareModelList
    {
        public static IEnumerable<Type> ShareModelList
        {
            get 
            {
                return new Type[] 
                { 
                    typeof(EmployeeOrcModel), 
                    typeof(OrganizatonWithEmployeeOrcModel)
                };
            }
        }
    }
}
