using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer gridRenderer;
    [SerializeField] public Tile[,] _tiles;
    private Tile _currentSelectedTile;
    private int _rows;
    private int _columns;
    private float padding  = 0.01f;
    private float _gridHeightOffset = 0.15f;
    private float _gridWidthOffset = 0.15f;
    public float GridSize;

    public float ItemSize { get; private set; }


    public void Start()
    {
        Initialize(8,8);
    }

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


        float cellSize = Mathf.Min(availableWidth / rows, availableHeight / columns);

        float boardWidth = cellSize * rows + totalPaddingX + 2 * _gridWidthOffset;
        float boardHeight = cellSize * columns + totalPaddingY + 2 * _gridHeightOffset;

        AdjustBoardSprite(boardWidth, boardHeight);
        _tiles = new Tile[rows, columns];

        InitializeBoardWithCells(cellSize);


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
                    tile.SetIndex(cellIndex.x, cellIndex.y);

                    _tiles[j, i] = tile;
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


    private float AdjustGridSprite(float targetSize)
    {
        if (gridRenderer != null)
        {
            Vector2 spriteSize = gridRenderer.sprite.bounds.size;

            float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;
            float screenHeight = Camera.main.orthographicSize * 2;

            float desiredWidth = screenWidth * 0.90f ;
            float desiredHeight = screenHeight * 0.85f;

            float scaleX = desiredWidth / spriteSize.x;
            float scaleY = desiredHeight / spriteSize.y;

            var scale =  Mathf.Min(scaleX, scaleY);


            transform.localScale = new Vector3(scale, scale, 1);
            return scale;
        }
        return 1f;
    }



    public void OnMouseDown()
    {
        Debug.Log("Input started");
    }

    public void OnMouseDrag()
    {
        Debug.Log("Input is taken right now");
    }

    public void  OnMouseUp()
    {
        Debug.Log("Input finished");
    }



}