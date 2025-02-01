using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private InventorySlotUI _inventorySlotUIPrefab;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private RectTransform _deleteButtonRect;
        [SerializeField] private int _deleteButtonOffset;
        [SerializeField] private List<InventorySlotUI> _inventorySlotsUI = new List<InventorySlotUI>();
        [SerializeField] private InventorySlotUI _slotToDelete;


        private void OnEnable()
        {
            EventManager.onSelectInventorySlot += ShowDeleteButton;
        }

        private void OnDisable()
        {
            EventManager.onSelectInventorySlot -= ShowDeleteButton;
        }

        public void CreateSlots(int count)
        {
            float slotWidth = _inventorySlotUIPrefab._rectTransform.rect.width;
            float slotHeight = _inventorySlotUIPrefab._rectTransform.rect.height;

            int maxSlotInLine = Mathf.FloorToInt(_rectTransform.rect.width / slotWidth);
            float padding = (this._rectTransform.rect.width - (maxSlotInLine * slotWidth)) / 2;

            Vector2 startPosition = new Vector2(padding + slotWidth / 2, - padding - slotHeight / 2);
            Vector2 currentPosition = startPosition;

            for (int i = 0; i < count; i++)
            {
                var slot = Instantiate(_inventorySlotUIPrefab, this.transform);
                _inventorySlotsUI.Add(slot);

                slot._rectTransform.anchorMin = new Vector2(0, 1);
                slot._rectTransform.anchorMax = new Vector2(0, 1);
                slot._rectTransform.pivot = new Vector2(0.5f, 0.5f);

                slot._rectTransform.anchoredPosition = currentPosition;

                slot._rectTransform.SetAsFirstSibling();

                currentPosition.x += slotWidth;

                if ((i + 1) % maxSlotInLine == 0)
                {
                    currentPosition.x = startPosition.x;
                    currentPosition.y -= slotHeight;
                }
            }
        }

        public void AddToUISlot(int slotIndex, Item item, int count)
        {
            _inventorySlotsUI[slotIndex]._image.sprite = item.Sprite;
            if (count > 1)
                _inventorySlotsUI[slotIndex]._textCount.text = count.ToString();
        }

        private void ShowDeleteButton(InventorySlotUI slot)
        {
            if (!_deleteButton.gameObject.activeSelf || _slotToDelete != slot)
            {
                _slotToDelete = slot;
                _deleteButtonRect.position = slot._rectTransform.position - new Vector3(0, _deleteButtonOffset);
                _deleteButton.gameObject.SetActive(true);
            }
            else
            {
                _slotToDelete = null;
                _deleteButton.gameObject.SetActive(false);
            }
        }

        public void ShowInventoryWindow()
        {
            if (!this.gameObject.activeSelf)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }

        public void UpdateUISlot(int slotIndex , Item item, int count)
        {
            if (count == 0)
            {
                ClearSlot(slotIndex);
                return;
            }

            _inventorySlotsUI[slotIndex]._image.sprite = item.Sprite;
            _inventorySlotsUI[slotIndex]._textCount.text = count.ToString();
        }

        public void ClearSlot(int slotIndex)
        {
            _inventorySlotsUI[slotIndex]._image.sprite = null;
            _inventorySlotsUI[slotIndex]._textCount.text = string.Empty;
        }

        public void DeleteSlots()
        {
            foreach (var slot in _inventorySlotsUI)
            {
                Destroy(slot.gameObject);
            }
            _inventorySlotsUI.Clear();
        }

        public void HandleClearSlot()
        {
            _slotToDelete._image.sprite = null;
            _slotToDelete._textCount.text = string.Empty;
            EventManager.onClearInventorySlot?.Invoke(_inventorySlotsUI.IndexOf(_slotToDelete));
        }

    }
}