using AutoMapper;
using UAC.Share.Model;
using UAC.Storage.Entity;

namespace UAC.Storage.Mapping
{
    public class UserRoleMapping : Profile
    {
        public UserRoleMapping()
        {
            CreateMap<UserRole, UserRoleModel>();
            CreateMap<UserRoleModel, UserRole>();
        }
    }
}
