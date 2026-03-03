using Callvote.API.Features.Commands.DefaultProviders;

namespace Callvote.API.Features.Commands
{
    public class CommandHandler
    {
#pragma warning disable SA1401 // Fields should be private
        public static CommandProvider CurrentProvider = new SecretLabCommandProvider();
#pragma warning restore SA1401 // Fields should be private
    }
}
