using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotikotTools.RuntimeConsole
{
    [RequireComponent(typeof(Image))]
    public class LogView : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler
    {
        public event Action<PointerEventData.InputButton> OnClick;
        public event Action OnMouseEnter;
        public event Action OnMouseExit;

        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _messageLabel;

        private Image _backgroundImage;

        public LogData Data { get; private set; }
        public bool IsSelected { get; private set; }

        public bool IsTextOverflowing => _messageLabel.isTextOverflowing;

        private void Awake()
        {
            _backgroundImage = GetComponent<Image>();
        }

        public void SetData(LogData data)
        {
            Data = data;
            UpdateView();
        }

        public void Select()
        {
            IsSelected = true;
            _backgroundImage.color = ConsolePreferences.HighlightedLogBackgroundColor;
        }

        public void Deselect()
        {
            IsSelected = false;
            _backgroundImage.color = ConsolePreferences.NormalLogBackgroundColor;
        }

        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnter?.Invoke();
        public void OnPointerExit(PointerEventData eventData) => OnMouseExit?.Invoke();
        public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(eventData.button);

        private void UpdateView()
        {
            _messageLabel.text = Data.Message;
            _messageLabel.color = ConsolePreferences.GetLogTextColor(Data.Type);

            _iconImage.sprite = Data.Icon;
            _iconImage.gameObject.SetActive(Data.Icon != null);
        }
    }
}