using RicoClient.Scripts.Cards;
using RicoClient.Scripts.Cards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace RicoClient.Scripts.Cards
{
    public abstract class EntityCardScript : BaseCardScript
    {
        [SerializeField]
        protected TMP_Text _health = null;
        [SerializeField]
        protected TMP_Text _attack = null;

        public int Attack { get { return int.Parse(_attack.text); } }
        public int Health { get { return int.Parse(_health.text); } }

        public override void FillCard(Card card)
        {
            base.FillCard(card);

            _health.text = card.Properties.Health.ToString();
            _attack.text = card.Properties.Attack.ToString();
        }

        public override void FillCard(Card card, int deckCardId)
        {
            FillCard(card);

            DeckCardId = deckCardId;
        }

        public void Damage(int healthValue)
        {
            if (healthValue >= 0)
            {
                int newHealth = Health - healthValue;
                _health.text = newHealth.ToString();
            }
        }

        public void ShiftStats(int healthDelta, int attackDelta)
        {
            int newHealth = Health + healthDelta;
            _health.text = newHealth.ToString();

            int newAttack = Attack + attackDelta;
            _attack.text = newAttack.ToString();
        }
    }
}
