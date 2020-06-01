using RicoClient.Scripts.Cards;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicoClient.Scripts.Game.CardLogic.CurrentLogic
{
    public class MyCurrentSpellLogic : BaseCurrentLogic
    {
        public static event Action<BaseCardScript> OnSpellReturnedToHand;
        public static event Action<GameObject, BaseCardScript> OnSpellDroppedToTarget;

        public bool IsApllied { get; set; }

        private Canvas _canvas;

        private LineRenderer _aimLine;
        private Vector3 _aimTarget;
        private bool _isFocusedOnTarget;

        public MyCurrentSpellLogic(BaseCardScript card, Transform parent, LineRenderer aimLine) : base(card, parent)
        {
            _canvas = card.GetComponent<Canvas>();
            _aimLine = aimLine;

            IsApllied = false;
            _canvas.enabled = false;
            _aimLine.gameObject.SetActive(true);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (!_isFocusedOnTarget)
            {
                _aimLine.SetPosition(1, GetMousePosition(Input.mousePosition));
            }
            else
            {
                _aimLine.SetPosition(1, _aimTarget);
            }
        }

        public override void OnEndDrag()
        {
            RemoveAimTarget();
            _aimLine.gameObject.SetActive(false);

            if (IsApllied)
            {
                OnSpellDroppedToTarget?.Invoke(InHandCardHolder, CardScript);
            }
            else
            {
                InHandCardHolder.SetActive(true);
                _rectTransform.SetParent(InHandCardHolder.transform, false);
                CardScript.PlaceInHand();

                _canvas.enabled = true;

                OnSpellReturnedToHand?.Invoke(CardScript);
            }
        }

        public void SetAimTarget(Vector3 target)
        {
            _aimTarget = target;
            _isFocusedOnTarget = true;
        }

        public void RemoveAimTarget()
        {
            _isFocusedOnTarget = false;
        }

        private Vector3 GetMousePosition(Vector3 pos)
        {
            Canvas aimLineParentCanvas = _aimLine.transform.parent.GetComponent<Canvas>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(aimLineParentCanvas.transform as RectTransform,
                pos, Camera.main, out Vector2 movePos);

            Vector3 positionToReturn = aimLineParentCanvas.transform.TransformPoint(movePos);
            positionToReturn.z -= 0.15f;
            return positionToReturn;
        }
    }
}
