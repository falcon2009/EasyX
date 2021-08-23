using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Organization.Share.Model
{
    public record OrganizationWithEmployeeModel : IKey<OrganizationKey>
    {
        public Guid Id { get; set; }
        public string Titlte { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
        public List<EmployeeModel> EmployeeList { get; set; }

        [JsonIgnore]
        public OrganizationKey Key => new() { Id = Id };
    }
}
