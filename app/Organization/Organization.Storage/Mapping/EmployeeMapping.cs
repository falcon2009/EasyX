using AutoMapper;
using Organization.Share.Model;
using Organization.Storage.Entity;

namespace Organization.Storage.Mapping
{
    public class EmployeeMapping : Profile
    {
        public EmployeeMapping()
        {
            CreateMap<Employee, EmployeeModel>();
            CreateMap<EmployeeModel, Employee>()
                .ForMember(entity => entity.Organization, option => option.Ignore());
        }
    }
}
