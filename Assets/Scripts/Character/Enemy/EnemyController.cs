using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Character.Enemy
{
    public class EnemyController : CharacterController
    {
        [SerializeField] private List<ItemObject> _dropItems = new List<ItemObject>();
        [SerializeField] private Weapon _weapon;
        [SerializeField] private Rigidbody2D _rigidbody;

        private void OnEnable()
        {
            EventManager.onBulletHit += HandleBulletHit;
        }

        private void OnDisable()
        {
            EventManager.onBulletHit -= HandleBulletHit;
        }

        private void Update()
        {
            if (Target != null)
            {
                Vector2 direction = (Target.transform.position - transform.position).normalized;
                _rigidbody.velocity = direction * MoveSpeed;
                var distance = (this.transform.position - Target.transform.position).magnitude;

                if (distance < AttackDistance && Time.time >= NextFireTime)
                {
                    _weapon.Shoot(Target.transform.position, Damage);
                    NextFireTime = Time.time + ReloadTime;   
                }
            }
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

        internal override void Die()
        {
            Debug.Log("Die");
            EventManager.onEnemyDie?.Invoke(this);
            DropItem();
            Destroy(_hpBar.gameObject);
            Destroy(this.gameObject);
        }

        private void DropItem()
        {
            var randomIndex = Random.Range(0, _dropItems.Count);

            var item = Instantiate(_dropItems[randomIndex]);
            item.transform.position = this.transform.position;

            if(!item.IsSingle())
            {
                item.GenerateItemCount();
            }
        }
    }
}