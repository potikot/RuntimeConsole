using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PotikotTools
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class OptimizedScroll<VT, DT> : MonoBehaviour,
        IDragHandler, IScrollHandler
        where VT : Component
    {
        [Header("Container")]
        [SerializeField] protected RectTransform container;

        [Header("Settings")]
        [SerializeField] protected VT elementViewPrefab;
        [Space()]
        [SerializeField] protected int simultaneousElementsAmount = 10;
        [Space()]
        [SerializeField] protected bool isScrollAllowed = true;
        [SerializeField] protected bool isDragAllowed = true;

        protected delegate VT CreateElement();
        protected delegate void SetupElement(VT view);

        protected CreateElement createElement;
        protected SetupElement setupElement;

        protected VT[] elements;
        protected int lastElementIndex;

        protected List<DT> elementDatas = new();
        protected int firstDataIndex;

        protected virtual void Awake()
        {
            lastElementIndex = simultaneousElementsAmount - 1;
            Init();
        }

        private void Init()
        {
            if (GetComponentInChildren<Image>() == null)
                gameObject.AddComponent<Image>().color = Color.clear;

            OnCreateElement();
            OnSetupElement();

            InitializeView();
            UpdateView();
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

        public void Scroll(bool next)
        {
            if (next && firstDataIndex < elementDatas.Count - simultaneousElementsAmount)
            {
                firstDataIndex++;
                UpdateView();
                OnScroll(next);
            }
            else if (!next && firstDataIndex > 0)
            {
                firstDataIndex--;
                UpdateView();
                OnScroll(next);
            }
        }

        public void AddElement(DT data)
        {
            elementDatas.Add(data);

            if (firstDataIndex < elementDatas.Count - simultaneousElementsAmount)
                Scroll(true);
            else
                UpdateView();
        }

        public void SetElements(List<DT> datas)
        {
            elementDatas = datas;
            firstDataIndex = 0;

            UpdateView();
        }

        public void Clear()
        {
            elementDatas.Clear();
            firstDataIndex = 0;

            UpdateView();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragAllowed || eventData.button != PointerEventData.InputButton.Left)
                return;

            Scroll(eventData.delta.y > 0f);
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (!isScrollAllowed)
                return;

            Scroll(eventData.scrollDelta.y < 0f);
        }

        private void InitializeView()
        {
            elements = new VT[simultaneousElementsAmount];

            for (int i = 0; i < simultaneousElementsAmount; i++)
            {
                elements[i] = createElement.Invoke();
                elements[i].transform.SetParent(container);

                setupElement?.Invoke(elements[i]);
            }
        }

        private void UpdateView()
        {
            for (int i = 0; i < simultaneousElementsAmount; i++)
            {
                int dataIndex = firstDataIndex + i;
                if (dataIndex < elementDatas.Count)
                {
                    SetElementData(elements[i], elementDatas[dataIndex]);
                    elements[i].gameObject.SetActive(true);
                }
                else
                {
                    elements[i].gameObject.SetActive(false);
                }
            }
        }

        protected virtual void OnCreateElement()
        {
            if (elementViewPrefab == null)
            {
                createElement = () =>
                {
                    GameObject newElement = new("Element");
                    VT newElementViewComponent = newElement.AddComponent<VT>();

                    return newElementViewComponent;
                };
            }
            else
            {
                createElement = () =>
                {
                    VT newElement = Instantiate(elementViewPrefab);

                    return newElement;
                };
            }
        }

        protected virtual void OnSetupElement() { }
        protected virtual void OnScroll(bool next) { }

        protected abstract void SetElementData(VT view, DT data);
    }
}