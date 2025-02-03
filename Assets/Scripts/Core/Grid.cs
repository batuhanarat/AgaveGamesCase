using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour, IProvidable
{
    [SerializeField] private SpriteRenderer gridRenderer;
    [SerializeField] private BoxCollider2D  gridCollider;
    public Tile[,] Tiles;
    private Tile _currentSelectedTile;
    private int _rows;
    private int _columns;
    private float padding  = 0.01f;
    private float _gridHeightOffset = 0.15f;
    private float _gridWidthOffset = 0.15f;

    public float GridSize;

    private readonly Link link = new();

    public float CellSize { get; private set; }

    private void Awake()
    {
        ServiceProvider.Register(this);
    }
    #region  Visualization of grid
    public void Initialize(int rows, int columns)
    {

        //columns  -> width
        //rows -> height
        _rows = rows;
        _columns = columns;

        float availableScreenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2 * 0.9f;
        float availableScreenHeight = Camera.main.orthographicSize * 2 * 0.65f;
        float totalPaddingX = padding * (_columns - 1);
        float totalPaddingY = padding * (_rows - 1);

        float availableWidth = availableScreenWidth - totalPaddingX;
        float availableHeight = availableScreenHeight - totalPaddingY;


        CellSize = Mathf.Min(availableWidth / rows, availableHeight / columns);

        float gridHeight = CellSize * rows + totalPaddingX + 2 * _gridWidthOffset;
        float gridWidth = CellSize * columns + totalPaddingY + 2 * _gridHeightOffset;

        AdjustBoardSprite(gridWidth, gridHeight);
        Tiles = new Tile[columns, rows];

        InitializeBoardWithCells(CellSize);


        void InitializeBoardWithCells(float cellSize) {
            Vector2Int cellIndex = new Vector2Int();

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {

                    float localXPosition = (-gridWidth / 2) + _gridWidthOffset + (j * (cellSize + padding)) + (cellSize / 2);
                    float localYPosition = (-gridHeight / 2) + _gridHeightOffset + (i * (cellSize + padding)) + (cellSize / 2);

                    Vector3 worldPosition = transform.TransformPoint(new Vector3(localXPosition, localYPosition, 0));
                    Tile tile = ServiceProvider.AssetLib.GetAsset<Tile>(AssetType.Tile);
                    tile.transform.position = worldPosition;

                    if (tile == null) {
                        Debug.LogWarning("Cell script is null");
                    }

                    tile.transform.SetParent(transform, true);

                    var spriteRenderer = tile.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null) {
                        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
                        float scale = cellSize / Mathf.Min(spriteSize.x, spriteSize.y);
                        tile.transform.localScale = new Vector3(scale, scale, 1);

                    }

                    cellIndex.x = j;
                    cellIndex.y = i;
                    tile.SetCoord(cellIndex);

                    Tiles[j, i] = tile;
                }
            }
        }
    }
    private void AdjustBoardSprite(float boardWidth, float boardHeight)
    {
        if (gridRenderer != null) {
            gridRenderer.size = new Vector2(boardWidth, boardHeight);
            gridCollider.size = new Vector2(boardWidth, boardHeight);
        } else {
            Debug.LogWarning("Board sprite renderer is missing!");
        }
    }

    # endregion


    public void AddToGrid(ItemBase item, Vector2Int coord)
    {
        Tile tile = Tiles[coord.x,coord.y];
        tile.SetItem(item,true);
        return;
    }

    public void OnMouseDown()
    {
        if(!ServiceProvider.MoveManager.CanMakeMove) return;
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!TryGetTileFromPosition(clickPosition, out Tile tile) || !tile.TryGetColoredItem(out ColoredItem coloredItem) ) return;
        _currentSelectedTile = tile;
        link.Initialize(coloredItem);
    }

    public void OnMouseDrag()
    {
        if(!ServiceProvider.MoveManager.CanMakeMove) return;

        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!TryGetTileFromPosition(clickPosition, out Tile tile) || tile == _currentSelectedTile || !tile.TryGetColoredItem(out ColoredItem coloredItem)) return;
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
    public bool CheckTilesAreAdjacent(Tile tile1, Tile tile2)
    {
        var xDiff = Mathf.Abs(tile1._coord.x - tile2._coord.x);
        var yDiff = Mathf.Abs(tile1._coord.y - tile2._coord.y);
        var manhattanDistance = xDiff + yDiff;
        return manhattanDistance == 1;
    }
    public bool TryGetRightTile(int column, int row, out Tile rightTile)
    {
        if(column + 1 >= _columns) {
            rightTile = default;
            return false;
        }
        rightTile = Tiles[column+1,row];

        return rightTile.HasItem;
    }
    public bool TryGetLeftTile(int column, int row, out Tile leftTile)
    {
        if(column - 1 < 0) {
            leftTile = default;
            return false;
        }
        leftTile = Tiles[column-1,row];

        return leftTile.HasItem;
    }
    public bool TryGetUpperTile(int column, int row, out Tile upperTile)
    {
        if(row + 1 >= _rows) {
            upperTile = default;
            return false;
        }
        upperTile = Tiles[column,row+1];

        return upperTile.HasItem;
    }
    public bool TryGetBellowTile(int column, int row, out Tile belowTile)
    {
        if(row - 1 < 0) {
            belowTile = default;
            return false;
        }
        belowTile = Tiles[column,row-1];

        return belowTile.HasItem;
    }
    public Tile GetTileFromIndex(int column, int row)
    {
        return Tiles[column,row];
    }
    public Tile GetTileFromIndex(Vector2Int index)
    {
        return Tiles[index.x,index.y];
    }

    public List<Tile> GetNeighborsFromIndexesWithItem(int column, int row)
    {
        List<Tile> neighbors = new();

        if (TryGetUpperTile(column, row, out Tile upperTile)) neighbors.Add(upperTile);
        if (TryGetBellowTile(column, row, out Tile bellowTile)) neighbors.Add(bellowTile);
        if (TryGetRightTile(column, row, out Tile rightTile)) neighbors.Add(rightTile);
        if (TryGetLeftTile(column, row, out Tile leftTile)) neighbors.Add(leftTile);

        return neighbors;
    }

    public bool TryGetTileFromPosition(Vector3 worldPosition,out Tile tile)
    {
        Vector3 localPosition = worldPosition - transform.position;

        int col = Mathf.FloorToInt((localPosition.x + _columns / 2.0f * CellSize) / CellSize);
        int row = Mathf.FloorToInt((localPosition.y + _rows / 2.0f * CellSize) / CellSize);

        if (col < 0 || col >= _columns || row < 0 || row >= _rows)
        {
            tile = default;
            return false;
        }

        tile = Tiles[col,row];
        return true;
    }
}