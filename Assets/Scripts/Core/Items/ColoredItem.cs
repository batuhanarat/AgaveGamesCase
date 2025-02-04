using DG.Tweening;
using UnityEngine;

public class ColoredItem : ItemBase
{
    public ItemColor Color { get; private set; }
    private Vector3 _originalScale;
    private bool _isHighlighted = false;
    private Tween _currentScaleTween;
    private const float HIGHLIGHTED_SCALE_MULTIPLIER = 1.2f;
    private float _highlightDuration = 0.1f;

    public void Configure(ItemColor color, Sprite sprite)
    {
        Color = color;
        spriteRenderer.sprite = sprite;
    }

    public override void TryExplode()
    {
        var tile = ServiceProvider.GameGrid.GetTileFromIndex(Index);
        tile.PlayParticleEffect(spriteRenderer.sprite);
        ServiceProvider.ItemFactory.RecycleItem(this);
    }

    public void HighlightAsLinked()
    {
        if (_isHighlighted) return;
        ServiceProvider.GameGrid.GetTileFromIndex(Index).Highlight();
        _isHighlighted = true;
    }

    public void RemoveHighlightAsLinked()
    {
        if (!_isHighlighted) return;
        ServiceProvider.GameGrid.GetTileFromIndex(Index).Unhighlight();
        _isHighlighted = false;
    }

    public void HighlightAsLastInLink()
    {
        _currentScaleTween?.Kill();
        _currentScaleTween = transform.DOScale(scaleAfterPlacedInTile * HIGHLIGHTED_SCALE_MULTIPLIER, _highlightDuration)
            .SetEase(Ease.OutBack);
    }

    public void RemoveHighlightAsLastInLink()
    {
        _currentScaleTween?.Kill();
        _currentScaleTween = transform.DOScale(scaleAfterPlacedInTile, _highlightDuration)
            .SetEase(Ease.InBack);
    }

}