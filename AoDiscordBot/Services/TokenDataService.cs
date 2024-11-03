using ArweaveAO.Models;
using ArweaveAO;
using Microsoft.Extensions.Options;
using ArweaveAO.Models.Token;
using System.Text.Json;
using System.Collections.Concurrent;

namespace AoDiscordBot.Services
{
    public class TokenDataService
    {
        private readonly TokenClient tokenClient;
        private readonly string cacheFilePath = "token_cache.json";
        private ConcurrentDictionary<string, TokenData> tokenCache;

        public TokenDataService(IOptions<ArweaveConfig> config, HttpClient httpClient)
        {
            tokenClient = new TokenClient(config, httpClient);
            tokenCache = LoadCache();
        }

        public async Task<TokenData?> GetTokenInfoByName(string tokenName)
        {
            //Only allowed for system tokens
            var tokenInfo = SystemTokens.GetAll().FirstOrDefault(x => x.Name.Equals(tokenName, StringComparison.InvariantCultureIgnoreCase) || x.Ticker.Equals(tokenName, StringComparison.InvariantCultureIgnoreCase));
            if (tokenInfo != null)
                return tokenInfo;

            return null;
        }


        public async Task<TokenData?> GetTokenInfo(string tokenId)
        {
            // Check by name
            var tokenInfo = await GetTokenInfoByName(tokenId);
            if (tokenInfo != null)
                return tokenInfo;


            // Check system tokens first
            tokenInfo = SystemTokens.GetAll().FirstOrDefault(x => x.TokenId.Equals(tokenId));
            if (tokenInfo != null)
                return tokenInfo;

            // Check cache
            if (tokenCache.TryGetValue(tokenId, out TokenData? cachedToken))
                return cachedToken;

            // If not in cache, fetch from API
            tokenInfo = await tokenClient.GetTokenMetaData(tokenId);
            if (tokenInfo != null)
            {
                // Add to cache and save
                tokenCache[tokenId] = tokenInfo;
                SaveCache();
            }

            return tokenInfo;
        }

        private ConcurrentDictionary<string, TokenData> LoadCache()
        {
            if (File.Exists(cacheFilePath))
            {
                string json = File.ReadAllText(cacheFilePath);
                var dictionary = JsonSerializer.Deserialize<Dictionary<string, TokenData>>(json);
                return new ConcurrentDictionary<string, TokenData>(dictionary ?? new Dictionary<string, TokenData>());
            }
            return new ConcurrentDictionary<string, TokenData>();
        }

        private void SaveCache()
        {
            string json = JsonSerializer.Serialize(tokenCache);
            File.WriteAllText(cacheFilePath, json);
        }
    }
}
