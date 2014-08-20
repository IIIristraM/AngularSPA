using Domain.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class TestService : ITestService
    {
        IUnitOfWork _unitOfWork;

        public TestService(IUnitOfWork unitOfWork, IUserManagerFactory userManagerFactory)
        {
            if (!unitOfWork.Equals(userManagerFactory))
            {
                throw new InvalidCastException("Error in IoC configuration");
            }

            _unitOfWork = unitOfWork;
        }

        public void DoSomthing()
        {
            //throw new Exception();
        }
    }
}
