namespace Gvin.Injection
{
    /// <summary>
    /// Represents dependency injector.
    /// </summary>
    public interface IInjector
    {
        /// <summary>
        /// Creates object of specified type according to injector configuration.
        /// </summary>
        /// <typeparam name="T">Type of object to be created.</typeparam>
        /// <returns>Instance of specified type initialized with all required parameters.</returns>
        T Create<T>() where T : IInjectable;
    }
}