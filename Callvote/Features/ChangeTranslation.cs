using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.RepresentationModel;
using Exiled.Loader;
using System.Net;
using System.IO;
using PluginAPI.Core;
using YamlDotNet.Serialization.NamingConventions;
using HarmonyLib;
using LiteNetLib.Utils;
using Exiled.API.Features;
using Server = PluginAPI.Core.Server;
using Log = Exiled.API.Features.Log;

namespace Callvote.Features
{
    public class ChangeTranslation
    {
        public static void LoadTranslation(string countryCode)
        {
            try
            {
                string githubTranslationLink = $"https://raw.githubusercontent.com/Unbistrackted/Callvote/EXILED/Callvote/Translations/{GetLanguage(countryCode)}.yml";

                using (WebClient client = new WebClient())
                {
                    RewriteTranslationFile(client.DownloadString(githubTranslationLink), Paths.Translations);
                }
                Callvote.Instance.LoadTranslation();
            }
            catch (Exception)
            {
                Callvote.Instance.LoadTranslation();
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

            using (StreamWriter writer = new StreamWriter(translationsPath))
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
