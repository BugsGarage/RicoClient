using RicoClient.Scripts.Menu.Collection;
using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Shop
{
    public class ShopMenuScript : BaseMenuScript
    {
        private UserManager _user;

        [Inject]
        public void Initialize(UserManager user)
        {
            _user = user;
        }
    }
}
