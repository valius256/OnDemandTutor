namespace OnDemandTutor.Models.Models;

public class TutorRegistration : IBaseEntity
{
    public string diplomaNumber;
    public string IdCardNumber;
    public DateOnly issuanceDate;
    public string? tutorAddress;
    public string tutorEmail;
    public string tutorName;
    public string? tutorPhoneNumber;
    public int userId;
    public int Id { get; set; }

    public virtual User Tutor { get; set; }
}