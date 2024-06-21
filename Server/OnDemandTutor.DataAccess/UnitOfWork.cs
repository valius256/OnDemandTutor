using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;

namespace OnDemandTutor.DataAccess;

public interface IUnitOfWorkRepository
{
    public IUserRepository UserRepository { get; }
    public ISubjectRepository SubjectRepository { get; }
    int SaveChanges();
    Task<int> SaveChangesAsync();
    Task MigrateAsync();
}

public class UnitOfWorkRepository : IUnitOfWorkRepository
{
    private readonly ApplicationDbContext _context;

    public UnitOfWorkRepository(ApplicationDbContext context, IUserRepository userRepository,
        ISubjectRepository subjectRepository)
    {
        _context = context;
        UserRepository = userRepository;
        SubjectRepository = subjectRepository;
    }

    public IUserRepository Users { get; }


    public IUserRepository UserRepository { get; }

    public ISubjectRepository SubjectRepository { get; }


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