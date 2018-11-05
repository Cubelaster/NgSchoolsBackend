using System;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsDataLayer.Repository.Contracts
{
    public interface IUserRepository
    {
        User GetUserById(Guid Id);
        User GetUserByName(string name);
    }
}