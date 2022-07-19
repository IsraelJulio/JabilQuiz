using JabilQuiz.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Interface.DataServices
{
    public interface IAlternativeService
    {
        Task<List<Alternative>> GetAllAsync();

        Task<List<Alternative>> GetByFilterAsync(Expression<Func<Alternative, bool>> filter,            
            Func<Alternative, object> orderingFunction = null,
            bool orderingAsc = true,
            int skip = 0,
            int pageLength = -1);

        Task<Alternative> SaveAsync(Alternative entity);

        Task<List<Alternative>> SaveRangeAsync(List<Alternative> entityList);

        Task DeleteAsync(int id);
        Task DeleteRangeAsync(List<Alternative> entityList);
 

    }
}
