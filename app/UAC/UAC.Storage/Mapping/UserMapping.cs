using AutoMapper;
using EasyX.Data.Core.Tool;
using System.Linq;
using UAC.Share.Key;
using UAC.Share.Model;
using UAC.Storage.Entity;
using UAC.Storage.EntityModel;

namespace UAC.Storage.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserModel>();
            CreateMap<UserModel, User>()
                .ForMember(model => model.UserRoleList, option => option.Ignore())
                .AfterMap<MapUserFromUserModel>();

            CreateMap<User, UserWithRoleModel>()
                .ForMember(model => model.RoleModelList, option => option.Ignore())
                .AfterMap<MapUserWithRoleModelFromModel>();

            CreateMap<UserBase, UserBaseModel>();

            CreateMap<UserBrief, UserBriefModel>();
        }

        private class MapUserFromUserModel: IMappingAction<UserModel, User>
        {
            public void Process(UserModel source,  User destination, ResolutionContext context)
            {
                MappingHelper.JoinOneToMany<User, UserModel, UserRole, UserRoleModel, UserRoleKey>
                    (destination, source, entity => entity.UserRoleList, model => model.UserRoleList, context.Mapper, false, false);
            }
        }

        private class MapUserWithRoleModelFromModel : IMappingAction<User, UserWithRoleModel>
        {
            public void Process(User source, UserWithRoleModel destination, ResolutionContext context)
            {
                if (source.UserRoleList.Any())
                {
                    destination.RoleModelList = source.UserRoleList.Select(item => context.Mapper.Map<RoleModel>(item.Role)).ToList();
                }
            }
        }
    }
}
