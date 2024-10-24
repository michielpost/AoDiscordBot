using ArweaveAO.Models.Token;

namespace AoDiscordBot.Services
{
    public static class SystemTokens
    {
        public static List<TokenData> GetAll()
        {
            var list = new List<TokenData>();

            list.Add(
               new TokenData
               {
                   TokenId = Constants.AoTokenId,
                   Denomination = 12,
                   Logo = "UkS-mdoiG8hcAClhKK8ch4ZhEzla0mCPDOix9hpdSFE",
                   Name = "AO",
                   Ticker = "AO"
               }); //AO

            list.Add(
               new TokenData
               {
                   TokenId = Constants.AoProxyTokenId,
                   Denomination = 12,
                   Logo = "UkS-mdoiG8hcAClhKK8ch4ZhEzla0mCPDOix9hpdSFE",
                   Name = "AO",
                   Ticker = "AO"
               }); //AO PROXY

            list.Add(
               new TokenData
               {
                   TokenId = Constants.CredTokenId,
                   Denomination = 3,
                   Logo = "eIOOJiqtJucxvB4k8a-sEKcKpKTh9qQgOV3Au7jlGYc",
                   Name = "AOCRED",
                   Ticker = "testnet-AOCRED"
               }); //CRED

            list.Add(
              new TokenData
              {
                  TokenId = Constants.LlamaTokenId,
                  Denomination = 12,
                  Logo = "9FSEgmUsrug7kTdZJABDekwTGJy7YG7KaN5khcbwcX4",
                  Name = "Llama Coin",
                  Ticker = "LLAMA"
              }); //LLAMA


            list.Add(
                new TokenData
                {
                    TokenId = "8p7ApPZxC_37M06QHVejCQrKsHbcJEerd3jWNkDUWPQ",
                    Denomination = 3,
                    Logo = "AdFxCN1eEPboxNpCNL23WZRNhIhiamOeS-TUwx_Nr3Q",
                    Name = "Bark",
                    Ticker = "BRKTST"
                }); //BARK

            list.Add(
                new TokenData
                {
                    TokenId = "OT9qTE2467gcozb2g8R6D6N3nQS94ENcaAIJfUzHCww",
                    Denomination = 3,
                    Logo = "4eTBOaxZSSyGbpKlHyilxNKhXbocuZdiMBYIORjS4f0",
                    Name = "TRUNK",
                    Ticker = "TRUNK"
                });  //TRUNK

            list.Add(
              new TokenData
              {
                  TokenId = "aYrCboXVSl1AXL9gPFe3tfRxRf0ZmkOXH65mKT0HHZw",
                  Denomination = 6,
                  Logo = "wfI-5PlYXL66_BqquCXm7kq-ic1keu0b2CqRjw82yrU",
                  Name = "AR.IO EXP",
                  Ticker = "EXP"
              });

            list.Add(
                new TokenData
                {
                    TokenId = "BUhZLMwQ6yZHguLtJYA5lLUa9LQzLXMXRfaq9FVcPJc",
                    Denomination = 12,
                    Logo = "quMiswyIjELM0FZtjVSiUtg9_-pvQ8K25yfxrp1TgnQ",
                    Name = "0rbit Points",
                    Ticker = "0RBT"
                });  //0rbit

            list.Add(
               new TokenData
               {
                   TokenId = "PBg5TSJPQp9xgXGfjN27GA28Mg5bQmNEdXH2TXY4t-A",
                   Denomination = 12,
                   Logo = "VzvP24VxdNt1kf3E-EXxxrihaNBnXpEI-5ymwWddJRk",
                   Name = "Earth",
                   Ticker = "EARTH"
               });

            list.Add(
               new TokenData
               {
                   TokenId = "KmGmJieqSRJpbW6JJUFQrH3sQPEG9F6DQETlXNt4GpM",
                   Denomination = 12,
                   Logo = "jayAVj1wgIcmin0bjG_DIGxq3_qANSp5EV7PcfUAvdQ",
                   Name = "Fire",
                   Ticker = "FIRE"
               });

            list.Add(
               new TokenData
               {
                   TokenId = "2nfFJb8LIA69gwuLNcFQezSuw4CXPE4--U-j-7cxKOU",
                   Denomination = 12,
                   Logo = "7WqV5FWdDcbQzQNxNvfpr093yLHDtjeO7qPM9HQskWE",
                   Name = "Air",
                   Ticker = "AIR"
               });

            list.Add(
               new TokenData
               {
                   TokenId = "NkXX3uZ4oGkQ3DPAWtjLb2sTA-yxmZKdlOlEHqMfWLQ",
                   Denomination = 12,
                   Logo = "ioI2_z6qkzGBrvZXbojjf6Q5uVZumx4rDDdHm-Jfyt0",
                   Name = "Lava",
                   Ticker = "FIRE-EARTH"
               });

            list.Add(new TokenData
            {
                TokenId = "GcFxqTQnKHcr304qnOcq00ZqbaYGDn4Wbb0DHAM-wvU",
                Denomination = 12,
                Logo = "K8nurc9H0_ZQm17jbs3ryEs6MrlX-oIK_krpprWlQ-Q",
                Name = "Astro USD (Test)",
                Ticker = "USDA-TST"
            });

            return list;

        }
    }
}
