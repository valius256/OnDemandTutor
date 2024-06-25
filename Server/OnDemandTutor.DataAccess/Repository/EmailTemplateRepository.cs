using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository;

public class EmailTemplateRepository : GenericRepository<EmailTemplate>, IEmailTemplateRepository
{
    public EmailTemplateRepository(ApplicationDbContext context) : base(context)
    {
    }
}