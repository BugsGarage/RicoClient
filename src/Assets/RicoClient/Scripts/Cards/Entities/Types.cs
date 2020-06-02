using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RicoClient.Scripts.Cards.Entities
{
    public enum RarityType
    {
        None,
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }

    [Flags]
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

    [Flags]
    public enum AbilityTargetType
    {
        None = 0,
        Self = 1,
        Ally = 2,
        Enemy = 4
    }

    public enum AbilityTargetnessType
    {
        None,
        Pick
    }
}
