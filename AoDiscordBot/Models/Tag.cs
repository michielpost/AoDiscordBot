using System.Diagnostics;
using System.Text.Json.Serialization;

namespace AoDiscordBot.Models
{
    [DebuggerDisplay("{Name}: {Value}")]
    public class Tag
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("value")]
        public required string Value { get; set; }
    }
}
