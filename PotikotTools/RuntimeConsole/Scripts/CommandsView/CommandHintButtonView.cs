using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PotikotTools.RuntimeConsole
{
    public class CommandHintButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _label;

        private Action _onClick;

        public Button Button => _button;

        private void OnEnable() => _button.onClick.AddListener(OnClick);
        private void OnDisable() => _button.onClick.RemoveListener(OnClick);

        public void SetData(string text, Action onClick)
        {
            _label.text = text;
            _onClick = onClick;
        }

        private void OnClick() => _onClick?.Invoke();
    }
}