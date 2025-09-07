using UnityEngine;
using UnityEngine.UI;

namespace LastBreakthrought.Infrustructure.UI
{
    public abstract class MainMenuButton : MonoBehaviour 
    {
        [SerializeField] protected Button Button;

        private void Awake() => OnAwake();
        public abstract void OnAwake();
    }
}
