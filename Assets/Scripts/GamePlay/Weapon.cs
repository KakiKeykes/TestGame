using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;
        [SerializeField] private Transform _bulletSpawnPosition;
        [SerializeField] private Item _ammo;

        public Item Ammo {  get { return _ammo; } set {  _ammo = value; } }

        public void Shoot(Vector3 targetPosition, int damage)
        {
            var direction = (targetPosition - this.transform.position);
            var rotationZ = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            var currentBullet = Instantiate(_bullet, _bulletSpawnPosition.position, Quaternion.identity);
            currentBullet.Damage = damage;
            currentBullet.transform.rotation = Quaternion.Euler(0,0, rotationZ + 90);
            currentBullet.tag = this.tag;
        }
    }
}