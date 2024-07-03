using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.Blog;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository
{
    public class ConsultationRequestRepository : GenericRepository<ConsultationRequest>, IConsultationRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultationRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<GetConsultationRequestDto>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request)
        {
            var consultListQuery = dbSet
                .Include(cs => cs.HandleBy)
                .AsQueryable();

            // Apply filters based on request
            if (!string.IsNullOrEmpty(request.Name))
            {
                consultListQuery = consultListQuery.Where(cs => cs.Name.Contains(request.Name));
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                consultListQuery = consultListQuery.Where(cs => cs.Phone == request.Phone);
            }

            if (!string.IsNullOrEmpty(request.ConsultationContent))
            {
                consultListQuery = consultListQuery.Where(cs => cs.ConsultationContent.Contains(request.ConsultationContent));
            }

            if (request.RequestDateFrom.HasValue)
            {
                consultListQuery = consultListQuery.Where(cs => cs.RequestDate >= request.RequestDateFrom.Value);
            }

            if (request.RequestDateTo.HasValue)
            {
                consultListQuery = consultListQuery.Where(cs => cs.RequestDate <= request.RequestDateTo.Value);
            }

            var consultationRequests = await consultListQuery
                .AsNoTracking()
                .Select(cs => new GetConsultationRequestDto
                {
                    Id = cs.Id,
                    Name = cs.Name,
                    Phone = cs.Phone,
                    ConsultationContent = cs.ConsultationContent,
                    CreatedDate = cs.RequestDate.ToDateTime(TimeOnly.MinValue) // Assuming RequestDate is of type DateOnly
                })
                .ToListAsync();

            return consultationRequests;
        }



    }
}

