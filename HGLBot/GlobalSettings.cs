using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace HGLBot
{
    public class GlobalSettings
    {
        private const string path = "./config/global.json";
        private static GlobalSettings _instance = new GlobalSettings();

        public static void Load()
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"{path} was not found.");
            _instance = JsonConvert.DeserializeObject<GlobalSettings>(File.ReadAllText(path));
        }
        public static void Save()
        {
            using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
                writer.Write(JsonConvert.SerializeObject(_instance, Formatting.Indented));
        }

        // API Keys
        public class APISettings
        {
            [JsonProperty("google")]
            public string GoogleKey;
        }
        [JsonProperty("apikeys")]
        private APISettings _apikeys = new APISettings();
        public static APISettings apiKeys => _instance._apikeys;

        // Discord
        public class DiscordSettings
        {
            [JsonProperty("token")]
            public string Token;
        }
        [JsonProperty("bot")]
        private DiscordSettings _discord = new DiscordSettings();
        public static DiscordSettings Discord => _instance._discord;


        // Users
        public class UserSettings
        {
            [JsonProperty("owner")]
            public ulong owner;
            [JsonProperty("dev")]
            public ulong DevId;
        }
        [JsonProperty("users")]
        private UserSettings _users = new UserSettings();
        public static UserSettings users => _instance._users;

    }
}
