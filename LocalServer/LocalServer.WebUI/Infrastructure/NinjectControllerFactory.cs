using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using LocalServer.Domain.Abstract;
using LocalServer.Domain.Concrete;

namespace LocalServer.WebUI.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;
        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext,
        Type controllerType)
        {
            return controllerType == null
            ? null
            : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings()
        {
            ninjectKernel.Bind<IProductRepository>().To<EFProductRepository>();
            ninjectKernel.Bind<IManufacturerRepository>().To<EFManufacturerRepository>();
            ninjectKernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
            ninjectKernel.Bind<IBatchRequestDetailRepository>().To<EFBatchRequestDetailRepository>();
            ninjectKernel.Bind<IBatchRequestRepository>().To<EFBatchRequestRepository>();
            ninjectKernel.Bind<ITransactionRepository>().To<EFTransactionRepository>();
            ninjectKernel.Bind<ITransactionDetailRepository>().To<EFTransactionDetailRepository>();
            ninjectKernel.Bind<ICashRegisterRepository>().To<EFCashRegisterRepository>();
            ninjectKernel.Bind<IPriceDisplayRepository>().To<EFPriceDisplayRepository>();
            ninjectKernel.Bind<IEmployeeRepository>().To<EFEmployeeRepository>();
            ninjectKernel.Bind<ISessionRepository>().To<EFSessionRepository>();
        }
    }
}