using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Tank
{
    public class AddOrUpdateTankCommand : IRequest<AddOrUpdateTankResponseModel>
    {
        public AddOrUpdateTankRequestModel Model { get; set; }
    }

    public class AddOrUpdateTankCommandHandler : IRequestHandler<AddOrUpdateTankCommand, AddOrUpdateTankResponseModel>
    {
        private readonly IUserManager _userManager;
        private readonly IFishTankManager _fishTankManager;

        public AddOrUpdateTankCommandHandler(IUserManager userManager, IFishTankManager fishTankManager)
        {
            _userManager = userManager;
            _fishTankManager = fishTankManager;
        }

        public async Task<AddOrUpdateTankResponseModel> Handle(AddOrUpdateTankCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserById(request.Model.UserId);

            if (user != null)
            {
                if (request.Model.TankId == null)
                {
                    var fishTankCreationResponse = await _fishTankManager.CreateOrEditFishTank(request.Model, user);
                    return fishTankCreationResponse;
                }
                else
                {
                    var fishTankEditResponse = await _fishTankManager.CreateOrEditFishTank(request.Model);

                    if (fishTankEditResponse == null)
                        throw new Exception("Bad attempt");
                    return fishTankEditResponse;
                }
            }

            throw new Exception("Bad attempt");
        }
    }
}
