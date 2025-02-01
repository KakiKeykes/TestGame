using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Save
{
    [CreateAssetMenu(fileName = "NewItemStorage", menuName = "Custom/ItemStorage")]
    public class ItemStorage : ScriptableObject
    {
        [SerializeField] List<Item> _items = new List<Item>();

        public Item GetItemByID(int id)
        {
            return _items[id];
        }
    }
}