using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Custom/Item")]
    public class Item : ScriptableObject
    {
        [SerializeField] private int _itemID;
        [SerializeField] private int _maxStackCount;
        [SerializeField] private Sprite _sprite;

        public Sprite Sprite { get { return _sprite; } }
        public int ItemID { get { return _itemID; } }
        public int MaxStackCount { get { return _maxStackCount; } }
    }
}