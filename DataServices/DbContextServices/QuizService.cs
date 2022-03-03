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
    public class QuizService : IQuizService
    {
        private readonly DatabaseContext _context;
        public QuizService(DatabaseContext context)
        {
            _context = context;
        }

        private async Task ContextSaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Quiz>().FindAsync(id);

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

        public async Task<List<Quiz>> GetAllAsync()
        {
            var query = _context.Set<Quiz>().AsQueryable();           
            var result = await query.AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<List<Quiz>> GetByFilterAsync(

            Expression<Func<Quiz, bool>> filter,
            Func<Quiz, object> orderingFunction = null,
            bool orderingAsc = true,
            int skip = 0,
            int pageLength = -1)
        {
           

            IQueryable<Quiz> query = _context.Set<Quiz>()
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
                return Enumerable.Empty<Quiz>().ToList();
            }
        }

        private async Task<Quiz> PostAsync(Quiz entity)
        {           
            await _context.Set<Quiz>().AddAsync(entity);
            await ContextSaveAsync();
            return entity;
        }

        private async Task<Quiz> PutAsync(Quiz entity)
        {          
            _context.Entry(entity).State = EntityState.Modified;
            await ContextSaveAsync();
            return entity;
        }

        public async Task<Quiz> SaveAsync(Quiz entity)
        {
            if (entity.Id == 0)
                entity = await PostAsync(entity);
            else
                entity = await PutAsync(entity);

            return entity;
        }

        public async Task<List<Quiz>> SaveRangeAsync(List<Quiz> entityList)
        {
            _context.Set<Quiz>().UpdateRange(entityList);
            await ContextSaveAsync();
            return entityList;
        }
        public async Task DeleteRangeAsync(List<Quiz> entityList)
        {
            _context.Set<Quiz>().RemoveRange(entityList);

            await ContextSaveAsync();
        }
    }
}
