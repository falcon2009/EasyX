using AutoMapper;
using Organization.Share.Model;
using Organization.Storage.EntityModel;

namespace Organization.Storage.Mapping
{
    public class OrganizationMapping : Profile
    {
        public OrganizationMapping()
        {
            CreateMap<Entity.Organization, OrganizationModel>();
            CreateMap<OrganizationModel, Entity.Organization>()
                .ForMember(entity => entity.EmployeeList, option => option.Ignore());

            CreateMap<OrganizationLookup, OrganizationLookupModel>();

            CreateMap<OrganizationWithEmployee, OrganizationWithEmployeeModel>();
        }
    }
}
