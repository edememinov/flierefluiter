﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Web.Mvc;
using Flierefluiter.Domain.Concrete;
using Flierefluiter.Domain.Abstract;
using Flierefluiter.Domain.Entities;


namespace Flierefluiter.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver {        
        
        private IKernel kernel; 
        
        public NinjectDependencyResolver(IKernel kernelParam) {            
            kernel = kernelParam;            
            AddBindings();        } 
        public object GetService(Type serviceType) 
       
        {            
            return kernel.TryGet(serviceType);        
        } 
       
        public IEnumerable<object> GetServices(Type serviceType) {            
            return kernel.GetAll(serviceType);        } 
        private void AddBindings() {

            kernel.Bind<IFlierefluiterRepository>().To<EFFlierefluiterRepository>();

    
        }    
    }
}