using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyX.Data.Api.Entity
{
    public interface IDeleteTrackable
    {
        DateTimeOffset? DeletedOn { get; set; }
        string DeletedById { get; set; }
        string DeletedByName { get; set; }
    }
}
