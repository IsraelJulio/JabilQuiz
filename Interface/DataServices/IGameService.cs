using JabilQuiz.Model;
using Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DataServices
{
    public interface IGameService
    {
        Task<List<Game>> GetAllAsync();

        Task<List<Game>> GetByFilterAsync(Expression<Func<Game, bool>> filter,            
            Func<Game, object> orderingFunction = null,
            bool orderingAsc = true,
            int skip = 0,
            int pageLength = -1);

        Task<Game> SaveAsync(Game entity);

        Task<List<Game>> SaveRangeAsync(List<Game> entityList);

        Task DeleteAsync(int id);
        Task DeleteRangeAsync(List<Game> entityList);
 

    }
}
