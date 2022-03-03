using Interface.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DataAccess.Context
{
    public class UnityOfWork : IUnityOfWork
    {
        private readonly DatabaseContext _context;

        public UnityOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollBack()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}