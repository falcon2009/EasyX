using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.ModelService;
using UAC.Share.Key;
using UAC.Share.Model;
using UAC.Storage.Entity;
using UAC.Storage.EntityModel;

namespace UAC.Storage.Service
{
    public class UserService : ModelServiceEFRepoGeneric<User, UserKey>
    {
        public UserService(IQueryableDataProvider<User, UserKey> dataProvider, IDataManager<User, UserKey> dataManager, IMapper mapper)
            : base(dataProvider, dataManager, mapper)
        { }

        protected override void ConfigureService()
        {
            BindModel<UserModel, User>();
            BindModel<UserWithRoleModel, User>();
            BindModel<UserBaseModel, UserBase>();
            BindModel<UserBriefModel, UserBrief>();
        }
    }
}
