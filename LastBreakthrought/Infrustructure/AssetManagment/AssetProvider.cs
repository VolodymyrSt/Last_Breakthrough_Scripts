using UnityEngine;
using Zenject;

namespace LastBreakthrought.Infrustructure.AssetManagment
{
    public class AssetProvider : IAssetProvider
    {
        private readonly IInstantiator _instantiator;
        public AssetProvider(IInstantiator instantiator) =>
            _instantiator = instantiator;

        public T Instantiate<T>(string path) =>
            _instantiator.InstantiatePrefabResourceForComponent<T>(path);
        public T Instantiate<T>(string path, Vector3 at, Transform parent) =>
            _instantiator.InstantiatePrefabResourceForComponent<T>(path, at, Quaternion.identity, parent);
        public T Instantiate<T>(string path, Vector3 at, RectTransform parent) =>
            _instantiator.InstantiatePrefabResourceForComponent<T>(path, at, Quaternion.identity, parent);

        public GameObject Instantiate(string path) =>
            _instantiator.InstantiatePrefabResource(path);
        public GameObject Instantiate(string path, Vector3 at, Transform parent) =>
            _instantiator.InstantiatePrefabResource(path, at, Quaternion.identity, parent);
    }
}
