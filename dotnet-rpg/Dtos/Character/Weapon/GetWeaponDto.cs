using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Dtos.Character.Weapon
{
    public class GetWeaponDto
    {
        public string Weapon { get; set; } = string.Empty;

        public int Damage { get; set; }
    }
}