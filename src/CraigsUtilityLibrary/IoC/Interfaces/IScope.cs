using Microsoft.Framework.DependencyInjection;

namespace Utilities.IoC.Interfaces
{
    /// <summary>
    /// The service scope interface
    /// </summary>
    public interface IScope : IServiceScope
    {
        /// <summary>
        /// Creates the service scope.
        /// </summary>
        /// <returns>The service scope</returns>
        IServiceScope CreateScope();
    }
}