using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dot_battle.Dtos.Fight;

namespace dot_battle.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public FightService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.HttpContextAccessor = httpContextAccessor;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var serviceResponse = new ServiceResponse<FightResultDto>();
            try
            {
                var dbUser = await _context.Users.Include(u => u.Characters)!.ThenInclude(u => u.CharacterWeapon).FirstOrDefaultAsync(u => u.GoogleId == GetUserId());
                if (dbUser is null)
                {
                    throw new Exception("user not found, pls register to use the service");
                }
                if (dbUser.Characters is null)
                {
                    throw new Exception("user does not have any characters, pls create characters to start the fight");
                }
                var charactersToFight = dbUser.Characters.Where(c => request.CharacterIds.Contains(c.Id)).ToList();
                serviceResponse.Data = FightSimulator(charactersToFight);
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }
            return serviceResponse;

        }

        private FightResultDto FightSimulator(List<Character> charactersToFight)
        {
            var fightResult = new FightResultDto();
            var rnd = new Random();
            charactersToFight = charactersToFight.OrderBy(c => rnd.Next()).ToList();
            var defeatedCharacters = new List<Character>();
            var round = 1000;
            while (charactersToFight.Count > 1 && round > 0)
            {

                foreach (var attacker in charactersToFight)
                {
                    if (defeatedCharacters.Contains(attacker))
                    {
                        continue;
                    }
                    var opponents = charactersToFight.Where(c => c.Id != attacker.Id && !defeatedCharacters.Contains(c)).ToList();
                    var opponent = opponents[rnd.Next(0, opponents.Count)];

                    int damage = 0;
                    string attackUsed = string.Empty;
                    // randomize number 1 or 0
                    bool useWeapon = new Random().Next(0, 2) == 0;
                    if (useWeapon && attacker.CharacterWeapon is not null)
                    {
                        // attack with weapon
                        attackUsed = attacker.CharacterWeapon.Name;
                        damage = attacker.CharacterWeapon.Damage + (new Random().Next(attacker.Strength));
                        damage -= new Random().Next(opponent.Defense);
                    }
                    else
                    {
                        // attack without weapon
                        attackUsed = "bare hands";
                        damage = new Random().Next(attacker.Strength / 2, attacker.Strength);
                    }


                    if (damage > 0)
                    {
                        opponent.HitPoints -= damage;
                    }
                    fightResult.Log.Add($"{attacker.Name} attacks {opponent.Name} with {attackUsed} causing {(damage >= 0 ? damage : 0)} damage");
                    if (opponent.HitPoints <= 0)
                    {
                        fightResult.Log.Add($"{opponent.Name} has been defeated");
                        defeatedCharacters.Add(opponent);
                    }
                }
                round--;
                charactersToFight.RemoveAll(c => defeatedCharacters.Contains(c));
            }

            if (charactersToFight.Count != 1)
            {
                throw new Exception("Fight didn't work as expected, there are more than 1 winners");
            }
            var winner = charactersToFight.ElementAt(0);
            fightResult.Log.Add($"{winner.Name} wins with {winner.HitPoints}HP left");
            return fightResult;
        }
    }
}