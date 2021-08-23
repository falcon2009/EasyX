using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Organization.Storage.Entity
{
    public record Organization : IKey<OrganizationKey>, ICreateTrackable
    {
            public Guid Id { get; set; }
            public string Titlte { get; set; }
            public string IdentificationNumber { get; set; }
            public DateTimeOffset? CreatedOn { get; set; }
            public string CreatedById { get; set; }
            public string CreatedByName { get; set; }
            public List<Employee> EmployeeList { get; set; }

            [JsonIgnore]
            public OrganizationKey Key => new() { Id = Id };
    }
}
