using UnityEngine;
using UnityEngine.Pool;

public class ItemFactory : IProvidable
{
    private IObjectPool<ColoredItem> itemPool;
    private ColoredItemConfig coloredItemConfig;
    private bool _collectionCheck = true;

    public ItemFactory()
    {
        ServiceProvider.Register(this);
        coloredItemConfig = Resources.Load<ColoredItemConfig>("ScriptableObjects/ColoredItemConfigSO"); // move to assetlib or serviceprovider

        //Maybe move to somewhere else

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

    public void ReturnToPool(ColoredItem item)
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

    #region Pooling
    public IObjectPool<ColoredItem> InitializePool(int defaultCapacity, int maxPoolSize)
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