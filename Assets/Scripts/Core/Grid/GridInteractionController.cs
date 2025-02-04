using UnityEngine;

public class GridInteractionController : MonoBehaviour
{
    private Tile _currentSelectedTile;
    private readonly Link link = new();
    private GridRenderer gridRenderer;

    void Awake()
    {
        gridRenderer = GetComponent<GridRenderer>();
    }

    public void OnMouseDown()
    {
        if(!ServiceProvider.MoveManager.CanMakeMove) return;

        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!gridRenderer.TryGetTileFromPosition(clickPosition, out Tile tile) ||
            !tile.TryGetColoredItem(out ColoredItem coloredItem) ) return;

        _currentSelectedTile = tile;
        link.Initialize(coloredItem);
    }

    public void OnMouseDrag()
    {
        if(!ServiceProvider.MoveManager.CanMakeMove) return;

        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(!gridRenderer.TryGetTileFromPosition(clickPosition, out Tile tile) ||
            tile == _currentSelectedTile ||
            !tile.TryGetColoredItem(out ColoredItem coloredItem)) return;

        _currentSelectedTile = tile;
        link.TryAdd(coloredItem);
    }

    public void OnMouseUp()
    {
        if(!ServiceProvider.MoveManager.CanMakeMove) return;

        _currentSelectedTile = null;
        bool isExploded = link.TryExplodeLink();
        link.Reset();

        if(isExploded)
        {
            ServiceProvider.FallManager.StartFall();
        }
    }


}