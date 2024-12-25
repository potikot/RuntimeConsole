using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PotikotTools.RuntimeConsole
{
    public class CommandInputView : MonoBehaviour
    {
        public event Action<string> OnCommandChanged;
        public event Action<string> OnCommandSubmitted;

        public event Action<string> OnFocused;
        public event Action OnUnfocused;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _submitButton;

        private bool _isNotEmptyLastChange;

        public bool IsEmpty => string.IsNullOrEmpty(_inputField.text);

        private void OnEnable()
        {
            _inputField.onValueChanged.AddListener(Internal_OnCommandChanged);

            _inputField.onSubmit.AddListener(Internal_OnCommandSubmitted);
            _submitButton.onClick.AddListener(Internal_OnCommandSubmitted);

            _inputField.onSelect.AddListener(Internal_OnFocused);
            _inputField.onDeselect.AddListener(Internal_OnUnfocused);
        }

        private void OnDisable()
        {
            _inputField.onValueChanged.RemoveListener(Internal_OnCommandChanged);

            _inputField.onSubmit.RemoveListener(Internal_OnCommandSubmitted);
            _submitButton.onClick.RemoveListener(Internal_OnCommandSubmitted);

            _inputField.onSelect.RemoveListener(Internal_OnFocused);
            _inputField.onDeselect.RemoveListener(Internal_OnUnfocused);
        }

        public void Focus()
        {
            _inputField.ActivateInputField();
        }

        public void SetInput(string value)
        {
            _inputField.text = value;
            _inputField.caretPosition = value.Length;
        }

        public void SetInputWithoutNotify(string value)
        {
            _inputField.SetTextWithoutNotify(value);
        }

        public void ClearInput()
        {
            SetInputWithoutNotify(string.Empty);
        }

        private void RemoveLeftSpaces()
        {
            if (IsEmpty || _inputField.text[0] != ' ')
                return;

            int firstCharacterIndex = 0, limit = _inputField.text.Length;
            for (; firstCharacterIndex < limit; firstCharacterIndex++)
                if (_inputField.text[firstCharacterIndex] != ' ')
                    break;

            _inputField.SetTextWithoutNotify(_inputField.text[firstCharacterIndex..]);
        }

        private void Internal_OnCommandChanged(string text)
        {
            RemoveLeftSpaces();

            if (!IsEmpty || _isNotEmptyLastChange)
            {
                OnCommandChanged?.Invoke(text);
            }

            _isNotEmptyLastChange = !IsEmpty;
        }

        private void Internal_OnCommandSubmitted()
        {
            Internal_OnCommandSubmitted(_inputField.text);
        }

        private void Internal_OnCommandSubmitted(string text)
        {
            OnCommandSubmitted?.Invoke(text);
        }

        private void Internal_OnFocused(string text)
        {
            OnFocused?.Invoke(text);
        }

        private void Internal_OnUnfocused(string text)
        {
            OnUnfocused?.Invoke();
        }
    }
}