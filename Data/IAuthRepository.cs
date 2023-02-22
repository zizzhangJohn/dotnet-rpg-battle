using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dot_battle.Dtos;
using dot_battle.Models;

namespace dot_battle.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<GetUserDto>> GetUser();
    }
}