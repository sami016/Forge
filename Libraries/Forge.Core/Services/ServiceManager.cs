using Forge.Core.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace Forge.Core.Services
{
    public class ServiceManager
    {
        private readonly EntityManager _entityManager;
        private readonly ServiceContainer _serviceContainer;
        private IList<Service> _services = new List<Service>();

        public ServiceManager(EntityManager entityManager, ServiceContainer serviceContainer)
        {
            _entityManager = entityManager;
            _serviceContainer = serviceContainer;
        }

        public T Add<T>(T service)
            where T : Service
        {
            var serviceEnt = _entityManager.Create();
            serviceEnt.Add(service);
            serviceEnt.Create();
            _serviceContainer.AddService(typeof(T), service);
            _services.Add(service);
            return service;
        }

        public void Remove<T>(T service)
            where T : Service
        {
            if (_services.Contains(service))
            {
                _serviceContainer.RemoveService(typeof(T));
                var serviceEntity = service.Entity;
                service.Dispose();
                serviceEntity.Delete();
            }
        }

        public void Remove<T>()
            where T : Service
        {
            var type = typeof(T);
            foreach (var service in _services.ToArray())
            {
                if (service.GetType() == type)
                {
                    Remove<T>(service as T);
                }
            }
        }
    }
}
