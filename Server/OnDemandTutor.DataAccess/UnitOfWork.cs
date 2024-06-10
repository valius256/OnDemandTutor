using Microsoft.EntityFrameworkCore;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.DataAccess.Repository;
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

    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository Users {get; private set; }



        public UnitOfWorkRepository(ApplicationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);

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
