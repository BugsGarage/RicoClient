using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace RicoClient.Scripts.Menu.Modals
{
    public class ModalInfo : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _infoText = null;
        [SerializeField]
        private Button okButton = null;

        public void SetInfoDialog(string text, Action okAction)
        {
            _infoText.text = text;
            okButton.onClick.AddListener(new UnityAction(okAction));

            gameObject.SetActive(true);
        }
    }
}
