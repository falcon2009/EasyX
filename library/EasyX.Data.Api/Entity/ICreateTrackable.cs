using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Data.Api.Entity
{
#nullable enable
    public interface ICreateTrackable
    {
        public DateTimeOffset? CreatedOn { get; set; }
        public string CreatedById { get; set; }
        public string CreatedByName { get; set; }
    }
#nullable disable
}
