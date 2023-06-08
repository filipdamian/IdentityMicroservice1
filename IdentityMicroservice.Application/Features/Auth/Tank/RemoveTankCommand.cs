using IdentityMicroservice.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Tank
{
    public class RemoveTankCommand : IRequest<bool>
    {
        public int TankId { get; set; }
        public Guid UserId { get; set; }
    }

    public class RemoveTankCommandHandler : IRequestHandler<RemoveTankCommand, bool>
    {
        private readonly IUserManager _userManager;
        private readonly IFishTankManager _fishTankManager;

        public RemoveTankCommandHandler(IFishTankManager fishTankManager, IUserManager userManager)
        {
            _fishTankManager = fishTankManager;
            _userManager = userManager;
        }

        public async Task<bool> Handle(RemoveTankCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserById(request.UserId);

            if (user != null)
            {
                var removeTankResponse = await _fishTankManager.RemoveFishTank(request.TankId);
                return removeTankResponse;
            }
            throw new Exception("Bad attempt");
        }
    }
}