using RicoClient.Scripts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace RicoClient.Scripts.Menu.Main
{
    public class MainMenuScript : MonoBehaviour
    {
        public void SetMainMenuActive()
        {
            gameObject.SetActive(true);
        }
    }
}
