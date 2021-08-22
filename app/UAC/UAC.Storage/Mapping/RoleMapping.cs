using AutoMapper;
using EasyX.Data.Api.Request;
using System.Collections.Generic;
using System.Linq;
using UAC.Share.Model;
using UAC.Storage.Entity;
using UAC.Storage.EntityModel;

namespace UAC.Storage.Mapping
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<Role, RoleModel>()
                .ForMember(model => model.UserModelList, option => option.Ignore())
                .AfterMap<MapRoleModelFromRole>();

            CreateMap<RoleModel, Role>()
                .ForMember(entity => entity.UserRoleList, option => option.Ignore());

            CreateMap<RoleLookup, RoleLookupModel>();
        }

        private class MapRoleModelFromRole : IMappingAction<Role, RoleModel>
        {
            public void Process(Role source, RoleModel destination, ResolutionContext context)
            {
                if (source?.UserRoleList?.Any() ?? false)
                {
                    destination.UserModelList = source.UserRoleList.Select(entity => context.Mapper.Map<UserModel>(entity.User)).ToList();
                }
                else 
                {
                    destination.UserModelList = new List<UserModel>();
                }
            }
        }
    }
}
