using Organization.Share.Model;
using System;
using System.Collections.Generic;

namespace Organization.Share
{
    public static class OrganizationShareModelList
    {
        public static IEnumerable<Type> ShareModelList
        {
            get 
            {
                return new Type[] 
                { 
                    typeof(EmployeeModel), 
                    typeof(OrganizationModel), 
                    typeof(OrganizationLookupModel), 
                    typeof(OrganizationWithEmployeeModel), 
                    typeof(PositionModel)
                };
            }
        }
    }
}
