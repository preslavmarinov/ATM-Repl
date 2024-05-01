using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DI
{
    public class ServiceContainer
    {
        private static volatile ServiceContainer instance;
        private static readonly object instanceLock = new object();

        private readonly Dictionary<Type, Func<object>> transientServices = new Dictionary<Type, Func<object>>();
        private readonly Dictionary<Type, object> singletonServices = new Dictionary<Type, object>();

        private ServiceContainer()
        {
        }

        public static ServiceContainer GetInstance()
        {
            if(instance == null)
            {
                lock(instanceLock)
                {
                    if(instance == null) instance = new ServiceContainer();
                }
            }

            return instance;
        }

        public void AddTransient<TInterface, TService>()
            where TService : TInterface
        {
            transientServices[typeof(TInterface)] = () => CreateInstance<TService>();
        }

        public void AddTransient<TService>()
        {
            transientServices[typeof(TService)] = () => CreateInstance<TService>();
        }

        public void AddSingleton<TInterface, TService>()
           where TService : TInterface
        {
            TService instance = CreateInstance<TService>();
            singletonServices[typeof(TInterface)] = instance;
        }

        public void AddSingleton<TService>()
        {
            TService instance = CreateInstance<TService>();
            singletonServices[typeof(TService)] = instance;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        private object GetService(Type serviceType)
        {
            if (singletonServices.TryGetValue(serviceType, out var instance))
            {
                return instance;
            }

            if (transientServices.TryGetValue(serviceType, out var factory))
            {
                return factory();
            }

            throw new InvalidOperationException($"Service of type {serviceType} is not registered");
        }

        private T CreateInstance<T>()
        {
            Type serviceType = typeof(T);
            var constructors = serviceType.GetConstructors().FirstOrDefault();

            if (constructors == null)
            {
                throw new InvalidOperationException($"Type {serviceType.Name} does not have a any constructors.");
            }

            var parameters = constructors.GetParameters();
            var resolvedParameters = parameters.Select(p => GetService(p.ParameterType)).ToArray();

            return (T)Activator.CreateInstance(serviceType, resolvedParameters);
        }

    }
}
