using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dot_battle.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int HitPoints { get; set; } = 100;
        public int Strength { get; set; }
        public int Defense { get; set; }
        public CharacterType CharacterType { get; set; }
        public CharacterWeapon? CharacterWeapon { get; set; }
        public User? User { get; set; }

    }
}