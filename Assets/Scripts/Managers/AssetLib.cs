using UnityEngine;

public class AssetLib : MonoBehaviour, IProvidable
{
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private GameObject GreenColoredItemPrefab, BlueColoredItemPrefab, RedColoredItemPrefab,YellowColoredItemPrefab;
    private Transform _gridRoot;
    private Transform _itemRoot;

    private void Awake()
    {
        ServiceProvider.Register(this);

        _gridRoot = new GameObject("GridRoot").transform;
        _itemRoot = new GameObject("ItemRoot").transform;

        _gridRoot.parent = transform;
        _itemRoot.parent = transform;
    }

    public T GetAsset<T>(AssetType assetType, string objectName) where T : MonoBehaviour
    {
        var asset = GetAsset<T>(assetType);
        if (asset == null)
        {
            return null;
        }

        asset.name = objectName;
        return asset;
    }

    public T GetAsset<T>(AssetType assetType) where T : class
    {
        var asset = GetAsset(assetType);
        return asset == null ? null : asset.GetComponent<T>();
    }

    private GameObject GetAsset(AssetType assetType)
    {
        return assetType switch
        {
            AssetType.Tile => Instantiate(TilePrefab, _gridRoot),
            AssetType.GreenColoredItem => Instantiate(GreenColoredItemPrefab, _itemRoot),
            AssetType.BlueColoredItem => Instantiate(BlueColoredItemPrefab, _itemRoot),
            AssetType.RedColoredItem => Instantiate(RedColoredItemPrefab, _itemRoot),
            AssetType.YellowColoredItem => Instantiate(YellowColoredItemPrefab, _itemRoot),
            _ => null
        };
    }
}