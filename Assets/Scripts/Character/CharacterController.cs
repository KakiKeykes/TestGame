using Assets.Scripts.Save;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class CharacterController : MonoBehaviour, ISaveable
    {
        [SerializeField] private float _currentHealth = 10f;
        [SerializeField] private float _maxHealth = 10f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _attackDistance = 3f;
        [SerializeField] private float _targetDistance = 5f;
        [SerializeField] private float _reloadTime = 1f;
        [SerializeField] private float _nextFireTime;
        [SerializeField] private CharacterController _target;
        [SerializeField] internal HPBar _hpBar;

        public float CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }
        public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
        public int Damage { get { return _damage; } }
        public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
        public float AttackDistance { get { return _attackDistance; } }
        public float TargetDistance { get { return _targetDistance; } }
        public float ReloadTime { get { return _reloadTime; } }
        public float NextFireTime { get { return _nextFireTime; } set { _nextFireTime = value; } }
        public CharacterController Target { get { return _target; } set { _target = value; } }
        public HPBar HPBar { get { return _hpBar; } set { _hpBar = value; } }


        private void OnDestroy()
        {
            Destroy(_hpBar.gameObject);
        }

        internal void InitializeHPBar()
        {
            _hpBar.InitializeHPBar(this.transform, _maxHealth, _currentHealth);
        }

        internal void UpdateHPBar(float currentHP)
        {
            _hpBar.UpdateHPBar(currentHP);
        }

        internal virtual void Die()
        {
            Destroy(this.gameObject);
        }

        public virtual void SaveData(SaveData data)
        {
            data.Data["CurrentHealth"] = _currentHealth;
        }

        public virtual void LoadData(SaveData data)
        {
            if (data.Data.TryGetValue("CurrentHealth", out var health))
            {
                _currentHealth = (float)health;
            }
        }
    }
}