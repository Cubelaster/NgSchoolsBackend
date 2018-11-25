using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Enums;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NgSchoolsDataLayer.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        #region Ctors and Members

        private readonly NgSchoolsContext context;

        public UserRepository(NgSchoolsContext context)
        {
            this.context = context;
        }

        #endregion Ctors and Members

        public User GetUserById(Guid Id)
        {
            return context.Users.FirstOrDefault(u => u.Id == Id);
        }

        public User GetUserByEmail(string email)
        {
            return context.Users.FirstOrDefault(u => u.Email == email);
        }

        public List<User> GetAllUsers()
        {
            return context.Users.Where(u => u.Status == DatabaseEntityStatusEnum.Active).ToList();
        }

        public User Update(User user)
        {
            var userToUpdate = context.Users.Where(u => u.Id == user.Id).FirstOrDefault();
            userToUpdate.DateModified = DateTime.Now;

            return userToUpdate;
        }

        public UserDetails UpdateUserDetails(UserDetails userDetails)
        {
            var userDetailsToUpdate = context.UserDetails.Where(u => u.Id == userDetails.Id).FirstOrDefault();
            userDetailsToUpdate.FirstName = userDetails.FirstName;
            userDetailsToUpdate.LastName = userDetails.LastName;
            userDetailsToUpdate.MiddleName = userDetails.MiddleName;
            userDetailsToUpdate.Mobile = userDetails.Mobile;
            userDetailsToUpdate.Mobile2 = userDetails.Mobile2;
            userDetailsToUpdate.Phone = userDetails.Phone;
            userDetailsToUpdate.Signature = userDetails.Signature;
            userDetailsToUpdate.Title = userDetails.Title;
            userDetailsToUpdate.Avatar = userDetails.Avatar;
            userDetailsToUpdate.DateModified = DateTime.Now;

            return userDetailsToUpdate;
        }
    }
}
