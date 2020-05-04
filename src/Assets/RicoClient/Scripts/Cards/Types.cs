using Newtonsoft.Json;
using RicoClient.Scripts.Cards.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards
{
    public enum CardType
    {
        None = 0,
        Unit = 1,
        Building = 2,
        Base = 4,
        Spell = 8
    }

    public enum AbilityType
    {
        None,
        Damage,
        Buff,
        Heal
    }

    public enum AbilityActivationType
    {
        None,
        Warcry
    }

    public enum AbilityTargetType
    {
        None = 0,
        Ally = 1,
        Enemy = 2
    }

    public enum AbilityTargetnessType
    {
        None,
        Pick
    }
}
