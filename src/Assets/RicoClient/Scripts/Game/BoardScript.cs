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

        private RectTransform _rectTransform;
        private Image _areaImage;

        private BaseCardScript _currentSelectedCard;
        private GameObject _currentSelectedCardHolder;
        private BaseCardScript _selectedCardPreview;

        private List<GameObject> _onBoardCards;

        private bool _isPointerOver;

        protected void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _areaImage = GetComponent<Image>();

            _onBoardCards = new List<GameObject>();
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
            foreach (var onBoardCard in _onBoardCards)
            {
                Destroy(onBoardCard);
            }
        }

        public void HighlightAreaWith(BaseCardScript selectedCard)
        {
            if (_onBoardCards.Count < MaxOnBoardCardsCount)
            {
                _areaImage.enabled = true;
                _currentSelectedCard = selectedCard;
            }
        }

        public void RemoveHighlight()
        {
            _areaImage.enabled = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_areaImage.enabled)
            {
                CanvasGroup cardCanvas = _selectedCardPreview.GetComponent<CanvasGroup>();
                cardCanvas.alpha = 1;
                cardCanvas.blocksRaycasts = true;

                _selectedCardPreview.PlaceOnBoard();

                Destroy(_currentSelectedCard.gameObject);

                _onBoardCards.Insert(_currentSelectedCardHolder.transform.GetSiblingIndex(), _currentSelectedCardHolder);

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
                PlaceCardOnBoard(_selectedCardPreview);

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
            for (int i = 0; i < _onBoardCards.Count; i++)
            {
                if (i == 0 && localPoint.x < _onBoardCards[i].transform.localPosition.x)
                {
                    _currentSelectedCardHolder.transform.SetAsFirstSibling();
                    break;
                }
                else if (localPoint.x <= _onBoardCards[i].transform.localPosition.x && localPoint.x > _onBoardCards[i - 1].transform.localPosition.x)
                {
                    _currentSelectedCardHolder.transform.SetSiblingIndex(i);
                    break;
                }
                else if (i == _onBoardCards.Count - 1 && localPoint.x > _onBoardCards[i].transform.localPosition.x)
                {
                    _currentSelectedCardHolder.transform.SetAsLastSibling();
                    break;
                }
            }
        }

        private void PlaceCardOnBoard(BaseCardScript card)
        {
            var initialSize = _currentSelectedCard.GetComponent<RectTransform>().sizeDelta;

            RectTransform cardTransform = card.GetComponent<RectTransform>();
            cardTransform.SetParent(_currentSelectedCardHolder.transform, false);
            cardTransform.localPosition = Vector3.zero;
            cardTransform.sizeDelta = initialSize;
        }
    }
}
