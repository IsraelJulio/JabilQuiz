using DataAccess.Context;
using Interface.DataServices;
using JabilQuiz.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServices.DbContextServices
{
    public class GameService : IGameService
    {
        private readonly DatabaseContext _context;
        public GameService(DatabaseContext context)
        {
            _context = context;
        }

        private async Task ContextSaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<Game>().FindAsync(id);

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

        public async Task<List<Game>> GetAllAsync()
        {
            var query = _context.Set<Game>().AsQueryable();           
            var result = await query.AsNoTracking().ToListAsync();
            return result;
        }

        public async Task<List<Game>> GetByFilterAsync(

            Expression<Func<Game, bool>> filter,
            Func<Game, object> orderingFunction = null,
            bool orderingAsc = true,
            int skip = 0,
            int pageLength = -1)
        {
           

            IQueryable<Game> query = _context.Set<Game>()
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
                return Enumerable.Empty<Game>().ToList();
            }
        }

        private async Task<Game> PostAsync(Game entity)
        {           
            await _context.Set<Game>().AddAsync(entity);
            await ContextSaveAsync();
            return entity;
        }

        private async Task<Game> PutAsync(Game entity)
        {          
            _context.Entry(entity).State = EntityState.Modified;
            await ContextSaveAsync();
            return entity;
        }

        public async Task<Game> SaveAsync(Game entity)
        {
            if (entity.Id == 0)
                entity = await PostAsync(entity);
            else
                entity = await PutAsync(entity);

            return entity;
        }

        public async Task<List<Game>> SaveRangeAsync(List<Game> entityList)
        {
            _context.Set<Game>().UpdateRange(entityList);
            await ContextSaveAsync();
            return entityList;
        }
        public async Task DeleteRangeAsync(List<Game> entityList)
        {
            _context.Set<Game>().RemoveRange(entityList);

            await ContextSaveAsync();
        }
    }
}
