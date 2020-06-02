using RicoClient.Scripts.Cards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicoClient.Scripts.Cards
{
    public class UnitCardScript : EntityCardScript
    {
        [SerializeField]
        protected TMP_Text _initiative = null;

        public override void FillCard(Card card)
        {
            base.FillCard(card);
            _initiative.text = card.Properties.Initiative.ToString();
        }

        public override void FillCard(Card card, int deckCardId)
        {
            FillCard(card);

            DeckCardId = deckCardId;
        }
    }
}
