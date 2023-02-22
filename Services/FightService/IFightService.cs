using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dot_battle.Dtos.Fight;


namespace dot_battle.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);
    }
}