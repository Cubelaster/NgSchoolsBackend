using System;
using System.Collections.Generic;
using NgSchoolsDataLayer.Models;

namespace NgSchoolsDataLayer.Repository.Contracts
{
    public interface IUserRepository
    {
        User GetUserById(Guid Id);
        User GetUserByEmail(string email);
        List<User> GetAllUsers();
        User Update(User user);
        UserDetails UpdateUserDetails(UserDetails userDetails);
    }
}