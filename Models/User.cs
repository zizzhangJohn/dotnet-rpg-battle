using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dot_battle.Models
{
    public class User
    {
        public int Id { get; set; }
        public string GoogleId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<Character>? Characters { get; set; }
    }
}