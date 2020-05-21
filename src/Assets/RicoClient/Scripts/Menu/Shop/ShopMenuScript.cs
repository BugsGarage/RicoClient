using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.Pay;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Shop
{
    public class ShopMenuScript : BaseMenuScript
    {
        private PayManager _pay;

        [SerializeField]
        private TMP_Text _playerBalance = null;

        [SerializeField]
        private BoughtCardModalScript _boughtCard = null;
        [SerializeField]
        private TMP_Text _randomCardCost = null;

        [Inject]
        public void Initialize(PayManager pay)
        {
            _pay = pay;
        }

        protected void OnEnable()
        {

        }

        public async void OnRandomBuyClick()
        {

        }

        public async void On10GoldBuyClick()
        {

        }

        public async void On100GoldBuyClick()
        {

        }

        public async void On1000GoldBuyClick()
        {

        }
    }
}
