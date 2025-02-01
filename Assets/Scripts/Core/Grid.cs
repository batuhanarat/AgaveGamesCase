using UnityEngine;

public class Grid : MonoBehaviour, IProvidable
{
    [SerializeField] private SpriteRenderer gridRenderer;
    [SerializeField] public Tile[,] _grid;
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
        _rows = rows;
        _columns = columns;

        float availableScreenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2 * 0.9f;
        float availableScreenHeight = Camera.main.orthographicSize * 2 * 0.65f;
        float totalPaddingX = padding * (_rows - 1);
        float totalPaddingY = padding * (_columns - 1);

        float availableWidth = availableScreenWidth - totalPaddingX;
        float availableHeight = availableScreenHeight - totalPaddingY;


        CellSize = Mathf.Min(availableWidth / rows, availableHeight / columns);

        float boardWidth = CellSize * rows + totalPaddingX + 2 * _gridWidthOffset;
        float boardHeight = CellSize * columns + totalPaddingY + 2 * _gridHeightOffset;

        AdjustBoardSprite(boardWidth, boardHeight);
        _grid = new Tile[rows, columns];

        InitializeBoardWithCells(CellSize);


        void InitializeBoardWithCells(float cellSize) {
            Vector2Int cellIndex = new Vector2Int();

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < columns; j++) {

                    float localXPosition = (-boardWidth / 2) + _gridWidthOffset + (j * (cellSize + padding)) + (cellSize / 2);
                    float localYPosition = (-boardHeight / 2) + _gridHeightOffset + (i * (cellSize + padding)) + (cellSize / 2);

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

                    _grid[j, i] = tile;
                }
            }
        }
    }
    private void AdjustBoardSprite(float boardWidth, float boardHeight)
    {
        if (gridRenderer != null) {
            gridRenderer.size = new Vector2(boardWidth, boardHeight);
        } else {
            Debug.LogWarning("Board sprite renderer is missing!");
        }
    }

    # endregion


    public void AddToGrid(ItemBase item, Vector2Int coord)
    {
        Tile tile = _grid[coord.x,coord.y];
        tile.SetItem(item);
        return;
    }

    public void OnMouseDown()
    {
       // Debug.Log("Input started");

        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!TryGetTileFromPosition(clickPosition, out Tile tile)) return;
        if(!tile.TryGetColoredItem(out ColoredItem coloredItem)) return;
        _currentSelectedTile = tile;
        link.Initialize(coloredItem);
    }

    public void OnMouseDrag()
    {
        Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(!TryGetTileFromPosition(clickPosition, out Tile tile)) return;
        if(tile == _currentSelectedTile) return;
        if(!tile.TryGetColoredItem(out ColoredItem coloredItem)) return;
        _currentSelectedTile = tile;
        link.TryAdd(coloredItem);
       // Debug.Log("Input is taken right now");
    }

    public void OnMouseUp()
    {
        _currentSelectedTile = null;
        link.TryExplodeLink();
        link.Reset();

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

        tile = _grid[col,row];
        return true;
    }
}