using IdentityMicroservice.Application.Common.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Fish
{
    public class AddOrRemoveFishCommand : IRequest<bool>
    {
        public int TankId { get; set; }
        public Guid UserId { get; set; }
        public string FishName { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AddOrRemoveFishCommandHandler : IRequestHandler<AddOrRemoveFishCommand, bool>
    {
        private readonly IUserManager _userManager;
        private readonly IFishTankManager _fishTankManager;

        public AddOrRemoveFishCommandHandler(IFishTankManager fishTankManager, IUserManager userManager)
        {
            _fishTankManager = fishTankManager;
            _userManager = userManager;
        }

        public async Task<bool> Handle(AddOrRemoveFishCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserById(request.UserId);

            if (user != null)
            {
                var operationResult = await _fishTankManager.AddOrRemoveFish(request.TankId, request.FishName, request.IsDeleted);
                return operationResult;
            }
            throw new Exception("bad attempt");
        }
    }
}
