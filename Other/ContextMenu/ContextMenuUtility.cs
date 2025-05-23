using System;
using System.Collections.Generic;
using UnityEngine;

namespace PotikotTools
{
    public static class ContextMenuUtility
    {
        private static ContextMenu _prefab;
        private static ContextMenu _menu;

        static ContextMenuUtility()
        {
            _prefab = Resources.Load<ContextMenu>("ContextMenu");
        }

        public static ContextMenu GetOrCreateMenu(Transform container, List<(string, Action)> content)
        {
            if (container == null || container is not RectTransform)
                return null;

            if (_menu == null)
                _menu = UnityEngine.Object.Instantiate(_prefab, container);
            else
            {
                _menu.Clear();
                _menu.transform.SetParent(container);
                _menu.Hide();
            }

            if (content != null)
                foreach (var item in content)
                    _menu.AddElement(item.Item1, item.Item2);

            return _menu;
        }
    }
}
