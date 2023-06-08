using System;

namespace IdentityMicroservice.Application.ViewModels.AppInternal
{
    public class AddOrUpdateTankRequestModel
    {
        public Guid UserId { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public int? TankId { get; set; } = null;
    }
}
