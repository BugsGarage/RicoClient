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
    public class ModalWarn : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _warnText = null;
        [SerializeField]
        private Button _okButton = null;
        [SerializeField]
        private Button _cancelButton = null;

        public void SetWarnDialog(string text, Action okAction)
        {
            _warnText.text = text;
            _okButton.onClick.AddListener(new UnityAction(okAction));

            gameObject.SetActive(true);
        }

        public void SetWarnDialog(string text, Action okAction, Action cancelAction)
        {
            _warnText.text = text;
            _okButton.onClick.AddListener(new UnityAction(okAction));
            _cancelButton.onClick.AddListener(new UnityAction(cancelAction));

            gameObject.SetActive(true);
        }
    }
}
