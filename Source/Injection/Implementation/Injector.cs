using System;
using System.Collections.Generic;
using System.Linq;
using Gvin.Injection.Configuration;

namespace Gvin.Injection.Implementation
{
    internal class Injector : IInjector, IDisposable
    {
        private readonly Dictionary<Type, InjectorMappingType> mapping;

        public Injector(IInjectorConfiguration initialMapping)
        {
            mapping = initialMapping.GetMapping().ToDictionary(pair => pair.Key, pair => pair.Value);
            mapping.Add(typeof(IInjector), new InjectorMappingType{Object = this});
        }

        public T Create<T>() where T : IInjectable
        {
            var type = typeof(T);
            return (T) CreateFromInterfaceType(type);
        }

        private object CreateFromInterfaceType(Type type)
        {
            if (!type.IsInterface)
                throw new ApplicationException($"Interface type expected but got: {type.FullName}");

            if (!mapping.ContainsKey(type))
                throw new ApplicationException($"Mapping for type \"{type}\" not found.");

            var typeMapping = mapping[type];
            if (typeMapping.Type == null && typeMapping.Object == null)
                throw new ApplicationException(
                    $"Invalid type mapping for type \"{type.FullName}\": both Type and Object configurations are null.");
            if (typeMapping.Type != null && typeMapping.Object != null)
                throw new ApplicationException(
                    $"Invalid type mapping for type \"{type.FullName}\": both Type and Object configurations are filled (should be only 1 of them).");

            if (typeMapping.Object == null)
            {
                return CreateFromImplType(typeMapping.Type);
            }

            return typeMapping.Object;
        }

        private object CreateFromImplType(Type type)
        {
            var constructors = type.GetConstructors();
            if (constructors.Length > 1)
                throw new ApplicationException($"Type {type.Name} contains several constructors.");

            var constructor = constructors.First();
            var parameters = constructor.GetParameters().Select(parameter => parameter.ParameterType).ToArray();
            if (parameters.Length == 0)
            {
                return constructor.Invoke(new object[0]);
            }

            if (parameters.Any(parameter => !parameter.IsInterface))
            {
                throw new ApplicationException($"Constructor parameters for type \"{type.FullName}\" contains non-interface parameter(s).");
            }

            var parametersImpl = parameters.Select(CreateFromInterfaceType).ToArray();
            return constructor.Invoke(parametersImpl);
        }

        public void Dispose()
        {
            var objectMapping = mapping.Values.Where(mappingType => mappingType.Object != null)
                .Select(mappingType => mappingType.Object).OfType<IDisposable>().ToArray();
            foreach (var disposableObject in objectMapping)
            {
                if (disposableObject is Injector) // Skip circular self-disposing.
                    continue;

                disposableObject.Dispose();
            }

            mapping.Clear();
        }
    }
}