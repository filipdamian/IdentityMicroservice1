using System;


namespace IdentityMicroservice.Application.Common.Exceptions
{
    public class UserAlreadyRegisteredException: Exception
    {

        public string EntityName { get; }
        public UserAlreadyRegisteredException(string entityName, string errMessage) : base(errMessage)
        {
            EntityName = entityName;
        }
    }
}
