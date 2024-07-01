using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;

namespace OnDemandTutor.DataAccess;

public interface IUnitOfWorkRepository
{
    public IUserRepository UserRepository { get; }
    public ISubjectRepository SubjectRepository { get; }
    public ISlotRepository SlotRepository { get; }
    public IBlogRepository BlogRepository { get; }
    public IClassRepository ClassRepository { get; }
    public IConsultationRequestRepository ConsultationRequestRepository { get; }
    public ITutorDegreeRepository TutorDegreeRepository { get; }
    public IEmailTemplateRepository EmailTemplateRepository { get; }

    public ITransactionRepository TransactionRepository { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task MigrateAsync();
}

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
    private readonly ApplicationDbContext _context;

    public UnitOfWorkRepository(ApplicationDbContext context, IUserRepository userRepository,
        ISubjectRepository subjectRepository, ISlotRepository slotRepository, IBlogRepository blogRepository,
        IClassRepository classRepository, IConsultationRequestRepository consultationRequestRepository,
        IEmailTemplateRepository emailTemplateRepository, ITransactionRepository transactionRepository, ITutorDegreeRepository tutorDegreeRepository)
    {
        _context = context;
        UserRepository = userRepository;
        SubjectRepository = subjectRepository;
        SlotRepository = slotRepository;
        BlogRepository = blogRepository;
        ClassRepository = classRepository;
        ConsultationRequestRepository = consultationRequestRepository;
        EmailTemplateRepository = emailTemplateRepository;
        TransactionRepository = transactionRepository;
        TutorDegreeRepository = tutorDegreeRepository;
    }

    public IUserRepository Users { get; }


    public IUserRepository UserRepository { get; }

    public ISubjectRepository SubjectRepository { get; }

    public ISlotRepository SlotRepository { get; }

    public IBlogRepository BlogRepository { get; }

    public IClassRepository ClassRepository { get; }

    public IConsultationRequestRepository ConsultationRequestRepository { get; }
    public IEmailTemplateRepository EmailTemplateRepository { get; }

    public ITransactionRepository TransactionRepository { get; }

    public ITutorDegreeRepository TutorDegreeRepository { get; }

    public Task MigrateAsync()
    {
        return _context.Database.MigrateAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}