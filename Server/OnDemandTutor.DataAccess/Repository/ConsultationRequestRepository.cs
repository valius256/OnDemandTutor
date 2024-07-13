using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Dtos.ConsultationRequestDtos;
using OnDemandTutor.Models.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.Repository
{
    public class ConsultationRequestRepository : GenericRepository<ConsultationRequest>, IConsultationRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public ConsultationRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<ConsultationRequest>> ViewAllConsultationsRequestAsync(ConsultationRequestFilterDto request)
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

            if (request.ConsultationStatus.HasValue)
            {
                consultListQuery = consultListQuery.Where(cs => cs.Status == request.ConsultationStatus.Value);
            }



            consultListQuery = consultListQuery.OrderBy(cr => cr.CreatedDate);
            var consultListQuery1 = await consultListQuery.OrderBy(cr => cr.CreatedDate).ToListAsync();
            int limit = request.Limit > 0 ? request.Limit : 10;
            int page = request.Page > 0 ? request.Page : 1;

            var consultationRequests = await consultListQuery
                .ToNewPagingAsync(page, limit);

            return consultationRequests;
        }



    }
}

