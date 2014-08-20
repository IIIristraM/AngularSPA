using Domain.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Implementation
{
    public class DefaultUserStore<TUser> : UserStore<TUser> where TUser: IdentityUser
    {
        public DefaultUserStore(DbContext context) : base(context) { }
    }
}
