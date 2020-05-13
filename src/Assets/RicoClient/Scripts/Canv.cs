using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.RicoClient.Scripts
{
    public class Canv : MonoBehaviour
    {
        Canvas m_Canvas;

        void Start()
        {
            m_Canvas = GetComponent<Canvas>();
            m_Canvas.renderMode = RenderMode.WorldSpace;
        }

        void Update()
        {

        }
    }
}
