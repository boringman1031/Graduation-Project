using UnityEngine;
using UnityEngine.AddressableAssets;

public class InitalLoad : MonoBehaviour
{
    public AssetReference presistentScene;

    private void Start()
    {
        Addressables.LoadSceneAsync(presistentScene);

    }
}
