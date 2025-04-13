using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using YamlDotNet.RepresentationModel;
using Log = Exiled.API.Features.Log;
using Server = PluginAPI.Core.Server;

namespace Callvote.Features
{
    public class ChangeTranslation
    {
        public static void LoadTranslation(string countryCode)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string githubTranslationLink = $"https://raw.githubusercontent.com/Unbistrackted/Callvote/EXILED/Callvote/Translations/{GetLanguage(countryCode)}.yml";
                    string path = LoaderPlugin.Config.ConfigType == ConfigType.Default ? Paths.Translations : Paths.GetTranslationPath(Callvote.Instance.Prefix);
                    RewriteTranslationFile(client.DownloadString(githubTranslationLink), path);
                }
                Callvote.Instance.LoadTranslation();
            }
            catch (Exception)
            {
                TranslationManager.Reload();
            }
        }


        private static string GetLanguage(string countryCode)
        {
            if (!LanguageByCountryCodeDictionary.TryGetValue(countryCode, out string language))
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        string url = $"http://ipinfo.io/{Server.ServerIpAddress}/country";
                        countryCode = client.DownloadString(url).Trim();
                    }
                }
                catch (WebException ex)
                {
                    countryCode = "EN";
                    Log.Error(ex.Message);
                }

                if (!LanguageByCountryCodeDictionary.TryGetValue(countryCode, out language))
                {
                    language = LanguageByCountryCodeDictionary["EN"];
                }
            }
            return language;
        }

        private static void RewriteTranslationFile(string rawGithubTranslation, string translationsPath)
        {
            YamlStream githubTranslation = new YamlStream();
            YamlStream localTranslations = new YamlStream();

            using (StringReader reader = new StringReader(rawGithubTranslation))
            {
                githubTranslation.Load(reader);
            }

            using (StreamReader reader = new StreamReader(translationsPath))
            {
                localTranslations.Load(reader);
            }

            YamlMappingNode githubTranslationRoot = (YamlMappingNode)githubTranslation.Documents[0].RootNode;
            YamlMappingNode localTranslationsRoot = (YamlMappingNode)localTranslations.Documents[0].RootNode;

            YamlMappingNode githubCallvote = (YamlMappingNode)githubTranslationRoot.Children[new YamlScalarNode(Callvote.Instance.Prefix)];
            YamlMappingNode localCallvote = (YamlMappingNode)localTranslationsRoot.Children[new YamlScalarNode(Callvote.Instance.Prefix)];


            if (!localTranslationsRoot.Children.ContainsKey(Callvote.Instance.Prefix))
            {
                localTranslationsRoot.Add(Callvote.Instance.Prefix, githubTranslationRoot);
            }

            foreach (var kvp in githubCallvote.Children)
            {
                localCallvote.Children[kvp.Key] = kvp.Value;
            }

            using (StreamWriter writer = new StreamWriter(translationsPath, false, new UTF8Encoding(true)))
            {
                localTranslations.Save(writer, false);
            }
        }

        internal static IReadOnlyDictionary<string, string> LanguageByCountryCodeDictionary { get; } = new Dictionary<string, string>()
        {
            ["EN"] = "english",
            ["CN"] = "chinese",
            ["BR"] = "portuguese",
            ["PT"] = "portuguese",
            ["RU"] = "russian",
            ["KZ"] = "russian",
            ["BY"] = "russian",
            ["UA"] = "russian",
        };
    }
}
