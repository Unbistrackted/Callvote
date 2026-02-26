using System.Collections.Generic;
using Callvote.API.Enums;
using Callvote.API.Interfaces;

namespace Callvote.API.Features.Generic
{
    /// <summary>
    /// The type that manages the <see cref="IProvider"/> instances.
    /// </summary>
    /// <typeparam name="T">As <see cref="IProvider"/>.</typeparam>
    public abstract class BaseProviderHandler<T>
        where T : IProvider
    {
        /// <summary>
        /// Gets the Dictionary of registered Providers.
        /// </summary>
        public abstract Dictionary<string, T> Providers { get; }

        /// <summary>
        /// Gets the current <see cref="IProvider"/>.
        /// </summary>
        public virtual T CurrentProvider { get; internal set; }

        /// <summary>
        /// Gets this Provider Handler type.
        /// </summary>
        public abstract ProviderType ProviderHandlerType { get; }

        /// <summary>
        /// Register an <see cref="IProvider"/>.
        /// </summary>
        /// <param name="provider">The <see cref="IProvider"/> to be registered.</param>
        public virtual void RegisterProvider(T provider)
        {
            if (provider.Type != this.ProviderHandlerType || this.Providers.ContainsKey(provider.Name))
            {
                return;
            }

            this.Providers[provider.Name] = provider;
            this.CurrentProvider = provider;
        }

        /// <summary>
        /// Sets an <see cref="IProvider"/> as the <see cref="CurrentProvider"/> if found by it's name.
        /// </summary>
        /// <param name="providerName">The provider name to be searched.</param>
        /// <returns>If the <see cref="CurrentProvider"/> was changed.</returns>
        public virtual bool SelectProvider(string providerName)
        {
            if (!this.TryGetProvider(providerName, out T provider))
            {
                return false;
            }

            return this.SelectProvider(provider);
        }

        /// <summary>
        /// Sets an <see cref="IProvider"/> as the <see cref="CurrentProvider"/> if found inside <see cref="Providers"/>.
        /// </summary>
        /// <param name="provider">The provider to be checked.</param>
        /// <returns>If the <see cref="CurrentProvider"/> was changed.</returns>
        public virtual bool SelectProvider(T provider)
        {
            if (!this.Providers.TryGetValue(provider.Name, out T p))
            {
                return false;
            }

            if (!ReferenceEquals(p, provider))
            {
                return false;
            }

            this.CurrentProvider = provider;
            return true;
        }

        /// <summary>
        /// Removes an <see cref="IProvider"/> from <see cref="Providers"/> if found by it's name and, if it's the <see cref="CurrentProvider"/>, nulls it.
        /// </summary>
        /// <param name="providerName">The provider name to be searched.</param>
        /// <returns>If the provider was removed from <see cref="Providers"/>.</returns>
        public virtual bool RemoveProvider(string providerName)
        {
            if (!this.TryGetProvider(providerName, out T provider))
            {
                return false;
            }

            return this.RemoveProvider(provider);
        }

        /// <summary>
        /// Removes an <see cref="IProvider"/> from <see cref="Providers"/> and, if it's the <see cref="CurrentProvider"/>, nulls it.
        /// </summary>
        /// <param name="provider">The provider to be checked.</param>
        /// <returns>If the provider was removed from <see cref="Providers"/>.</returns>
        public virtual bool RemoveProvider(T provider)
        {
            if (!this.Providers.TryGetValue(provider.Name, out T p))
            {
                return false;
            }

            if (!ReferenceEquals(p, provider))
            {
                this.CurrentProvider = default;
            }

            this.Providers.Remove(provider.Name);
            return true;
        }

        /// <summary>
        /// Gets an <see cref="IProvider"/> by the name.
        /// </summary>
        /// <param name="providerName">The provider name to be searched.</param>
        /// <param name="provider">The found provider.</param>
        /// <returns>If the provider was found.</returns>
        public virtual bool TryGetProvider(string providerName, out T provider)
        {
            if (!this.Providers.TryGetValue(providerName, out provider))
            {
                return false;
            }

            return provider != null;
        }
    }
}
