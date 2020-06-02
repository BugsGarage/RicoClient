using RicoClient.Scripts.Cards.Entities;
using RicoClient.Scripts.Game.CardLogic;
using RicoClient.Scripts.Game.CardLogic.BoardLogic;
using RicoClient.Scripts.Game.CardLogic.CurrentLogic;
using RicoClient.Scripts.Game.CardLogic.HandLogic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RicoClient.Scripts.Cards
{
    public abstract class BaseCardScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, 
        IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        public static event Action<BaseCardScript> OnCardRightClick;
        public static event Action<BaseCardScript> OnCardLeftClick;

        private const string ResourceImagesPath = "Cards/Images/{0}";
        private const string ResourceDescriptionsPath = "Cards/Descriptions/{0}";

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

        public int Cost { get { return int.Parse(_cost.text); } }

        public BaseLogic Logic { get; private set; }
        public int CardId { get; private set; }
        public int DeckCardId { get; protected set; }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        public virtual void FillCard(Card card)
        {
            CardId = card.CardId;

            _name.text = card.Name;
            _rarity.text = card.Rarity.ToString();
            _cost.text = card.Cost.ToString();

            _image.sprite = Resources.Load<Sprite>(string.Format(ResourceImagesPath, CardId));
            _description.text = Resources.Load<TextAsset>(string.Format(ResourceDescriptionsPath, CardId))?.text;
        }

        public virtual void FillCard(Card card, int deckCardId)
        {
            FillCard(card);

            DeckCardId = deckCardId;
        }

        public void PlaceInHand()
        {
            Logic = new MyHandCardLogic(this);
        }

        public void Select(Transform parent)
        {
            Logic = new MyCurrentCardLogic(this, parent);
        }

        public void ActiveSpell(Transform parent, LineRenderer aimLine)
        {
            Logic = new MyCurrentSpellLogic(this, parent, aimLine);
        }

        public void PlaceOnBoard(LineRenderer aimLine, bool isMine)
        {
            Logic?.CardDropped();

            if (isMine)
            {
                Logic = new MyBoardCardLogic(this, aimLine);
                ((MyBoardCardLogic) Logic).CheckCardWarcry();
            }
            else
            {
                Logic = new EnemyBoardCardLogic(this, aimLine);
            }
        }

        public void Copy(BaseCardScript otherCard)
        {
            CardId = otherCard.CardId;
            DeckCardId = otherCard.DeckCardId;
            Logic = otherCard.Logic;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnCardRightClick?.Invoke(this);
                if (Logic != null)
                    Logic.OnRightClick();
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnCardLeftClick?.Invoke(this);
                if (Logic != null)
                    Logic.OnLeftClick();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (Logic != null)
                Logic.OnEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (Logic != null)
                Logic.OnExit();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (Logic != null)
                Logic.OnBeginDrag(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (Logic != null)
                Logic.OnEndDrag();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (Logic != null)
                Logic.OnDrag(eventData);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (Logic != null)
                Logic.OnDrop();
        }

        protected void Update()
        {
            if (Logic != null)
                Logic.OnUpdate();
        }
    }
}
