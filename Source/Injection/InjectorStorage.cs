﻿using System;
using Gvin.Injection.Configuration;
using Gvin.Injection.Implementation;

namespace Gvin.Injection
{
    /// <summary>
    /// Responsible for initial injector initialization and clearing.
    /// </summary>
    public static class InjectorStorage
    {
        private static readonly object InjectorLock = new object();

        private static Injector current;

        /// <summary>
        /// Initializes injector with specified configuration.
        /// Injector can be initialized only once. Use <c>Clear</c> to reset initialization status.
        /// </summary>
        /// <param name="configuration">Configuration that will be passed to the injector.</param>
        public static void Initialize(IInjectorConfiguration configuration)
        {
            lock (InjectorLock)
            {
                if (current != null)
                    throw new InvalidOperationException("Injector already initialized.");

                current = new Injector(configuration);
            }
        }

        /// <summary>
        /// Gets current configured injector.
        /// </summary>
        public static IInjector Current => current;

        /// <summary>
        /// Clears initialized injector and resets storage state.
        /// </summary>
        public static void Clear()
        {
            if (current == null)
                throw new InvalidOperationException("Injector was not initialized.");

            lock (InjectorLock)
            {
                current.Dispose();
                current = null;
            }
        }
    }
}