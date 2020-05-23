﻿using RicoClient.Scripts.Cards.Entities;
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
    public class BuildingCardScript : BaseCardScript
    {
        [SerializeField]
        protected TMP_Text _health = null;
        [SerializeField]
        protected TMP_Text _attack = null;
        [SerializeField]
        protected TMP_Text _resource = null;

        public override void FillCard(Card card)
        {
            base.FillCard(card);

            _health.text = card.Properties.Health.ToString();
            _attack.text = card.Properties.Attack.ToString();
            _resource.text = card.Properties.Resources.ToString();
        }
    }
}
