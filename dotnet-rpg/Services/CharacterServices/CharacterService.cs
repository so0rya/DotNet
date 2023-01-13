using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Models;
using dotnet_rpg.Dtos.Character;
using AutoMapper;

namespace dotnet_rpg.Services.CharacterService
{
   
    public class CharacterService: ICharacterService
    {
         private static List<Character> characters = new List<Character>
        {
            new Character(),
            new Character {Id=1, Name= " Sam " }
        };
        private IMapper _mapper;
        private object updatedCharacter;

        public CharacterService(IMapper mapper)
        {
            _mapper=mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {   
            var serviceResponse = new ServiceResponse<List<Character>>();
            Character character= _mapper.Map<Character>(newCharacter);
            character.Id= characters.Max(c=>c.Id)=1;
            characters.Add(character);
            serviceResponse.Data=characters.Select(c=>Mapper.Map<GetCharacterDto(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
             ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                Character character =characters.First(c=>c.Id == updatedCharacter.Id);
                characters.Remove(character);
                response.Data = characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();

            }
            catch (Exception ex){
                response.Success=false;
                response.Message=ex.Message;
            }

            return response;
        }

        public Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updated)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCharacter(object updatedCharacter)
        {
            throw new NotImplementedException();
        }
    }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
           return new ServiceResponse<List<GetCharacterDto>>{
           Data= characters.Select(c=>_mapper.Map<GetCharacterDto>(c)).ToList()
        };
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {   
            var serviceResponse = new ServiceResponse<Character>();
            var character= characters.FirstOrDefault(c => c.Id==id);
            serviceResponse.Data=_mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
         }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();
            try{
            Character character =characters.FirstOrDefault(c=>c.Id == updatedCharacter.Id);

            _mapper.Map(updatedCharacter,character);
            // character.Name=updatedCharacter.Name;
            // character.HitPoints=updatedCharacter.HitPoints;
            // character.Defense=updatedCharacter.Defense;     
            // character.Intelligence=updatedCharacter.Intelligence;
            // character.Strength=updatedCharacter.Strength;
            // character.Class=updatedCharacter.Class;


            response.Data= _mapper.Map<GetCharacterDto>(character);
            }
            catch (Exception ex){
                response.Success=false;
                response.Message=ex.Message;
            }

            return response;
        }

        
    }
