using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models;
using dotnet_rpg.Dtos.Character;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using dotnet_rpg.Dtos.Character.Skill;

namespace dotnet_rpg.Services.CharacterService
{
   
    public class CharacterService: ICharacterService
    {
        //  private static List<Character> characters = new List<Character>
        // {
        //     new Character(),
        //     new Character {Id=1, Name= " Sam " }
        // };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private object updatedCharacter;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper=mapper;
            _context=context;
        }

        private int GetUserId()=>int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {   
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character= _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            serviceResponse.Data= 
                await _context.Characters
                .Where(c=> c.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>>DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character = await _context.Characters.FirstOrDefaultAsync(c=>c.Id == id && c.User.Id == GetUserId());
                if (character != null)
                {
                    _context.Characters.Remove(character);
                    await _context.SaveChangesAsync();
                    response.Data = await _context.Characters
                                                .Where(c => c.User.Id == GetUserId())
                                                .Select(c=> _mapper.Map<GetCharacterDto>(c)).ToListAsync();
                }
            
                else
                {
                    response.Success = false;
                    response.Message =" Character not found";
                }

            
            }            
            catch (Exception ex)
            {
                response.Success=false;
                response.Message=ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters= await _context.Characters.Where(c=> c.User.Id == GetUserId()).ToListAsync();
            response.Data=dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return response; 
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter= await _context.Characters.FirstOrDefaultAsync(c => c.Id==id && c.User.Id == GetUserId());
            serviceResponse.Data=_mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
           ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters.Include(c => c.User).FirstOrDefaultAsync(c=>c.Id == updatedCharacter.Id);

                if(character.User.Id == GetUserId())
                {
            // _mapper.Map(updatedCharacter,character);
                    character.Name=updatedCharacter.Name;
                    character.HitPoints=updatedCharacter.HitPoints;
                    character.Defense=updatedCharacter.Defense;     
                    character.Intelligence=updatedCharacter.Intelligence;
                    character.Strength=updatedCharacter.Strength;
                    character.Class=updatedCharacter.Class;
                
                    await _context.SaveChangesAsync();
                    response.Data= _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    response.Success=false;
                    response.Message = "Character not found";
                }
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message=ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c=> c.Id == newCharacterSkill.CharacterId && c.User.Id == GetUserId());
                
                if (character == null)
                {
                    response.Success = false;
                    response.Message = "character not found";
                    return response;
                }

                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill == null)
                {
                    response.Success = false;
                    response.Message = "Skill not found";
                    return response;
                }
                character.Skills.Add(skill);
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
            }
            return response;
        }
    } 
}
