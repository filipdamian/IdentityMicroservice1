using IdentityMicroservice.Application.Common.Interfaces;
using System;
using System.Security.Cryptography;

namespace IdentityMicroservice.Application.Features.PasswordHashing
{
    public class HashAlgo : IHashAlgo
    {

        private const int SaltSize = 16; //128 bits
        private const int KeySize = 32; //256 bits
        private const int Iterations = 10000;



        public string CalculateHashValueWithInput(string input)
        {

            using (var algorithm = new Rfc2898DeriveBytes(input, SaltSize, Iterations, HashAlgorithmName.SHA512))
            {
                var hash = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);



                return $"{Iterations}.{salt}.{hash}";
            }
        }
        public bool IsPasswordVerified(string initialHash, string usedSalt, string input)
        {
            string reHashedPassword = "";

            // var utf8 = new Rfc2898DeriveBytes();
            byte[] saltByteArr = Convert.FromBase64String(usedSalt);
            using (var algorithm = new Rfc2898DeriveBytes(input, saltByteArr, Iterations, HashAlgorithmName.SHA512))
            {
                var hash = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                reHashedPassword = $"{Iterations}.{salt}.{hash}";
            }

            if (initialHash == reHashedPassword)
                return true;
            return false;
        }

    }
}
