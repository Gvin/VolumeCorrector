using System;
using System.Collections.Generic;

namespace Gvin.Injection.Configuration
{
    /// <summary>
    /// Interface for injector configuration.
    /// Used to define injector configurations.
    /// </summary>
    public interface IInjectorConfiguration
    {
        /// <summary>
        /// Creates mapping for the injector.
        /// </summary>
        Dictionary<Type, InjectorMappingType> GetMapping();
    }
}