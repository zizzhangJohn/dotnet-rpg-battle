using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dot_battle.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CharacterType
    {
        Knight = 1,
        Mage = 2,
        Cleric = 3
    }
}