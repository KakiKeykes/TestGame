using Assets.Scripts.GamePlay;
using Assets.Scripts.Save;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Character.Player
{
    public class PlayerController : CharacterController
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private Item _startItem;
        [SerializeField] private int _startItemCount;

        private void OnEnable()
        {
            EventManager.onItemHit += HandleItemHit;
            EventManager.onBulletHit += HandleBulletHit;
        }

        private void OnDisable()
        {
            EventManager.onItemHit -= HandleItemHit;
            EventManager.onBulletHit -= HandleBulletHit;
        }

        private void Start()
        {
            _inventory.TryAddItem(_startItem, _startItemCount);
        }

        void Update()
        {
            if (Target != null)
                CheckTargetDistance();
        }

        private void CheckTargetDistance()
        {
            var distance = (this.transform.position - Target.transform.position).magnitude;
            if (distance > TargetDistance)
                Target = null;
        }

        public void ShootHandle()
        {
            if (_weapon == null || !_inventory.HasItem(_weapon.Ammo)) return;

            _inventory.TryRemoveItem(_weapon.Ammo, 1);

            if (Target == null)
                _weapon.Shoot(Vector3.zero, Damage);
            else
                _weapon.Shoot(Target.transform.position, Damage);
        }

        private (bool Success, Item RemainingItem, int RemainingCount) HandleItemHit(Collider2D collision, Item item, int count)
        {
            if (collision.gameObject == gameObject)
            {
                return _inventory.TryAddItem(item, count);
            }
            return (false, item, count);
        }

        private void HandleBulletHit(Collider2D collision, int damage)
        {
            if (collision.gameObject == gameObject)
            {
                CurrentHealth -= damage;
                UpdateHPBar(CurrentHealth);
                if (CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }

        public override void LoadData(SaveData data)
        {
            if (data.Data.TryGetValue("CurrentPlayerHealth", out var currentHealth))
            {
                CurrentHealth = (float)(double)currentHealth;
                UpdateHPBar(CurrentHealth);
            }
            if (data.Data.TryGetValue("CurrentPosition", out var currentPosition))
            {
                var jArray = (JArray)currentPosition;

                var positionArray = jArray.Select(t => (float)t).ToArray();

                transform.position = new Vector3(positionArray[0], positionArray[1], positionArray[2]);
            }
            if (data.Data.TryGetValue("CurrentRotation", out var currentRotation))
            {
                var jArray = (JArray)currentRotation;

                var rotationArray = jArray.Select(t => (float)t).ToArray();

                transform.rotation = new Quaternion(rotationArray[0], rotationArray[1], rotationArray[2], rotationArray[3]);
            }
            _inventory.LoadData(data);
        }

        public override void SaveData(SaveData data)
        {
            data.Data["CurrentPlayerHealth"] = CurrentHealth;
            data.Data["CurrentPosition"] = new float[] { transform.position.x, transform.position.y, transform.position.z };
            data.Data["CurrentRotation"] = new float[] { transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w };

            _inventory.SaveData(data);
        }
    }
}