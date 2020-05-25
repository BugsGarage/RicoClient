using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace RicoClient.Scripts.Game.CardLogic
{
    public class BaseBuildingScript : MonoBehaviour
    {
        private Image _highlightImage;

        protected void Awake()
        {
            _highlightImage = transform.parent.GetComponent<Image>();
        }

        public void Highlight()
        {
            _highlightImage.enabled = true;
        }

        public void Unhighlight()
        {
            _highlightImage.enabled = false;
        }
    }
}
