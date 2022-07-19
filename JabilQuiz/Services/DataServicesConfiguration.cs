using DataAccess.Context;
using DataServices.DbContextServices;
using Interface.DataServices;
using Microsoft.Extensions.DependencyInjection;

namespace JabilQuiz.Services
{
    public static class DataServicesConfiguration
    {
        public static void AddDataServices(this IServiceCollection services)
        {
            services.AddTransient<IQuizService, QuizService>();
            services.AddTransient<IAlternativeService, AlternativeService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IUnityOfWork, UnityOfWork>();            
        }
    }
}