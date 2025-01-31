using UnityEngine;

public class AssetLib : MonoBehaviour, IProvidable
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject TilePrefab;
    [SerializeField] private GameObject GreenColoredItemPrefab, BlueColoredItemPrefab, RedColoredItemPrefab, YellowColoredItemPrefab;
    [SerializeField] private GameObject SuccessScreenPrefab, FailScreenPrefab;
    [SerializeField] private GameObject ScorePanel, MovePanel;

    private Transform _gridRoot;
    private Transform _itemRoot;
    private Transform _uiRoot;


    private void Awake()
    {
        ServiceProvider.Register(this);

        _gridRoot = new GameObject("GridRoot").transform;
        _itemRoot = new GameObject("ItemRoot").transform;

        _gridRoot.parent = transform;
        _itemRoot.parent = transform;
        _uiRoot = canvas.transform;
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
            AssetType.FailScreen => Instantiate(FailScreenPrefab,_uiRoot),
            AssetType.SuccessScreen => Instantiate(SuccessScreenPrefab,_uiRoot),
            AssetType.MovePanel => MovePanel,
            AssetType.ScorePanel => ScorePanel,
            _ => null
        };
    }
}