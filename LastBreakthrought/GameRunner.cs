using LastBreakthrought.Infrustructure;
using UnityEngine;
using Zenject;

namespace LastBreakthrought
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootstrapperPrefab;
        private IInstantiator _instantiator;

        [Inject]
        private void Construct(IInstantiator instantiator) 
            => _instantiator = instantiator;

        private void Awake() => InitializeBootstrapper();

        private void InitializeBootstrapper()
        {
            var bootstrapper = FindFirstObjectByType<GameBootstrapper>();

            if (bootstrapper == null)
                _instantiator.InstantiatePrefab(_bootstrapperPrefab);
        }
    }
}
