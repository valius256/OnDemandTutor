
namespace OnDemandTutor.Models.Dtos.User
{
    public class GetSimpleUserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Name
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
