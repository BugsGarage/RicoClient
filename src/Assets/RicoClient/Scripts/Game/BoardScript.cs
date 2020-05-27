using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RicoClient.Scripts.Game
{
    public class BoardScript : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private const int MaxOnBoardCardsCount = 7;

        [SerializeField]
        private GameObject _onBoardCardHolder = null;
        [SerializeField]
        private Vector2 _cardHolderOffset;
        [SerializeField]
        private LineRenderer _aimLine = null;

        public List<GameObject> OnBoardCards { get; private set; }

        private RectTransform _rectTransform;
        private Image _areaImage;

        private BaseCardScript _currentSelectedCard;
        private GameObject _currentSelectedCardHolder;
        private BaseCardScript _selectedCardPreview;

        private bool _isPointerOver;

        protected void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _areaImage = GetComponent<Image>();

            OnBoardCards = new List<GameObject>();
            _isPointerOver = false;
        }

        protected void Update()
        {
            if (_isPointerOver)
            {
                PlacePreviewCard();
            }
        }

        protected void OnDestroy()
        {
            foreach (var onBoardCard in OnBoardCards)
            {
                Destroy(onBoardCard);
            }
        }

        public void HighlightAreaWith(BaseCardScript selectedCard)
        {
            if (OnBoardCards.Count < MaxOnBoardCardsCount)
            {
                _areaImage.enabled = true;
                _currentSelectedCard = selectedCard;
            }
        }

        public void RemoveHighlight()
        {
            _areaImage.enabled = false;
        }

        public void AddCardOnBoard(BaseCardScript card)
        {
            var cardHolder = Instantiate(_onBoardCardHolder, transform);
            PlaceCardOnBoard(card, cardHolder);
            card.PlaceOnBoard(_aimLine, false);

            OnBoardCards.Add(cardHolder);
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_areaImage.enabled)
            {
                CanvasGroup cardCanvas = _selectedCardPreview.GetComponent<CanvasGroup>();
                cardCanvas.alpha = 1;
                cardCanvas.blocksRaycasts = true;

                _selectedCardPreview.PlaceOnBoard(_aimLine, true);

                Destroy(_currentSelectedCard.gameObject);

                OnBoardCards.Insert(_currentSelectedCardHolder.transform.GetSiblingIndex(), _currentSelectedCardHolder);

                _currentSelectedCardHolder = null;
                _isPointerOver = false;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_areaImage.enabled)
            {
                _currentSelectedCardHolder = Instantiate(_onBoardCardHolder, transform);
                
                // Another hack, because otherwise oncard text is not visualizing
                // Guess because moves from overlay canvas to screenspace, not googling anyway
                _selectedCardPreview = Instantiate(_currentSelectedCard.gameObject).GetComponent<BaseCardScript>();
                _selectedCardPreview.Copy(_currentSelectedCard);
                PlaceCardOnBoard(_selectedCardPreview, _currentSelectedCardHolder);

                CanvasGroup cardCanvas = _selectedCardPreview.GetComponent<CanvasGroup>();
                cardCanvas.alpha = 0.5f;
                cardCanvas.blocksRaycasts = false;

                _isPointerOver = true;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_areaImage.enabled)
            {
                Destroy(_currentSelectedCardHolder);

                _isPointerOver = false;
            }
        }

        private void PlacePreviewCard()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            RaycastResult boardResult = raycastResults.First();
            foreach (var raycastResult in raycastResults)
            {
                if (raycastResult.gameObject.GetComponent<BoardScript>() != null)
                {
                    boardResult = raycastResult;
                    break;
                }
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, boardResult.screenPosition, Camera.main, out Vector2 localPoint);
            for (int i = 0; i < OnBoardCards.Count; i++)
            {
                if (i == 0 && localPoint.x < OnBoardCards[i].transform.localPosition.x)
                {
                    _currentSelectedCardHolder.transform.SetAsFirstSibling();
                    break;
                }
                else if (localPoint.x <= OnBoardCards[i].transform.localPosition.x && localPoint.x > OnBoardCards[i - 1].transform.localPosition.x)
                {
                    _currentSelectedCardHolder.transform.SetSiblingIndex(i);
                    break;
                }
                else if (i == OnBoardCards.Count - 1 && localPoint.x > OnBoardCards[i].transform.localPosition.x)
                {
                    _currentSelectedCardHolder.transform.SetAsLastSibling();
                    break;
                }
            }
        }

        private void PlaceCardOnBoard(BaseCardScript card, GameObject cardHolder)
        {
            var initialSize = cardHolder.GetComponent<RectTransform>().sizeDelta - _cardHolderOffset;

            RectTransform cardTransform = card.GetComponent<RectTransform>();
            cardTransform.SetParent(cardHolder.transform, false);
            cardTransform.localPosition = Vector3.zero;
            cardTransform.sizeDelta = initialSize;
        }
    }
}
