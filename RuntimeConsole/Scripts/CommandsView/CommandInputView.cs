using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotikotTools.RuntimeConsole
{
    public class CommandInputView : MonoBehaviour
    {
        public event Action<string> OnCommandChanged;
        public event Action<string> OnCommandSubmitted;

        public event Action OnFocused;
        public event Action OnUnfocused;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _submitButton;

        [SerializeField] private GameObject _commandHintButtonsContainer;

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

        private void Internal_OnCommandChanged(string text)
        {
            _inputField.SetTextWithoutNotify(_inputField.text.RemoveLeftSpaces());

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
            _inputField.SetTextWithoutNotify(_inputField.text.RemoveRightSpaces());
            OnCommandSubmitted?.Invoke(_inputField.text);
        }

        private void Internal_OnFocused(string text)
        {
            OnFocused?.Invoke();
        }

        private void Internal_OnUnfocused(string text)
        {
            if (IsCommandRelated(GetFirstHoveredObject()))
                return;

            OnUnfocused?.Invoke();
        }

        private bool IsCommandRelated(GameObject obj)
        {
            return obj != null && (obj.transform.parent.gameObject == _commandHintButtonsContainer || obj == _commandHintButtonsContainer);
        }

        private GameObject GetFirstHoveredObject()
        {
            if (EventSystem.current == null)
                return null;

            PointerEventData eventData = new(EventSystem.current) { position = Input.mousePosition };
            List<RaycastResult> raycastResults = new();

            EventSystem.current.RaycastAll(eventData, raycastResults);

            return raycastResults.Count > 0 ? raycastResults[0].gameObject : null;
        }
    }
}