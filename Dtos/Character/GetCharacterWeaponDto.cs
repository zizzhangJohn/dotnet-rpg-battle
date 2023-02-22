using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dot_battle.Dtos
{
    public class GetCharacterWeaponDto
    {
        public string Name { get; set; } = null!;
        public int Damage { get; set; }
    }
}