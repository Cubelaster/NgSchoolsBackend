using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Models;
using NgSchoolsDataLayer.Repository.Contracts;
using System;
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

        public User GetUserByName(string name)
        {
            return context.Users.FirstOrDefault(u => u.UserName == name);
        }
    }
}
