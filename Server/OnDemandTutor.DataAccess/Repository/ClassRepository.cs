using System;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Models;

namespace OnDemandTutor.DataAccess.Repository
{
	public class ClassRepository : GenericRepository<Class>, IClassRepository
    {
		public ClassRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}

