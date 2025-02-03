using DG.Tweening;
using UnityEngine;

public class ColoredItem : ItemBase
{
    public ItemColor Color { get; private set; }
    private float _scale;

    public void Configure(ItemColor color, Sprite sprite)
    {
        Color = color;
        spriteRenderer.sprite = sprite;
    }

    public override void TryExplode()
    {
        ServiceProvider.ItemFactory.ReturnToPool(this);
    }

    public void HighlightForLink()
    {
        ServiceProvider.GameGrid.GetTileFromIndex(Index).MockHighlight();
        _scale = transform.lossyScale.x;
        transform.DOScale(_scale * 1.08f, 0.2f).SetEase(Ease.InSine);

    }

    public void RemoveHighlightForLink()
    {
        ServiceProvider.GameGrid.GetTileFromIndex(Index).MockUnhighlight();

        transform.DOScale(_scale, 0.2f).SetEase(Ease.InSine);

    }
}