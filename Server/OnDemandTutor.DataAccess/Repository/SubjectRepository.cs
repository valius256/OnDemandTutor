using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Subject;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.RequestModel.Subject;

namespace OnDemandTutor.DataAccess.Repository;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
    private readonly ApplicationDbContext _context;

    public SubjectRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> CheckSubjectExists(string subjectName)
    {
        return await _context.Subjects.AnyAsync(s => s.Name == subjectName);
    }

    public async Task<GetSubjectDtos> GetSubjectByCode(int code)
    {
        var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == code);
        return subject == null ? null : MapToGetSubjectDtos(subject);
    }

    public async Task<GetSubjectDtos> GetSubjectByName(string name)
    {
        var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name == name);
        return subject == null ? null : MapToGetSubjectDtos(subject);
    }

    public Task<IEnumerable<GetSubjectDtos>> GetSubjectsByCategory(string category)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsSubjectActive(int subjectId)
    {
        var subject = await _context.Subjects.FindAsync(subjectId);
        return subject != null && subject.Status;
    }

    public async Task<IEnumerable<GetSubjectDtos>> SearchSubjectsByName(string name)
    {
        var subjects = await _context.Subjects.Where(s => s.Name.Contains(name)).ToListAsync();
        return subjects.Select(MapToGetSubjectDtos).ToList();
    }

    public async Task UpdateSubjectDescription(SubjectRequestModel request)
    {
        var subject = await _context.Subjects.FindAsync(request.Id);
        if (subject != null)
        {
            subject.Description = request.Description;
            _context.Subjects.Update(subject);
        }
    }

    private GetSubjectDtos MapToGetSubjectDtos(Subject subject)
    {
        return new GetSubjectDtos
        {
            Id = subject.Id,
            Name = subject.Name,
            SubjectType = subject.SubjectType,
            CreateBy = subject.CreateById,
            Description = subject.Description,
            CreateAt = subject.CreateAt,
            Status = subject.Status
            //Classes = subject.Classes
        };
    }
}