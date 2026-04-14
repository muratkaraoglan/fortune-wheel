using System;
using UnityEngine;

namespace FortuneWheel.Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
        }
    }
}