using DataAccess.Context;
using Interface.DataServices;
using JabilQuiz.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.DbContextServices
{
    public class AlternativeService : IAlternativeService
    {
        private readonly DatabaseContext _context;
        public AlternativeService(DatabaseContext context)
        {
            _context = context;
        }

        private async Task ContextSaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Alternative>().FindAsync(id);

            _context.Entry(entity).State = EntityState.Deleted;
            await ContextSaveAsync();
        }
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public async Task<List<Alternative>> GetAllAsync()
        {
            var query = _context.Set<Alternative>().AsQueryable();           
            var result = await query.AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<List<Alternative>> GetByFilterAsync(

            Expression<Func<Alternative, bool>> filter,
            Func<Alternative, object> orderingFunction = null,
            bool orderingAsc = true,
            int skip = 0,
            int pageLength = -1)
        {
           

            IQueryable<Alternative> query = _context.Set<Alternative>()
                    .Where(filter)
                    .AsQueryable();

            if (orderingAsc)
                query = query.OrderBy(orderingFunction ?? (p => "")).AsQueryable();
            else
                query = query.OrderByDescending(orderingFunction ?? (p => "")).AsQueryable();

            query = query.Skip(skip);

            if (pageLength > -1)
                query = query.Take(pageLength);
           
            if (query.Any())
                return query.AsNoTracking().ToList();
            else
            {
                return Enumerable.Empty<Alternative>().ToList();
            }
        }

        private async Task<Alternative> PostAsync(Alternative entity)
        {           
            await _context.Set<Alternative>().AddAsync(entity);
            await ContextSaveAsync();
            return entity;
        }

        private async Task<Alternative> PutAsync(Alternative entity)
        {          
            _context.Entry(entity).State = EntityState.Modified;
            await ContextSaveAsync();
            return entity;
        }

        public async Task<Alternative> SaveAsync(Alternative entity)
        {
            if (entity.Id == 0)
                entity = await PostAsync(entity);
            else
                entity = await PutAsync(entity);

            return entity;
        }

        public async Task<List<Alternative>> SaveRangeAsync(List<Alternative> entityList)
        {
            _context.Set<Alternative>().UpdateRange(entityList);
            await ContextSaveAsync();
            return entityList;
        }
        public async Task DeleteRangeAsync(List<Alternative> entityList)
        {
            _context.Set<Alternative>().RemoveRange(entityList);

            await ContextSaveAsync();
        }
    }
}
