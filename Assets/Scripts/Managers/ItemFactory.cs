using UnityEngine;
using UnityEngine.Pool;

public interface IItemFactory
{
    void Initialize(GameConfig gameConfig);
    ColoredItem GetRandomColoredItem();
    ColoredItem GetItemWithColor(ItemColor itemColor);
    void RecycleItem(ColoredItem item);
}

public class ItemFactory : IProvidable , IItemFactory
{
    private IObjectPool<ColoredItem> itemPool;
    private ColoredItemConfig coloredItemConfig;


    public ItemFactory()
    {
        ServiceProvider.Register(this);
        coloredItemConfig = Resources.Load<ColoredItemConfig>("ScriptableObjects/ColoredItemConfigSO"); // move to assetlib or serviceprovider
    }

    //Refactor random algorithm
    public ColoredItem GetRandomColoredItem()
    {
        ItemColor[] itemColors = {ItemColor.Red, ItemColor.Green, ItemColor.Blue, ItemColor.Yellow};
        var randomIndex = UnityEngine.Random.Range(0,4);
        ItemColor randomlySelectedColor = itemColors[randomIndex];

        return GetItemWithColor(randomlySelectedColor);
    }

    public ColoredItem GetItemWithColor(ItemColor itemColor)
    {
        ColoredItem item = itemPool.Get();
        item.Configure(itemColor, coloredItemConfig.GetSpriteForColor(itemColor));
        return item;
    }

    public void RecycleItem(ColoredItem item)
    {
        ServiceProvider.GameGrid.Tiles[item.Index.x, item.Index.y].RemoveItem();
        item.Reset();
        itemPool.Release(item);
    }

    private void PrePopulatePool(IObjectPool<ColoredItem> pool, int defaultCapacity)
    {
        for(int i = 0 ; i < defaultCapacity ; i++) {
            var item = pool.Get();
            pool.Release(item);
        }
    }

    public void Initialize(GameConfig gameConfig)
    {
        var defaultCapacity = gameConfig.GridRow * gameConfig.GridColumn;
        var maxPoolSize = defaultCapacity * 2;
        InitializePool(defaultCapacity, maxPoolSize);
    }

    #region Pooling

    private bool _collectionCheck = true;

    private IObjectPool<ColoredItem> InitializePool(int defaultCapacity, int maxPoolSize)
    {
            itemPool = new ObjectPool<ColoredItem>(
                CreateNewColoredItem,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                _collectionCheck,
                defaultCapacity,
                maxPoolSize);

            PrePopulatePool(itemPool, defaultCapacity);

            return itemPool;
    }

    private ColoredItem CreateNewColoredItem()
    {
        ColoredItem item = ServiceProvider.AssetLib.GetAsset<ColoredItem>(AssetType.ColoredItem);
        item.gameObject.SetActive(false);
        return item;
    }

    private void OnTakeFromPool(ColoredItem item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(ColoredItem item)
    {
        item.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(ColoredItem item)
    {
        Object.Destroy(item.gameObject);
    }

    #endregion


}