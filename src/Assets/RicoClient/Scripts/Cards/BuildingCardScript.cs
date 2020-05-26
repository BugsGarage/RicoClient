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
    public class BuildingCardScript : EntityCardScript
    {
        [SerializeField]
        protected TMP_Text _resource = null;

        public override void FillCard(Card card)
        {
            base.FillCard(card);

            _resource.text = card.Properties.Resources.ToString();
        }
    }
}
