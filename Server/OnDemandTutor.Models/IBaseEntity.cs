using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.Models
{
    public enum RecordStatus
    {
        Active,
        Inactive,
        Deleted,
    }

    public interface IBaseEntity { }

    public abstract class BaseEntityEmpty : IBaseEntity
    {
        public Guid Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntityEmpty, IBaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime DeletedDate { get; set; }

        public RecordStatus RecordStatus { get; set; }
    }
}
