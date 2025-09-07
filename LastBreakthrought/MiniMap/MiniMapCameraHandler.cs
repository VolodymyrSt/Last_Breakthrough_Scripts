using DG.Tweening;
using LastBreakthrought.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace LastBreakthrought.MiniMap
{
    public class MiniMapCameraHandler : MonoBehaviour
    {
        [SerializeField] private Image _playerMark;

        private PlayerHandler _playerHandler;

        [Inject]
        private void Construct(PlayerHandler playerHandler) =>
            _playerHandler = playerHandler;

        private void LateUpdate()
        {
            transform.position = new Vector3(_playerHandler.GetPosition().x, 40f, _playerHandler.GetPosition().z);
            Vector3 playerEulerAngles = _playerHandler.transform.eulerAngles;
            _playerMark.transform.rotation = Quaternion.Euler(-90, -135 + playerEulerAngles.y, 0);
        }
    }
}
