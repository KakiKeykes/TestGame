using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        void Update()
        {
            if (_playerTransform == null) return;

            var camPos = new Vector3(_playerTransform.position.x, _playerTransform.position.y, this.transform.position.z);
            this.transform.position = camPos;
        }
    }
}