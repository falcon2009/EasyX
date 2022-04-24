using AutoMapper;
using Organization.Share.Model;
using OrganizatonOrc.Shared.Model;

namespace OrganizatonOrc.WebApi.Mapping
{
    public class EmployeeOrcMapping : Profile
    {
        public EmployeeOrcMapping()
        {
            CreateMap<EmployeeModel, EmployeeOrcModel>()
                .ForMember(destination => destination.PersonContactList, option => option.Ignore());
        }
    }
}
