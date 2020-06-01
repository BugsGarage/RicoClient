using RicoClient.Scripts.Cards;
using UnityEngine;

namespace RicoClient.Scripts.Game.CardLogic.CurrentLogic
{
    public class BaseCurrentLogic : BaseLogic
    {
        public GameObject InHandCardHolder { get; private set; }

        public BaseCurrentLogic(BaseCardScript card, Transform parent) : base(card)
        {
            InHandCardHolder = _rectTransform.parent.gameObject;
            _rectTransform.SetParent(parent, true);
            InHandCardHolder.SetActive(false);
        }
    }
}
