using RicoClient.Scripts.Cards.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RicoClient.Scripts.Cards
{
    public abstract class BaseCardScript : MonoBehaviour
    {
        [SerializeField]
        protected TMP_Text _name = null;
        [SerializeField]
        protected Image _image = null;
        [SerializeField]
        protected TMP_Text _rarity = null;
        [SerializeField]
        protected TMP_Text _cost = null;
        [SerializeField]
        protected TMP_Text _description = null;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void FillCard(Card card)
        {
            _name.text = card.Name;
            _rarity.text = card.Rarity;
            _cost.text = card.Cost.ToString();

            // ToDo find image and description by cardId
            _description.text = "Your beatiful description";
        }
    }
}
