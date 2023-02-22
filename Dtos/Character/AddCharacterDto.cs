using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dot_battle.Dtos
{
    public class AddCharacterDto
    {
        public string Name { get; set; } = null!;
        public CharacterType CharacterType { get; set; }
    }
}