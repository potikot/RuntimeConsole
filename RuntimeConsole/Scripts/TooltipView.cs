using TMPro;
using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class TooltipView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _label;

        private RectTransform _rectTransform;

        private float _halfScreenWidth = Screen.width / 2f;
        private float _halfScreenHeight = Screen.height / 2f;

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
            {
                if (newPosition.x > _halfScreenWidth || newPosition.x - _rectTransform.sizeDelta.x >= 0f)
                    newPosition.x = Input.mousePosition.x - _rectTransform.sizeDelta.x;
            }
            if (newPosition.y + _rectTransform.sizeDelta.y > Screen.height)
            {
                if (newPosition.y > _halfScreenHeight || newPosition.y - _rectTransform.sizeDelta.y >= 0f)
                    newPosition.y = Input.mousePosition.y - _rectTransform.sizeDelta.y;
            }

            _rectTransform.anchoredPosition = newPosition;
        }
    }
}