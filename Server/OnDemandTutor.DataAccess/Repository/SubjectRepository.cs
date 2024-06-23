using System;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository
{
	public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
		public SubjectRepository(ApplicationDbContext context) : base(context)
        {
		}
	}
}

