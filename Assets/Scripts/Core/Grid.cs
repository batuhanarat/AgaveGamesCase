using UnityEngine;

public class Grid : MonoBehaviour
{
    private readonly Tile[,] tiles;
    private Tile currentSelectedTile;

    public void  OnMouseDown()
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