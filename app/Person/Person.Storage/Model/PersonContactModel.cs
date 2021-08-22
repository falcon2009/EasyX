using Person.Share.Enum;
using System;

namespace Person.Storage.Model
{
    public class PersonContactModel
    {
        public Guid? Id { get; set; }
        public Guid? PersonId { get; set; }
        public ContactInfoType Type { get; set; }
        public string Value { get; set; }
    }
}
