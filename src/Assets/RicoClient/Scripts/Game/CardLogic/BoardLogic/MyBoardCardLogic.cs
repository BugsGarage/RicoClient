using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RicoClient.Scripts.Game.CardLogic.BoardLogic
{
    public class MyBoardCardLogic : BaseBoardLogic
    {
        public static event Action<UnitCardScript> OnCardPrepAttack;
        public static event Action<UnitCardScript> OnCardUnprepAttack;

        private Vector3 _aimTarget;
        private bool _isFocusedOnTarget;

        public bool CanAttack { get; set; }

        public MyBoardCardLogic(BaseCardScript card, LineRenderer aimLine) : base(card, aimLine)
        {
            //_canAttack = false;
            CanAttack = true;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (CardScript is UnitCardScript && CanAttack)
            {
                Vector3[] corners = new Vector3[4];
                _rectTransform.GetWorldCorners(corners);
                Vector3 upperCardSide = new Vector3((corners[1].x + corners[2].x) / 2, corners[1].y, corners[1].z);
                _aimLine.SetPositions(new Vector3[] { upperCardSide, upperCardSide });

                _aimLine.gameObject.SetActive(true);

                OnCardPrepAttack?.Invoke((UnitCardScript) CardScript);
            }
        }  

        public override void OnDrag(PointerEventData eventData)
        {
            if (CardScript is UnitCardScript && CanAttack)
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
        }

        public override void OnEndDrag()
        {
            _aimLine.gameObject.SetActive(false);

            OnCardUnprepAttack?.Invoke((UnitCardScript) CardScript);
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
            positionToReturn.z -= 0.2f;
            return positionToReturn;
        }
    }
}
