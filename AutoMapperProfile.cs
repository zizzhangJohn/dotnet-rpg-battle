using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dot_battle
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // user
            CreateMap<User, GetUserDto>();

            // character
            CreateMap<Character, GetCharacterDto>();
            CreateMap<AddCharacterDto, Character>();
            CreateMap<UpdateCharacterDto, Character>();

            // CharacterWeapon
            CreateMap<CharacterWeapon, GetCharacterWeaponDto>();


        }
    }
}