using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector3 position;
    private int X,Y;

    public void SetWorldPosition(Vector3 position)
    {
        this.position = position;
       // transform.position = position;
    }
    public void SetSize(float width, float height)
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float scaleX = width / spriteSize.x;
        float scaleY = height / spriteSize.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
    public void SetIndex(int x, int y)
    {
        X = x;
        Y = y;
    }
}