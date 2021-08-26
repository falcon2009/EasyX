using UAC.Share.Model;
using System;
using System.Collections.Generic;

namespace Person.Share
{
    public static class UacShareModel
    {
        public static IEnumerable<Type> ShareModelList
        {
            get 
            {
                return new Type[] 
                { 
                    typeof(RoleLookupModel), 
                    typeof(RoleModel), 
                    typeof(UserBaseModel),
                    typeof(UserBriefModel),
                    typeof(UserModel),
                    typeof(UserWithRoleModel)
                };
            }
        }
    }
}
