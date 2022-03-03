using System.Threading.Tasks;

namespace Interface.DataServices
{
    public interface IUnityOfWork
    {
        Task SaveAsync();

        Task BeginTransaction();

        Task Commit();

        Task RollBack();
    }
}