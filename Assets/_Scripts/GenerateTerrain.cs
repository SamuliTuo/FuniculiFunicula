using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    private float TileSize = 1;
    Dictionary<(int, int), Object> LiveTiles = new Dictionary<(int, int), Object>();
    GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        tile = Resources.Load("Square") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 topLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        int tileMinX = Mathf.FloorToInt(topLeft.x / TileSize);
        int tileMinY = Mathf.FloorToInt(topLeft.y / TileSize);
        int tileMaxX = Mathf.CeilToInt(bottomRight.x / TileSize);
        int tileMaxY = Mathf.CeilToInt(bottomRight.y / TileSize);


        for (int i = tileMinX; i < tileMaxX; i++)
        {
            for (int j = tileMinY; j < tileMaxY; j++)
            {
                if (!LiveTiles.ContainsKey((i, j)))
                {
                    GameObject newTile = Instantiate(tile, new Vector3(i * TileSize + TileSize / 2, j * TileSize + TileSize / 2, 0), Quaternion.identity);
                    LiveTiles.Add((i, j), newTile);
                }
            }
        }
    }
}