using UnityEngine;

namespace Assets.Scripts.Character.Player
{
    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] private VariableJoystick variableJoystick;
        [SerializeField] private Rigidbody2D _playerRigidBody;
        [SerializeField] private PlayerController _playerController;
        public void Update()
        {
            Vector3 direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
            _playerRigidBody.velocity = direction * _playerController.MoveSpeed;
        }
    }
}