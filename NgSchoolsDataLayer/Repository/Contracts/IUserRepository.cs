using System;
using System.Collections.Generic;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsDataLayer.Repository.Contracts
{
    public interface IUserRepository
    {
        User GetUserById(Guid Id);
        User GetUserByName(string name);
        List<User> GetAllUsers();
    }
}