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
        private Image _areaImage;

        protected void Awake()
        {
            _areaImage = GetComponent<Image>();
        }

        public void HighlightArea()
        {
            _areaImage.enabled = true;
        }

        public void RemoveHighlight()
        {
            _areaImage.enabled = false;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_areaImage.enabled)
            {
                Debug.Log("Drop at board");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_areaImage.enabled)
            {
                Debug.Log("Board pointer enter");
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_areaImage.enabled)
            {
                Debug.Log("Board pointer exit");
            }
        }
    }
}
