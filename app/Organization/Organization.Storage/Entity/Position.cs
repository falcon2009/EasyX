using EasyX.Data.Api.Entity;
using Organization.Share.Key;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Organization.Storage.Entity
{
    public record Position : IKey<PositionKey>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Employee> EmployeeList { get; set; }
        [JsonIgnore]
        public PositionKey Key => new() { Id = Id };
    }
}
