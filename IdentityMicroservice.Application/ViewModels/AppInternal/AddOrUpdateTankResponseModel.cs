namespace IdentityMicroservice.Application.ViewModels.AppInternal
{
    public class AddOrUpdateTankResponseModel
    {
        public int TankId { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Volume { get; set; }
    }
}
