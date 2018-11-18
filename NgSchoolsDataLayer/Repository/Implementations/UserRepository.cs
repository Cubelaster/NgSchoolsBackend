﻿using NgSchoolsDataLayer.Context;
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
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.MiddleName = user.MiddleName;
            userToUpdate.Mobile = user.Mobile;
            userToUpdate.Mobile2 = user.Mobile2;
            userToUpdate.Phone = user.Phone;
            userToUpdate.Signature = user.Signature;
            userToUpdate.Title = user.Title;
            userToUpdate.Avatar = user.Avatar;
            userToUpdate.DateModified = DateTime.Now;

            return userToUpdate;
        }
    }
}
