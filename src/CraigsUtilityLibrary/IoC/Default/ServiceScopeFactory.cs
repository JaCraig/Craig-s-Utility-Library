using Microsoft.Framework.DependencyInjection;
using Utilities.IoC.Interfaces;

namespace Utilities.IoC.Default
{
    /// <summary>
    /// Used to create scopes.
    /// </summary>
    public class ServiceScopeFactory : IScopeFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceScopeFactory"/> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public ServiceScopeFactory(IScope scope)
        {
            Scope = scope;
        }

        /// <summary>
        /// The scope
        /// </summary>
        private readonly IScope Scope;

        /// <summary>
        /// Creates a new scope object.
        /// </summary>
        /// <returns>The service scope</returns>
        public IServiceScope CreateScope()
        {
            return Scope.CreateScope();
        }
    }
}