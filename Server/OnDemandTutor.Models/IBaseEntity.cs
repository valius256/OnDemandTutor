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
    public int UpdatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public DateTime? DeletedDate { get; set; }

    public RecordStatus RecordStatus { get; set; }
    
    public void SoftDelete()
    {
        RecordStatus = RecordStatus.Deleted;
        DeletedDate = DateTime.UtcNow;
    }
}