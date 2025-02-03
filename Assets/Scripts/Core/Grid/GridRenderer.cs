using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public static GridRenderer Instance { get; private set; }
    [SerializeField] private SpriteRenderer gridRenderer;
    [SerializeField] private BoxCollider2D  gridCollider;

    private float padding  = 0.01f;
    private float _gridHeightOffset = 0.15f;
    private float _gridWidthOffset = 0.15f;
    private Tile[,] _tiles;
    private int _rows;
    private int _columns;

    public float CellSize { get; private set; }

    public void Awake()
    {
        Instance = this;
    }

    public void Initialize(int rows, int columns, Tile[,] tiles)
    {
        _rows = rows;
        _columns = columns;
        _tiles = tiles;
        //columns  -> width
        //rows -> height

        float availableScreenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2 * 0.9f;
        float availableScreenHeight = Camera.main.orthographicSize * 2 * 0.65f;
        float totalPaddingX = padding * (columns - 1);
        float totalPaddingY = padding * (rows - 1);

        float availableWidth = availableScreenWidth - totalPaddingX;
        float availableHeight = availableScreenHeight - totalPaddingY;


        CellSize = Mathf.Min(availableWidth / rows, availableHeight / columns);

        float gridHeight = CellSize * rows + totalPaddingX + 2 * _gridWidthOffset;
        float gridWidth = CellSize * columns + totalPaddingY + 2 * _gridHeightOffset;

        AdjustGridGameObject(gridWidth, gridHeight);

        InitializeGridWithTiles(CellSize);


        void InitializeGridWithTiles(float cellSize) {
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

                    tiles[j, i] = tile;
                }
            }
        }
    }
    private void AdjustGridGameObject(float boardWidth, float boardHeight)
    {
            gridRenderer.size = new Vector2(boardWidth, boardHeight);
            gridCollider.size = new Vector2(boardWidth, boardHeight);
    }
    public bool TryGetTileFromPosition(Vector3 worldPosition, out Tile tile)
    {
        Vector3 localPosition = worldPosition - transform.position;

        int col = Mathf.FloorToInt((localPosition.x + _columns / 2.0f * CellSize) / CellSize);
        int row = Mathf.FloorToInt((localPosition.y + _rows / 2.0f * CellSize) / CellSize);

        if (col < 0 || col >= _columns || row < 0 || row >= _rows)
        {
            tile = default;
            return false;
        }

        tile = _tiles[col,row];
        return true;
    }

}