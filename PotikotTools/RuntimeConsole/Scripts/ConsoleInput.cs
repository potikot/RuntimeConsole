using System;
using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class ConsoleInput : MonoBehaviour
    {
        public event Action OnToggleKeyPressed;

        private void Update()
        {
            if (Input.GetKeyDown(ConsolePreferences.ToggleKey))
                OnToggleKeyPressed?.Invoke();
        }
    }
}