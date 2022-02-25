using System;

namespace IdentityMicroservice.Application.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public string EntityName { get; }

        public NotFoundException(string entityName, string errMessage) : base(errMessage)
        {
            EntityName = entityName;
        }
    }
}
