using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnDemandTutor.DataAccess
{
    public interface IUnitOfWorkRepository
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        Task MigrateAsync();
    }

    internal class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWorkRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
