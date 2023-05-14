using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static TileManager _instance;

    // note this is not a DontDestroyOnLoad singleton! A new instance should be added in every level
    public static TileManager Instance => _instance;

    private void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(gameObject);
        } else {
            _instance = this;
        }

        tiles = new Dictionary<Vector2Int, Tile>();
    }

    private Dictionary<Vector2Int, Tile> tiles;

    public void RegisterTile(Tile tile) {
        tiles.Add(tile.coordinates, tile);
    }


    public Tile GetTile(Vector3 position) {
        Vector2Int coordinates = Tile.WorldToGridCoordinates(position);
        return GetTile(coordinates);
    }

    public Tile GetTile(Vector2Int coordinates) {
        Tile tile;
        bool tileExists = tiles.TryGetValue(coordinates, out tile);
        if (!tileExists) {
            return null;
        } 
        return tile;
        
    }
    
    public bool tileExists(Vector3 position) {
        return GetTile(position) != null;
    }

    public bool tileExists(Vector2Int coordinates) {
        return GetTile(coordinates) != null;
    }
}
