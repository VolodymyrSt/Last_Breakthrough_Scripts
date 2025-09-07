using UnityEngine;

namespace LastBreakthrought.Infrustructure.AssetManagment
{
    public interface IAssetProvider
    {
        T Instantiate<T>(string path);
        T Instantiate<T>(string path, Vector3 at, Transform parent);
        T Instantiate<T>(string path, Vector3 at, RectTransform parent);

        GameObject Instantiate(string path);
        GameObject Instantiate(string path, Vector3 at, Transform parent);
    }
}