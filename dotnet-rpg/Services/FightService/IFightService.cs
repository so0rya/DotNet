using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character.Fight;
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);

        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);

        Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request);

        Task<ServiceResponse<List<HighScoreDto>>> GetHighscore();

    }
}