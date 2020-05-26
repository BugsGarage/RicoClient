using RicoClient.Scripts.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RicoClient.Scripts.Game
{
    public class BaseBuildingScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
    {
        public static event Action<Vector3> OnBaseEnter;
        public static event Action OnBaseExit;
        public static event Action<BaseBuildingScript> OnOnDropped;

        private readonly Color SelectionColor = new Color(0.16f, 0.47f, 1, 0.59f);
        private readonly Color HighlightColor = new Color(1, 1, 1, 0.59f);

        [SerializeField]
        protected TMP_Text _health = null;
        [SerializeField]
        protected TMP_Text _resource = null;

        public int Resource { get { return int.Parse(_resource.text); } }
        public int Health { get { return int.Parse(_health.text); } }

        private RectTransform _rectTransform;
        private Image _highlightImage;

        protected void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _highlightImage = transform.parent.GetComponent<Image>();
        }

        public void FillBase(int health, int resources)
        {
            _health.text = health.ToString();
            _resource.text = resources.ToString();
        }

        public void Damage(int healthValue)
        {
            if (healthValue >= 0)
            {
                int newHealth = Health - healthValue;
                _health.text = newHealth.ToString();
            }
        }

        public void Highlight()
        {
            _highlightImage.color = HighlightColor;
            _highlightImage.enabled = true;
        }

        public void Unhighlight()
        {
            _highlightImage.enabled = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_highlightImage.enabled)
            {
                _highlightImage.color = SelectionColor;

                Vector3[] corners = new Vector3[4];
                _rectTransform.GetWorldCorners(corners);
                Vector3 downBaseSide = new Vector3((corners[0].x + corners[3].x) / 2, corners[0].y, corners[0].z);

                OnBaseEnter?.Invoke(downBaseSide);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_highlightImage.enabled)
            {
                _highlightImage.color = HighlightColor;

                OnBaseExit?.Invoke();
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (_highlightImage.enabled)
            {
                Debug.Log("Base Drop");

                OnOnDropped?.Invoke(this);
            }
        }
    }
}
