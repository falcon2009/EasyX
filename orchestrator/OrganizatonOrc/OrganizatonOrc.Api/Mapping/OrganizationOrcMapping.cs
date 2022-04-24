using AutoMapper;
using Organization.Share.Model;
using OrganizatonOrc.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizatonOrc.WebApi.Mapping
{
    public class OrganizationOrcMapping : Profile
    {
        public OrganizationOrcMapping()
        {
            CreateMap<OrganizationWithEmployeeModel, OrganizationWithEmployeeOrcModel>();
        }
    }
}
