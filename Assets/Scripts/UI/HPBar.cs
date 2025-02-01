using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HPBar : MonoBehaviour
    {
        private Camera _camera;
        [SerializeField] private Slider _slider;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _offset;
        [SerializeField] private Transform _followingTransform;

        public void InitializeHPBar(Transform transform, float maxHP, float currentHP)
        {
            _camera = Camera.main;
            _followingTransform = transform;
            _slider.maxValue = maxHP;
            _slider.value = currentHP;
        }

        public void UpdateHPBar(float currentHP)
        {
            _slider.value = currentHP;
        }

        private void Update()
        {
            if (_followingTransform != null)
            {
                var screenPos = _camera.WorldToScreenPoint(_followingTransform.position);

                screenPos.y += _offset;

                _rectTransform.position = screenPos;
            }
        }
    }
}