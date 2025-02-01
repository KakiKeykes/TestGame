using Assets.Scripts.GamePlay;
using Assets.Scripts.Save;
using Assets.Scripts.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Character.Player
{
    public class Inventory : MonoBehaviour, ISaveable
    {
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private int _slotCount = 6;
        [SerializeField] private List<InventorySlot> _items = new();

        private void Awake()
        {
            EventManager.onClearInventorySlot += ClearSlot;

            for (int i = 0; i < _slotCount; i++)
                _items.Add(new InventorySlot());

            _inventoryUI.CreateSlots(_slotCount);
        }

        private void OnDisable()
        {
            EventManager.onClearInventorySlot -= ClearSlot;
        }

        public (bool Success, Item RemainingItem, int RemainingCount) TryAddItem(Item newItem, int newItemCount)
        {
            foreach (var slot in _items)
            {
                if (slot.Item != null && slot.Item.ItemID == newItem.ItemID)
                {
                    if (slot.Count < slot.Item.MaxStackCount)
                    {
                        int availableSpace = slot.Item.MaxStackCount - slot.Count;
                        int amountToAdd = Math.Min(availableSpace, newItemCount);

                        slot.Count += amountToAdd;
                        newItemCount -= amountToAdd;

                        if (newItemCount == 0)
                        {
                            _inventoryUI.AddToUISlot(_items.IndexOf(slot), slot.Item, slot.Count);
                            return (true, null, 0);
                        }
                    }
                }
            }

            foreach (var slot in _items)
            {
                if (slot.Item == null)
                {
                    slot.Item = newItem;
                    slot.Count = Math.Min(newItemCount, newItem.MaxStackCount);
                    newItemCount -= slot.Count;

                    _inventoryUI.AddToUISlot(_items.IndexOf(slot), slot.Item, slot.Count);

                    if (newItemCount == 0)
                    {
                        return (true, null, 0);
                    }
                }
            }

            return (false, newItem, newItemCount);
        }

        private void ClearSlot(int slotIndex)
        {
            _items[slotIndex].Item = null;
            _items[slotIndex].Count = 0;
        }

        public bool HasItem(Item item)
        {
            foreach (var slot in _items)
            {
                if (slot.Item == item) return true;
            }
            return false;
        }

        public bool TryRemoveItem(Item item, int count)
        {
            foreach (var slot in _items)
            {
                if (slot.Item != null && slot.Item.ItemID == item.ItemID)
                {
                    int amountToRemove = Math.Min(slot.Count, count);
                    slot.Count -= amountToRemove;
                    count -= amountToRemove;

                    _inventoryUI.UpdateUISlot(_items.IndexOf(slot), slot.Item, slot.Count);

                    if (slot.Count == 0)
                    {
                        slot.Item = null;
                    }

                    if (count == 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void SaveData(SaveData data)
        {
            data.Data["SlotCount"] = _items.Count;

            for (int i = 0; i < _items.Count; i++)
            {
                var slot = _items[i];
                data.Data[$"Slot_{i}_ItemID"] = slot.Item?.ItemID.ToString();
                data.Data[$"Slot_{i}_Count"] = slot.Count;
            }
        }

        public void LoadData(SaveData data)
        {
            if (data.Data.TryGetValue("SlotCount", out var slotCount))
            {
                _slotCount = Convert.ToInt32(slotCount);
            }

            _items.Clear();

            for (int i = 0; i < _slotCount; i++)
            {
                var slot = new InventorySlot();

                if (data.Data.TryGetValue($"Slot_{i}_ItemID", out var itemID) && itemID != null)
                {
                    slot.Item = FindItemByID(Convert.ToInt32(itemID));
                }

                if (data.Data.TryGetValue($"Slot_{i}_Count", out var count))
                {
                    slot.Count = Convert.ToInt32(count);
                }

                _items.Add(slot);
            }

            _inventoryUI.DeleteSlots();

            _inventoryUI.CreateSlots(_slotCount);

            for (int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Item != null)
                {
                    _inventoryUI.AddToUISlot(i, _items[i].Item, _items[i].Count);
                }
            }
        }

        private Item FindItemByID(int itemID)
        {
            var itemStorage = Resources.Load<ItemStorage>($"Items/ItemStorage");

            return itemStorage.GetItemByID(itemID);
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _count;

        public Item Item { get { return _item; } set { _item = value; } }
        public int Count { get { return _count; } set { _count = value; } }
    }

    [System.Serializable]
    public class InventorySlotData
    {
        public string ItemID;
        public int Count;
    }
}