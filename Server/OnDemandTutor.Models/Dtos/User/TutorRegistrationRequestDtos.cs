namespace OnDemandTutor.Models.Dtos.User;

public class TutorRegistrationRequestDtos
{
    public string diplomaNumber;
    public string IdCardNumber;
    public DateOnly issuanceDate;
    public string? tutorAddress;
    public string tutorEmail;
    public int tutorId;
    public string tutorName;
    public string? tutorPhoneNumber;
}