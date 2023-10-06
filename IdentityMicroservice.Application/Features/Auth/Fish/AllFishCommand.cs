
using IdentityMicroservice.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityMicroservice.Application.Features.Auth.Fish
{
    public class AllFishCommand : IRequest<List<string>>
    {
        public Guid UserId { get; set; }
    }
    public class AllFishCommandHandler : IRequestHandler<AllFishCommand, List<string>>
    {
        private readonly IUserManager _userManager;
        private readonly IFishTankManager _fishTankManager;

        public AllFishCommandHandler(IFishTankManager fishTankManager, IUserManager userManager)
        {
            _fishTankManager = fishTankManager;
            _userManager = userManager;
        }

        public async Task<List<string>> Handle(AllFishCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserById(request.UserId);

            if (user != null)
            {
                var operationResult = await _fishTankManager.GetAllFish();
                return operationResult;
            }
            throw new Exception("bad attempt");
        }
    }
}
