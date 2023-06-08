using IdentityMicroservice.Application.ViewModels.AppInternal;
using IdentityMicroservice.Domain.Entities;

namespace IdentityMicroservice.Infrastructure.Services.Mappers
{
    public static class FishTankMapper
    {
        public static AddOrUpdateTankResponseModel ToModelMapper(this FishTank fishTank)
        {
            var model = new AddOrUpdateTankResponseModel 
            {
                Height = fishTank.Height,
                Length = fishTank.Lenght,
                Width = fishTank.Width,
                Volume = fishTank.Height * fishTank.Width * fishTank.Lenght /100,
                TankId = fishTank.Id,
            };

            return model;
        }
    }
}
