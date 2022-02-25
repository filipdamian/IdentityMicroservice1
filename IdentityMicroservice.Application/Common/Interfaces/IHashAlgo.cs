
namespace IdentityMicroservice.Application.Common.Interfaces
{
    public interface IHashAlgo
    {
        string CalculateHashValueWithInput(string input);
        bool IsPasswordVerified(string input, string usedSalt, string initialHash);
    }
}
