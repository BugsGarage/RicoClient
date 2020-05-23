using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RicoClient.Scripts.Menu.Collection
{
    /// <summary>
    /// Script for deck in deck list panel in collection
    /// </summary>
    public class DeckScript : MonoBehaviour
    {
        public static event Action<DeckScript> OnDeckOpen;

        [SerializeField]
        private TMP_Text _name = null;

        public uint DeckId { get; private set; }

        public void SetDeck(uint deckId, string name)
        {
            DeckId = deckId;
            _name.text = name;
        }

        public void OnDeckClick()
        {
            OnDeckOpen?.Invoke(this);
        }
    }
}
