#if EXILED
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
#else
using LabApi.Features.Wrappers;
using LabApi.Loader;
#endif
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Callvote.Features
{
    /// <summary>
    /// Represents the type that handles changing Callvote's translations.
    /// </summary>
    public class ChangeTranslation
    {
        private static IReadOnlyDictionary<string, string> LanguageByCountryCodeDictionary { get; } = new Dictionary<string, string>()
        {
            ["en"] = "english",
            ["cn"] = "chinese",
            ["br"] = "portuguese",
            ["pt"] = "portuguese",
            ["ru"] = "russian",
            ["kz"] = "russian",
            ["by"] = "russian",
            ["ua"] = "russian",
            ["fr"] = "french",
            ["tr"] = "turkish",
            ["cy"] = "turkish",
            ["pl"] = "polish",
        };

        /// <summary>
        /// Gets and loads a translation from Callvote's Github Repository.
        /// </summary>
        /// <param name="language">The language that will be loaded.</param>
        /// <returns>If downloading and loading the translation file was sucessfull.</returns>
        public static async Task<bool> LoadTranslation(string language)
        {
            try
            {
                using (HttpClient client = new())
                {
                    string githubTranslationLink = $"https://raw.githubusercontent.com/Unbistrackted/Callvote/EXILED/Callvote/Translations/{GetLanguage(language)}.yml";
#if EXILED
                    string path = LoaderPlugin.Config.ConfigType == ConfigType.Default ? Paths.Translations : Paths.GetTranslationPath(CallvotePlugin.Instance.Prefix);
#else
                    string path = CallvotePlugin.Instance.GetConfigPath("translation");
#endif

                    string file = await client.GetStringAsync(githubTranslationLink);

                    RewriteTranslationFile(file, path);
                }
#if EXILED
                CallvotePlugin.Instance.LoadTranslation();
#else
                CallvotePlugin.Instance.LoadConfigs();
#endif
                ServerSpecificSettings.UnregisterSettings();
                ServerSpecificSettings.RegisterSettings();
                return true;
            }
            catch (Exception ex)
            {
                ServerConsole.AddLog($"[ERROR] [Callvote] {ex.Message}:\n {ex.StackTrace}", ConsoleColor.Red);
                return false;
            }
        }

        private static string GetLanguage(string input)
        {
            if (LanguageByCountryCodeDictionary.Values.Contains(input))
            {
                return input;
            }

            if (!LanguageByCountryCodeDictionary.TryGetValue(input, out string language))
            {
                try
                {
                    using (WebClient client = new())
                    {
                        string url = $"http://ipinfo.io/{Server.IpAddress}/country";
                        input = client.DownloadString(url).Trim().ToLower();
                    }
                }
                catch (WebException ex)
                {
                    input = "en";
                    ServerConsole.AddLog($"[ERROR] [Callvote] {ex.Message}", ConsoleColor.Red);
                }

                if (!LanguageByCountryCodeDictionary.TryGetValue(input, out language))
                {
                    language = LanguageByCountryCodeDictionary["en"];
                }
            }

            return language;
        }

        private static void RewriteTranslationFile(string rawGithubTranslation, string translationsPath)
        {
            YamlStream githubTranslation = [];
            YamlStream localTranslations = [];

            using (StringReader reader = new(rawGithubTranslation))
            {
                githubTranslation.Load(reader);
            }

            using (StreamReader reader = new(translationsPath))
            {
                localTranslations.Load(reader);
            }

            YamlMappingNode githubTranslationRoot = (YamlMappingNode)githubTranslation.Documents[0].RootNode;
            YamlMappingNode localTranslationsRoot = (YamlMappingNode)localTranslations.Documents[0].RootNode;

            YamlMappingNode githubCallvote = (YamlMappingNode)githubTranslationRoot.Children[new YamlScalarNode(CallvotePlugin.Instance.Prefix)];
            YamlMappingNode localCallvote = localTranslationsRoot;

            if (localTranslationsRoot.Children.ContainsKey(new YamlScalarNode(CallvotePlugin.Instance.Prefix)))
            {
                localCallvote = (YamlMappingNode)localTranslationsRoot.Children[new YamlScalarNode(CallvotePlugin.Instance.Prefix)];
            }

            foreach (var kvp in githubCallvote.Children)
            {
                localCallvote.Children[kvp.Key] = kvp.Value;
            }

            using (StreamWriter writer = new(translationsPath, false, new UTF8Encoding(true)))
            {
                localTranslations.Save(writer, false);
            }
        }
    }
}
