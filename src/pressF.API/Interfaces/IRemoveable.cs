using System;

namespace pressF.API.Interfaces
{
    interface IRemoveable
    {
        public DateTimeOffset? ExcludedDate { get; set; }
        public bool Excluded { get; set; }
    }
}
