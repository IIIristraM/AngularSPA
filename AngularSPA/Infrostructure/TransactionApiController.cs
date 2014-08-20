using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace App.AudioSearcher
{
    public class TransactionApiController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;       
        private readonly IUserManagerFactory _userManagerFactory;
        private ITransaction _transaction;
        private UserManager<UserProfile> _userManager;

        public TransactionApiController(IUnitOfWork unitOfWork, IUserManagerFactory userManagerFactory)
        {
            if (!unitOfWork.Equals(userManagerFactory))
            {
                throw new InvalidCastException("Error in IoC configuration");
            }

            _unitOfWork = unitOfWork;
            _userManagerFactory = userManagerFactory;
        }

        protected IUnitOfWork UnitOfWork
        {
            get
            {
                return _unitOfWork;
            }
        }

        protected UserManager<UserProfile> UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = _userManagerFactory.GetUserManager<UserProfile>();
                }

                return _userManager;
            }
        }

        public async override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            using (_transaction = UnitOfWork.GetTransaction())
            {
                var isAnyExecutionErrors = false;
                try
                {                    
                    return await base.ExecuteAsync(controllerContext, cancellationToken);
                }
                catch
                {
                    _transaction.Rollback();
                    isAnyExecutionErrors = true;
                    throw;
                }
                finally
                {
                    if (!isAnyExecutionErrors)
                    {
                        _transaction.Commit();
                    }
                }               
            }
        }
    }
}