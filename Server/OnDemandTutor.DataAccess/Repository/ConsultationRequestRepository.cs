using System;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
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

    }
}

