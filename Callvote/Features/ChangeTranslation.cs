﻿using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Loader;
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
    public class ChangeTranslation
    {
        public static async Task<bool> LoadTranslation(string language)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string githubTranslationLink = $"https://raw.githubusercontent.com/Unbistrackted/Callvote/EXILED/Callvote/Translations/{GetLanguage(language)}.yml";
                    string path = LoaderPlugin.Config.ConfigType == ConfigType.Default ? Paths.Translations : Paths.GetTranslationPath(Callvote.Instance.Prefix);
                    string file = await client.GetStringAsync(githubTranslationLink);

                    RewriteTranslationFile(file, path);
                }
                Callvote.Instance.LoadTranslation();
                ServerSpecificSettings.UnregisterSettings();
                ServerSpecificSettings.RegisterSettings();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error($"Error while loading translation:\n {ex.Message}:\n {ex.StackTrace}");
                TranslationManager.Reload();
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
                    using (WebClient client = new WebClient())
                    {
                        string url = $"http://ipinfo.io/{Server.IpAddress}/country";
                        input = client.DownloadString(url).Trim().ToLower();
                    }
                }
                catch (WebException ex)
                {
                    input = "en";
                    Log.Error(ex.Message);
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
            YamlMappingNode localCallvote = localTranslationsRoot;
            if (localTranslationsRoot.Children.ContainsKey(new YamlScalarNode(Callvote.Instance.Prefix)))
            {
                localCallvote = (YamlMappingNode)localTranslationsRoot.Children[new YamlScalarNode(Callvote.Instance.Prefix)];
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
            ["pl"] = "polish"
        };
    }
}
