using IdentityMicroservice.Application.Common.Interfaces;
using IdentityMicroservice.Application.ViewModels.AppInternal;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Tank
{
    public class GetTanksCommand : IRequest<GetTanksModel>
    {
        public int? TankId { get; set; } = null;
        public Guid UserId { get; set; }
    }

    public class GetTanksCommandHandler : IRequestHandler<GetTanksCommand, GetTanksModel>
    {
        private readonly IUserManager _userManager;
        private readonly IFishTankManager _fishTankManager;

        public GetTanksCommandHandler(IFishTankManager fishTankManager, IUserManager userManager)
        {
            _fishTankManager = fishTankManager;
            _userManager = userManager;
        }

        public async Task<GetTanksModel> Handle(GetTanksCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserById(request.UserId);

            if (user != null)
            {
                var response = await _fishTankManager.GetFishTanks(request.UserId,request.TankId);
                return response;
            }
            throw new Exception("BadAttempt");
        }
    }
}