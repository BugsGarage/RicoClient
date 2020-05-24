using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Game.CardLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace RicoClient.Scripts.Cards
{
    public abstract class BaseCardScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public static event Action<BaseCardScript> OnCardRightClick;
        public static event Action<BaseCardScript> OnCardLeftClick;

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

        protected BaseLogic _logic { get; private set; }

        public int CardId { get; private set; }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void FillCard(Card card)
        {
            CardId = card.CardId;

            _name.text = card.Name;
            _rarity.text = card.Rarity;
            _cost.text = card.Cost.ToString();

            // ToDo: find image and description by cardId
            _description.text = "Your beatiful description";
        }

        public void PlaceInHand()
        {
            _logic = new MyHandCardLogic(this);
        }

        public void Select(Transform parent)
        {
            _logic = new MyCurrentCardLogic(this, parent);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnCardRightClick?.Invoke(this);
                if (_logic != null)
                    _logic.OnRightClick();
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnCardLeftClick?.Invoke(this);
                if (_logic != null)
                    _logic.OnLeftClick();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_logic != null)
                _logic.OnEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_logic != null)
                _logic.OnExit();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_logic != null)
                _logic.OnBeginDrag();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_logic != null)
                _logic.OnEndDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_logic != null)
                _logic.OnDrag(eventData.delta);
        }
    }
}
