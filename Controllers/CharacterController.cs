using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using dot_battle.Services.CharacterService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dot_battle.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        [HttpGet("GetAll")]
        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacter()
        {
            return await _characterService.GetAllCharacter();
        }
        [HttpPost("AddOne")]
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            return await _characterService.AddCharacter(newCharacter);
        }
        [HttpPut("UpdateOne")]
        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            return await _characterService.UpdateCharacter(updatedCharacter);
        }
        [HttpDelete("{id}")]
        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            return await _characterService.DeleteCharacter(id);
        }
    }
}