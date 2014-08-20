using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUserManagerFactory
    {
        UserManager<TUser> GetUserManager<TUser>() where TUser : class, IUser<string>;
    }
}
