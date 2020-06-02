using TMPro;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Modals
{
    public class ModalError : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _errorText = null;

        private Canvas _canvas;

        public void Start()
        {
            _canvas = GetComponent<Canvas>();

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
                _canvas.enabled = true;
            }
        }
    }
}
