using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dot_battle.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public AuthRepository(IMapper mapper, IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<ServiceResponse<GetUserDto>> GetUser()
        {
            var response = new ServiceResponse<GetUserDto>();
            try
            {
                var dbUser = await _context.Users
                    .Include(u => u.Characters!)
                    .ThenInclude(c => c.CharacterWeapon)
                    .FirstOrDefaultAsync(u => u.GoogleId == getUserId());
                if (dbUser is null)
                {
                    Console.WriteLine("user not in db");
                    //the googleId is not database
                    var newUser = new User
                    {
                        GoogleId = getUserId(),
                        UserName = getUserName(),
                        Email = getUserEmail(),
                        Characters = CreateDefaultCharacter()
                    };
                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();
                    response.Data = _mapper.Map<GetUserDto>(newUser);
                }
                else
                {
                    Console.WriteLine("retrieving user from db");
                    response.Data = _mapper.Map<GetUserDto>(dbUser);
                }
            }
            catch (Exception err)
            {
                response.Message = err.Message;
                response.Success = false;
            }
            return response;
        }

        private List<Character> CreateDefaultCharacter()
        {
            return new List<Character>
            {
                new Character{
                    Name="Jozan",
                    HitPoints = 100,
                    Strength = new Random().Next(25, 40),
                    Defense = new Random().Next(25, 40),
                    CharacterType = CharacterType.Knight,
                    CharacterWeapon = new CharacterWeapon{
                        Name="Greatsword",
                        Damage = 10
                    }
                },
                new Character{
                    Name="Mialee",
                    HitPoints = 70,
                    Strength = new Random().Next(40, 50),
                    Defense = new Random().Next(10, 25),
                    CharacterType = CharacterType.Mage,
                    CharacterWeapon = new CharacterWeapon{
                        Name="Longbow",
                        Damage = 5
                    }
                },
                new Character{
                    Name="Ember",
                    HitPoints = 120,
                    Strength = new Random().Next(10, 25),
                    Defense = new Random().Next(40, 50),
                    CharacterType = CharacterType.Cleric,
                    CharacterWeapon = new CharacterWeapon{
                        Name="WarHammer",
                        Damage = 15
                    }
                }
            };
        }

        private string getUserId()
        {
            return _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        private string getUserName()
        {
            return _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name);
        }
        private string getUserEmail()
        {
            return _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email);
        }

    }
}