namespace OnDemandTutor.Models.Models;

public class TutorRegistration : IBaseEntity
{
    public int Id { get; set; }
    public int userId;
    public string tutorName;
    public string tutorEmail;
    public string? tutorPhoneNumber;
    public string? tutorAddress;
    public string IdCardNumber;
    public string diplomaNumber;
    public DateOnly issuanceDate; 
    
    public virtual User Tutor { get; set; }
}