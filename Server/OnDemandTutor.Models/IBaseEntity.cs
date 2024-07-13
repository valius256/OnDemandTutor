using DateTime = System.DateTime;

namespace OnDemandTutor.Models;

public enum RecordStatus
{
    Active,
    Inactive,
    Deleted
}

public interface IBaseEntity
{
}

public abstract class BaseEntityEmpty : IBaseEntity
{
    public int Id { get; set; }
}

public abstract class BaseEntity : BaseEntityEmpty, IBaseEntity
{
    public int UpdatedById { get; set; } = 0;

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public RecordStatus RecordStatus { get; set; } = RecordStatus.Active;

    public void SoftDelete()
    {
        RecordStatus = RecordStatus.Deleted;
        DeletedDate = DateTime.UtcNow;
    }
}