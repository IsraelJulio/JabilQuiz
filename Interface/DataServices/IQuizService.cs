using JabilQuiz.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DataServices
{
    public interface IQuizService
    {
        Task<List<Quiz>> GetAllAsync();

        Task<List<Quiz>> GetByFilterAsync(Expression<Func<Quiz, bool>> filter,            
            Func<Quiz, object> orderingFunction = null,
            bool orderingAsc = true,
            int skip = 0,
            int pageLength = -1);

        Task<Quiz> SaveAsync(Quiz entity);

        Task<List<Quiz>> SaveRangeAsync(List<Quiz> entityList);

        Task DeleteAsync(int id);
        Task DeleteRangeAsync(List<Quiz> entityList);
 

    }
}
