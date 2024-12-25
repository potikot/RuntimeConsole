using UnityEngine;
using static UnityEngine.EventSystems.PointerEventData;

namespace PotikotTools.RuntimeConsole
{
    public class ConsoleLogsScroll : OptimizedScroll<LogView, LogData>
    {
        private TooltipView _tooltip;
        private int _selectedViewIndex = -1;
        private int _selectedDataIndex = -1;

        public bool HasSelectedElement => _selectedViewIndex != -1;
        public bool HasSelectedData => _selectedDataIndex != -1;

        private void OnDisable()
        {
            Deselect();
        }

        public void Init(TooltipView tooltip)
        {
            _tooltip = tooltip;
        }

        public void Select(LogView logView)
        {
            int index = IndexOfView(logView);

            if (index == -1)
                return;

            Select(index);
        }

        public void Deselect()
        {
            if (!HasSelectedData)
                return;

            _selectedDataIndex = -1;
            
            if (HasSelectedElement)
            {
                elements[_selectedViewIndex].Deselect();
                _selectedViewIndex = -1;
            }
        }

        protected override void OnSetupElement()
        {
            setupElement = view =>
            {
                view.OnClick += button =>
                {
                    switch (button)
                    {
                        case InputButton.Left:
                            if (view.IsSelected)
                                Deselect();
                            else
                                Select(view);

                            break;
                        case InputButton.Right:
                            var menu = ContextMenuUtility.GetOrCreateMenu(transform.parent.parent, null);
                            menu.AddElement("Copy", () => GUIUtility.systemCopyBuffer = view.Data.Message);
                            menu.Show();
                            break;
                    }
                };

                view.OnMouseEnter += () =>
                {
                    if (view.IsTextOverflowing)
                        _tooltip.Show(view.Data.Message);
                };

                view.OnMouseExit += () =>
                {
                    _tooltip.Hide();
                };
            };
        }

        protected override void SetElementData(LogView view, LogData data)
        {
            view.SetData(data);
        }

        protected override void OnScroll(bool next)
        {
            if (HasSelectedData)
            {
                int newSelected;
                if (HasSelectedElement)
                    newSelected = _selectedViewIndex + (next ? -1 : 1);
                else
                    newSelected = _selectedDataIndex - firstDataIndex;

                if (newSelected >= 0 && newSelected < simultaneousElementsAmount)
                    Select(newSelected);
                else
                    DisableSelectedView();
            }
        }

        private void Select(int index)
        {
            if (HasSelectedElement)
                elements[_selectedViewIndex].Deselect();

            _selectedViewIndex = index;
            elements[_selectedViewIndex].Select();

            _selectedDataIndex = IndexOfData(elements[_selectedViewIndex].Data);
        }

        private void DisableSelectedView()
        {
            if (!HasSelectedElement)
                return;

            elements[_selectedViewIndex].Deselect();
            _selectedViewIndex = -1;
        }

        private int IndexOfView(LogView element)
        {
            for (int i = 0; i < elements.Length; i++)
                if (element == elements[i])
                    return i;

            return -1;
        }

        private int IndexOfData(LogData data)
        {
            int lastDataIndex = Mathf.Min(firstDataIndex + simultaneousElementsAmount, elementDatas.Count);
            for (int i = firstDataIndex; i < lastDataIndex; i++)
                if (data == elementDatas[i])
                    return i;

            return -1;
        }
    }
}