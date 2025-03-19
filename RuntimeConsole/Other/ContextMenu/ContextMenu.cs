using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotikotTools
{
    public class ContextMenu : MonoBehaviour
    {
        private List<ContextMenuItem> _items = new();
        private List<Button> _itemViews = new();

        private RectTransform _rectTransform;
        private RectTransform _container;
        private Button _itemViewPrefab;

        public bool IsEnabled { get; private set; }

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _container = _rectTransform;

            _itemViewPrefab = Resources.Load<Button>("ContextMenuItemView");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                GameObject obj = GetFirstHoveredObject();

                if (obj != null
                    && (obj == _container.gameObject
                    || obj.transform.parent == null
                    || obj.transform.parent.gameObject == _container.gameObject))
                    return;

                Hide();
            }
        }

        public void Show()
        {
            if (IsEnabled) return;
            IsEnabled = true;

            SetInMousePosition();
            gameObject.SetActive(true);
            UpdateView();
        }

        public void Hide()
        {
            if (!IsEnabled) return;
            IsEnabled = false;

            gameObject.SetActive(false);
        }

        public void AddElement(string title, Action action)
        {
            if (string.IsNullOrEmpty(title) || action == null)
                return;

            action += Hide;
            action += () => Destroy(gameObject);
            ContextMenuItem item = new(title, action);
            _items.Add(item);

            UpdateView();
        }

        public void InsertElement(int index, string title, Action action)
        {
            if (index < 0 || index > _items.Count || string.IsNullOrEmpty(title) || action == null)
                return;

            action += Hide;
            ContextMenuItem item = new(title, action);
            _items.Insert(index, item);

            UpdateView();
        }

        public void Clear()
        {
            _items.Clear();

            foreach (var itemView in _itemViews)
                Destroy(itemView.gameObject);

            _itemViews.Clear();
        }

        private void UpdateView()
        {
            if (!IsEnabled || _items.Count <= _itemViews.Count)
                return;
            
            for (int i = _itemViews.Count; i < _items.Count; i++)
                InstantiateItemView(_items[i]);
        }

        private void SetInMousePosition()
        {
            Vector2 mousePosition = Input.mousePosition;
            _rectTransform.anchoredPosition = mousePosition;
        }

        private void InstantiateItemView(ContextMenuItem item)
        {
            Button itemView = Instantiate(_itemViewPrefab, _container);

            itemView.onClick.AddListener(item.Execute);
            itemView.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;

            _itemViews.Add(itemView);
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
