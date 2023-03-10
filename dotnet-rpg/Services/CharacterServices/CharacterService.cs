using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models;
using dotnet_rpg.Dtos.Character;
using AutoMapper;
using dotnet_rpg.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
   
    public class CharacterService: ICharacterService
    {
         private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character {Id=1, Name= " Sam " }
        };
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private object updatedCharacter;

        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper=mapper;
            _context=context;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {   
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character= _mapper.Map<Character>(newCharacter);
            
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            serviceResponse.Data= 
                await _context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>>DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await _context.Characters.FirstOrDefaultAsync(c=>c.Id == id);
                if (character is null)
                    throw new Exception($"Character with Id '{id} not found");
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                response.Data = await _context.Characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToListAsync();

            }
            catch (Exception ex){
                response.Success=false;
                response.Message=ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters= await _context.Characters.ToListAsync();
            response.Data=dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter= await _context.Characters.FirstOrDefaultAsync(c => c.Id==id);
            serviceResponse.Data=_mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
           ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try{
                var character=
                await _context.Characters.FirstOrDefaultAsync(c=>c.Id == updatedCharacter.Id);

            _mapper.Map(updatedCharacter,character);
            // character.Name=updatedCharacter.Name;
            // character.HitPoints=updatedCharacter.HitPoints;
            // character.Defense=updatedCharacter.Defense;     
            // character.Intelligence=updatedCharacter.Intelligence;
            // character.Strength=updatedCharacter.Strength;
            // character.Class=updatedCharacter.Class;

            await _context.SaveChangesAsync();
            response.Data= _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex)
            {
                response.Success=false;
                response.Message=ex.Message;
            }
            return response;
        }
    } 
}
