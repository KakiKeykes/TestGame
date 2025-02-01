using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class ItemObject : MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private int _count = 1;
        [SerializeField] private int _randomSpread = 1;

        public int Count { get { return _count; } set { _count = value; } }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var result = EventManager.onItemHit?.Invoke(collision, _item, _count);

            if (result != null && !result.Value.Success)
            {
                _count = result.Value.RemainingCount;
            }
            else if (result != null && result.Value.Success)
                Destroy(gameObject);
        }

        public bool IsSingle()
        {
            return _item.MaxStackCount == 1;
        }

        public void GenerateItemCount()
        {
            _count = Mathf.Max(1, Random.Range(_count - _randomSpread, _count + _randomSpread));
        }
    }
}