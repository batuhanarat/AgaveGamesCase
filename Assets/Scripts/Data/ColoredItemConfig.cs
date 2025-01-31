using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AgaveGamesCase/ColoredItemConfig")]
public class ColoredItemConfig : ScriptableObject
{
    [SerializeField] private ColorConfig[] colorConfigs;
    private Dictionary<ItemColor, Sprite> _spriteMap;

    [System.Serializable]
    private class ColorConfig
    {
        public ItemColor color;
        public Sprite sprite;
    }

    private void OnEnable()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        _spriteMap = new Dictionary<ItemColor, Sprite>();
        foreach (var config in colorConfigs)
        {
            _spriteMap.Add(config.color, config.sprite);
        }
    }

    public Sprite GetSpriteForColor(ItemColor color)
    {
        return _spriteMap.TryGetValue(color, out var sprite) ? sprite : null;
    }

}