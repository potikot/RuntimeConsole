using TMPro;
using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    [RequireComponent(typeof(RectTransform))]
    public class TooltipView : MonoBehaviour
    {
        [Header("Label")]
        [SerializeField] private TextMeshProUGUI _label;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = transform as RectTransform;

            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.zero;
            _rectTransform.pivot = Vector2.zero;
        }

        private void Update()
        {
            MoveToMousePosition();
        }

        public void SetData(string message)
        {
            _label.text = message;
        }

        public void Show(string message)
        {
            SetData(message);
            gameObject.SetActive(true);
        }

        public void Hide() => gameObject.SetActive(false);

        private void MoveToMousePosition()
        {
            Vector3 newPosition = Input.mousePosition;

            if (newPosition.x + _rectTransform.sizeDelta.x > Screen.width)
                newPosition.x = Input.mousePosition.x - _rectTransform.sizeDelta.x;
            if (newPosition.y + _rectTransform.sizeDelta.y > Screen.height)
                newPosition.y = Input.mousePosition.y - _rectTransform.sizeDelta.y;

            _rectTransform.anchoredPosition = newPosition;
        }
    }
}