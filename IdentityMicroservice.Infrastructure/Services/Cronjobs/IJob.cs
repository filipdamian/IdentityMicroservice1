using System.Threading.Tasks;

namespace IdentityMicroservice.Infrastructure.Services.Cronjobs
{
    public interface IJob
    {
        public Task Execute();

    }
}
