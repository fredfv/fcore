using System;

namespace pressF.API.Interfaces
{
    public interface ITraceable
    {
        public DateTimeOffset InsertDate { get; set; }
        public DateTimeOffset? UpdateDate { get; set; }
    }
}
