using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _lifeTime;
        [SerializeField] private float _speed;
        private int _damage;

        public int Damage { set { _damage = value; } }
        

        void Awake()
        {
            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector2.down * _speed *  Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == this.tag) return;
             
            EventManager.onBulletHit?.Invoke(collision, _damage);
            Destroy(gameObject);
        }
    }
}