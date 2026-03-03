using System.Collections.Generic;
using Callvote.API.Enums;
using Callvote.API.Features.Commands.DefaultProviders;
using Callvote.API.Features.Generic;
using UnityEngine;

namespace Callvote.API.Features.Commands
{
    public class CommandHandler : BaseProviderHandler<CommandProvider>
    {
        private static CommandProvider currentProvider;

        /// <summary>
        /// Gets the <see cref="CommandHandler"/> instance.
        /// </summary>
        public static CommandHandler Instance { get; private set; } = new();

        /// <inheritdoc/>
        public override CommandProvider CurrentProvider
        {
            get => currentProvider ??= GetCommandProvider();
            internal set => currentProvider = value;
        }

        /// <inheritdoc/>
        public override Dictionary<string, CommandProvider> Providers { get; } = [];

        /// <inheritdoc/>
        public override ProviderType ProviderHandlerType => ProviderType.Command;

        public static void RegisterCommand(VoteCommand command)
        {
            Instance.CurrentProvider.RegisterCommand(command);
        }

        public static void UnregisterCommand(VoteCommand command)
        {
            Instance.CurrentProvider.UnregisterCommand(command);
        }

        private static CommandProvider GetCommandProvider()
        {
            string gameName = Application.productName;

            if (Application.productName == "SCPSL")
            {
                return new SecretLabCommandProvider();
            }

            return null;
        }
    }
}
