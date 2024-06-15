using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;

namespace OnDemandTutor.DataAccess
{
    public interface IUnitOfWorkRepository
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task MigrateAsync();
        public IUserRepository UserRepository { get; }
        public ISubjectRepository SubjectRepository { get; }
    }

    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository Users { get; private set; }

        private readonly IUserRepository _userRepository;
        private readonly ISubjectRepository _subjectRepository;


        public IUserRepository UserRepository => _userRepository;
        public ISubjectRepository SubjectRepository => _subjectRepository;

        public UnitOfWorkRepository(ApplicationDbContext context, IUserRepository userRepository, ISubjectRepository subjectRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _subjectRepository = subjectRepository;

        }




        public Task MigrateAsync()
        {
            return _context.Database.MigrateAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }


    }
}
