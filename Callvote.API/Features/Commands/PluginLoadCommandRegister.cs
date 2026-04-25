#if !BAREBONES
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandSystem;
using LabApi.Features.Console;
using MEC;
using RemoteAdmin;

namespace Callvote.API.Features.Commands
{
    /// <summary>
    /// Represents the helper class for registering commands into Parent Commands and a waiter for EXILED.
    /// </summary>
    public static class PluginLoadCommandRegister
    {
        public static bool IsExiledPresent() => LabApi.Loader.PluginLoader.Dependencies.Any(asm => asm.GetName().Name.Contains("Exiled"));

        public static bool IsExiledLoaded() => LabApi.Loader.PluginLoader.EnabledPlugins.Any(plugin => plugin.Name == "Exiled Loader");

        public static bool IsCallvoteLoaded() => LabApi.Loader.PluginLoader.EnabledPlugins.Any(plugin => plugin.Name == "Callvote");

        public static void RegisterCommandsIntoParent(Assembly modulePlugin, Type parentCommandType)
        {
            foreach (ICommand command in modulePlugin
                         .GetTypes()
                         .Where(type => typeof(ICommand).IsAssignableFrom(type) && !typeof(ParentCommand).IsAssignableFrom(type))
                         .Select(type => (ICommand)Activator.CreateInstance(type)))
            {
                AddAndCheckForParentCommand(parentCommandType, command, CommandProcessor.RemoteAdminCommandHandler.AllCommands);
                AddAndCheckForParentCommand(parentCommandType, command, GameCore.Console.ConsoleCommandHandler.AllCommands);
                AddAndCheckForParentCommand(parentCommandType, command, QueryProcessor.DotCommandHandler.AllCommands);
            }
        }

        // TODO: Add overloads for specific commands in module instead
        public static void RegisterParentCommandIntoParent(Assembly modulePlugin, Type parentCommandType)
        {
            foreach (ParentCommand command in modulePlugin
                         .GetTypes()
                         .Where(type => typeof(ParentCommand).IsAssignableFrom(type))
                         .Select(type => (ParentCommand)Activator.CreateInstance(type)))
            {
                AddAndCheckForParentCommand(parentCommandType, command, CommandProcessor.RemoteAdminCommandHandler.AllCommands);
                AddAndCheckForParentCommand(parentCommandType, command, GameCore.Console.ConsoleCommandHandler.AllCommands);
                AddAndCheckForParentCommand(parentCommandType, command, QueryProcessor.DotCommandHandler.AllCommands);
            }
        }

        public static IEnumerator<float> RegisterCommands(Assembly modulePlugin, Type parentCommandType, bool isCommandToBeRegisteredParent)
        {
            if (IsExiledPresent())
            {
                int timer = 0;
                while (!IsExiledLoaded() && !IsCallvoteLoaded())
                {
                    if (timer >= 60)
                    {
                        Logger.Error($"Exiled took too long to load or Exiled.Loader and Callvote is not present. Aborting command registration for {modulePlugin.GetName().Name}.");
                        yield break;
                    }

                    timer += 1;
                    yield return Timing.WaitForSeconds(3f);
                }
            }

            try
            {
                Logger.Info($"Registering {modulePlugin.GetName().Name} Module Commands...");

                if (!isCommandToBeRegisteredParent)
                {
                    RegisterCommandsIntoParent(modulePlugin, parentCommandType);
                }
                else
                {
                    RegisterParentCommandIntoParent(modulePlugin, parentCommandType);
                }

                Logger.Info($"Successfully registered {modulePlugin.GetName().Name} Module Commands.");
            }
            catch (Exception ex)
            {
                Logger.Error($"Error registering commands for {modulePlugin.GetName().Name}: {ex.Message}");
            }
        }

        private static void AddAndCheckForParentCommand(Type parentCommandType, ICommand command, IEnumerable<ICommand> commands)
        {
            foreach (ICommand c in commands)
            {
                if (parentCommandType.IsInstanceOfType(c) && c is ParentCommand parentCommand)
                {
                    parentCommand.RegisterCommand(command);
                }
            }
        }
    }
}

#endif