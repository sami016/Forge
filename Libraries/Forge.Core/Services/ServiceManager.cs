using Forge.Core.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Forge.Core.Services
{
    public class ServiceManager
    {
        private readonly EntityManager _entityManager;
        private IList<Service> _services = new List<Service>();

        public ServiceManager(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        public void Add<T>(T service)
            where T : Service
        {
            var serviceEnt = _entityManager.Create();
            serviceEnt.Add(service);
            serviceEnt.Create();
            _services.Add(service);
        }

        public void Remove<T>(T service)
            where T : Service
        {
            if (_services.Contains(service))
            {
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
