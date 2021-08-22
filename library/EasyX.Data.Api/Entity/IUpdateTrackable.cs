using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Data.Api.Entity
{
    public interface IUpdateTrackable
    {
        DateTimeOffset? UpdatedOn { get; set; }
        string UpdatedById { get; set; }
        string UpdatedByName { get; set; }
    }
}
