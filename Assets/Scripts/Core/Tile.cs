using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Vector3 position;
    private int X,Y;

    public void SetPosition(Vector3 position)
    {
        this.position = position;
        transform.position = position;
    }
    public void SetIndex(int x, int y)
    {
        X = x;
        Y = y;
    }
}