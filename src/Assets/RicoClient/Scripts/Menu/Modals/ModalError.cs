using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Modals
{
    public class ModalError : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _errorText = null;

        /// <summary>
        /// Workaround, because Start() is not calling with disabled GameObject
        /// Probably bad idea
        /// </summary>
        [Inject]
        public void Init()
        {
            Application.logMessageReceived += ErrorLog;
        }

        public void OnDestroy()
        {
            Application.logMessageReceived -= ErrorLog;
        }

        private void ErrorLog(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Error)
            {
                _errorText.text = condition;
                gameObject.SetActive(true);
            }
        }
    }
}
