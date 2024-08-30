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
    public IFAQRepository FAQRepository { get; }
    public ITutorDegreeRepository TutorDegreeRepository { get; }
    public IEmailTemplateRepository EmailTemplateRepository { get; }
    public ISlotStudentRepository SlotStudentRepository { get; }
    public ITransactionRepository TransactionRepository { get; }
    public IRequestWithDrawRepository RequestWithDrawRepository { get; }
    public INotificationRepository NotificationRepository { get; }
    public IStudentClassRepository StudentClassRepository { get; }
    public ITutorSubjectRepository TutorSubjectRepository { get; }

    public ITutorVideoRepository TutorVideoRepository { get; }

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
        IEmailTemplateRepository emailTemplateRepository, ITransactionRepository transactionRepository, ITutorDegreeRepository tutorDegreeRepository,
            ISlotStudentRepository slotStudentRepository, IFAQRepository fAQRepository, IRequestWithDrawRepository requestWithDrawRepository,
            INotificationRepository notificationRepository, IStudentClassRepository studentClassRepository, ITutorSubjectRepository tutorSubjectRepository,
            ITutorVideoRepository tutorVideoRepository
        )
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
        SlotStudentRepository = slotStudentRepository;
        FAQRepository = fAQRepository;
        RequestWithDrawRepository = requestWithDrawRepository;
        NotificationRepository = notificationRepository;
        StudentClassRepository = studentClassRepository;
        TutorSubjectRepository = tutorSubjectRepository;
        TutorVideoRepository = tutorVideoRepository;

    }

    public IUserRepository UserRepository { get; }

    public ISubjectRepository SubjectRepository { get; }

    public ISlotRepository SlotRepository { get; }

    public IBlogRepository BlogRepository { get; }

    public IClassRepository ClassRepository { get; }

    public IConsultationRequestRepository ConsultationRequestRepository { get; }

    public IFAQRepository FAQRepository { get; }

    public IEmailTemplateRepository EmailTemplateRepository { get; }

    public ISlotStudentRepository SlotStudentRepository { get; }

    public ITransactionRepository TransactionRepository { get; }

    public IRequestWithDrawRepository RequestWithDrawRepository { get; }

    public ITutorDegreeRepository TutorDegreeRepository { get; }

    public INotificationRepository NotificationRepository { get; }

    public IStudentClassRepository StudentClassRepository { get; }

    public ITutorSubjectRepository TutorSubjectRepository { get; }

    public ITutorVideoRepository TutorVideoRepository { get; }
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