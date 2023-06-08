using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Domain.Entities;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface IFishTankManager
    {
        Task<AddOrUpdateTankResponseModel> CreateOrEditFishTank(AddOrUpdateTankRequestModel model, IdentityUser? user = null);
        Task<bool> RemoveFishTank(int tankId);
        Task<bool> AddOrRemoveFish(int tankId, string fishName, bool isDeleted);
        Task<GetTanksModel> GetFishTanks(int? tankId = null);
    }
}
