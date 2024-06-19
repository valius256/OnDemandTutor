namespace OnDemandTutor.Models.Dtos.User;

public class TutorRegistrationRequestDtos
{
    public int tutorId;
    public string tutorName;
    public string tutorEmail;
    public string? tutorPhoneNumber;
    public string? tutorAddress;
    public string IdCardNumber;
    public string diplomaNumber;
    public DateOnly issuanceDate; 
}