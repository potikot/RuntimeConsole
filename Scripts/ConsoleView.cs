using UnityEngine;

namespace PotikotTools.RuntimeConsole
{
    public class ConsoleView : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private GameObject _canvas;

        [Header("Logs Scroll")]
        [SerializeField] private ConsoleLogsScroll _scroll;

        [Header("Tooltip")]
        [SerializeField] private TooltipView _tooltip;

        public void Start()
        {
            _scroll.Init(_tooltip);
        }

        public void Show()
        {
            _canvas.SetActive(true);
        }

        public void Hide()
        {
            _canvas.SetActive(false);
        }

        public void AddLog(LogData data)
        {
            _scroll.AddElement(data);
        }
    }
}