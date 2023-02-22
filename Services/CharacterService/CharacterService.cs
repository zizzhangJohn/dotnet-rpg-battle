using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dot_battle.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        private string GetUserId() => _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var characterToAdd = _mapper.Map<Character>(newCharacter);

                if (characterToAdd.CharacterType == CharacterType.Knight)
                {
                    // a knight
                    var knightWeaponList = new List<string>{
                        "LongSword",
                        "Handaxe",
                        "Battleaxe",
                        "Shortsword",
                        "Greatsword"
                    };
                    var weaponName = knightWeaponList.ElementAt(new Random().Next(0, knightWeaponList.Count));
                    characterToAdd.Defense = new Random().Next(25, 50);
                    characterToAdd.Strength = new Random().Next(25, 50);
                    characterToAdd.CharacterWeapon = new CharacterWeapon
                    {
                        Name = weaponName,
                        Damage = new Random().Next(5, 10)
                    };
                }
                else if (characterToAdd.CharacterType == CharacterType.Mage)
                {
                    // a mage
                    var mageWeaponList = new List<string>{
                        "Crossbow",
                        "Shortbow",
                        "Longbow",
                        "Sling",
                        "Blowgun"
                    };
                    var weaponName = mageWeaponList.ElementAt(new Random().Next(0, mageWeaponList.Count));
                    characterToAdd.Defense = new Random().Next(1, 25);
                    characterToAdd.Strength = new Random().Next(25, 60);
                    characterToAdd.CharacterWeapon = new CharacterWeapon
                    {
                        Name = weaponName,
                        Damage = new Random().Next(3, 6)
                    };
                }
                else if (characterToAdd.CharacterType == CharacterType.Cleric)
                {
                    // a cleric
                    var clericWeaponList = new List<string>{
                        "Warhammer",
                        "Trident",
                        "Halbert",
                        "Lance",
                        "Maul"
                    };
                    var weaponName = clericWeaponList.ElementAt(new Random().Next(0, clericWeaponList.Count));
                    characterToAdd.Defense = new Random().Next(50, 75);
                    characterToAdd.Strength = new Random().Next(1, 25);
                    characterToAdd.CharacterWeapon = new CharacterWeapon
                    {
                        Name = weaponName,
                        Damage = new Random().Next(7, 12)
                    };
                }
                characterToAdd.User = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == GetUserId());
                _context.Characters.Add(characterToAdd);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters
                    .Include(c => c.CharacterWeapon)
                    .Where(c => c.User!.GoogleId == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
                serviceResponse.Message = "Character added successfully";
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int characterToDeleteId)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var dbCharacterToDelete = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterToDeleteId);
                if (dbCharacterToDelete is null)
                {
                    throw new Exception($"Character not found, cannot delete");
                }
                _context.Characters.Remove(dbCharacterToDelete);
                await _context.SaveChangesAsync();

                serviceResponse.Data = await _context.Characters
                    .Include(c => c.CharacterWeapon)
                    .Where(c => c.User!.GoogleId == GetUserId())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
                serviceResponse.Message = $"Character is deleted successfully";
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var dbCharacters = await _context.Characters
                    .Include(c => c.CharacterWeapon)
                    .Where(c => c.User!.GoogleId == GetUserId()).ToListAsync();
                serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var dbCharacter = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id && c.User!.GoogleId == GetUserId());
                if (dbCharacter is null)
                {
                    throw new Exception("character not found, cannot update");
                }
                _mapper.Map(updatedCharacter, dbCharacter);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
                serviceResponse.Message = $"Character with id '{updatedCharacter.Id}' has been successfully updated";
            }
            catch (Exception err)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = err.Message;
            }
            return serviceResponse;
        }
    }
}