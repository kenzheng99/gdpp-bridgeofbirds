using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    CENTER,
    EDGE_LEFT,
    EDGE_RIGHT,
    EDGE_TOP,
    EDGE_BOTTOM,
    CORNER,
    BRIDGE
}

public class Tile : MonoBehaviour {
    [SerializeField] private TileType tileType = TileType.CENTER;
    [SerializeField] private GameObject edgeCollider;
    [SerializeField] private GameObject edgeColliderPrefab;
    private TileManager tileManager;
    public Vector2Int coordinates { get; private set; }
    public Vector2Int magpieDirection { get; private set; }

    void Start() {
        tileManager = TileManager.Instance;
        coordinates = WorldToGridCoordinates(transform.position);
        tileManager.RegisterTile(this);
        StartCoroutine(SetupEdgeCollider());
    }

    // this is a coroutine since it needs to run after all Start() methods are complete
    // so that all tiles have been registered with the TileManager
    private IEnumerator SetupEdgeCollider() {
        yield return null;
        if (isEdge()) {
            Vector2Int edgeCoordinates = coordinates + GetEdgeDirection();
            Tile tile = tileManager.GetTile(edgeCoordinates);
            if (tile && tile.GetTileType() == TileType.BRIDGE) {
                DisableEdgeCollider();
            }
        }
        
        SetupMagpieDirection();
    }

    public Vector2Int GetMagpieDirection() {
        return magpieDirection;
    }

    private void SetupMagpieDirection() {
        magpieDirection = Vector2Int.zero;

        Tile leftTile = tileManager.GetTile(coordinates + Vector2Int.left);
        Tile rightTile = tileManager.GetTile(coordinates + Vector2Int.right);
        Tile topTile = tileManager.GetTile(coordinates + Vector2Int.up);
        Tile bottomTile = tileManager.GetTile(coordinates + Vector2Int.down);
        

        if (tileType == TileType.EDGE_LEFT && leftTile == null) {
            magpieDirection = Vector2Int.left;
        } else if (tileType == TileType.EDGE_RIGHT && rightTile == null) {
            magpieDirection = Vector2Int.right;
        } else if (tileType == TileType.EDGE_TOP && topTile == null) {
            magpieDirection = Vector2Int.up;
        } else if (tileType == TileType.EDGE_BOTTOM && bottomTile == null) {
            magpieDirection = Vector2Int.down;
        } else if (tileType == TileType.BRIDGE) {
            if (leftTile == null && rightTile != null) {
                magpieDirection = Vector2Int.left;
                edgeCollider = Instantiate(edgeColliderPrefab, transform.position + Vector3.left, Quaternion.identity);
            } else if (rightTile == null && leftTile != null) {
                magpieDirection = Vector2Int.right;
                edgeCollider = Instantiate(edgeColliderPrefab, transform.position + Vector3.right, Quaternion.identity);
            } else if (topTile == null && bottomTile != null) {
                magpieDirection = Vector2Int.up;
                edgeCollider = Instantiate(edgeColliderPrefab, transform.position + Vector3.forward, Quaternion.identity);
            } else if (bottomTile == null && topTile != null) {
                magpieDirection = Vector2Int.down;
                edgeCollider = Instantiate(edgeColliderPrefab, transform.position + Vector3.back, Quaternion.identity);
            }
            
            // setup other colliders
            if (leftTile == null && magpieDirection != Vector2Int.left) {
                Instantiate(edgeColliderPrefab, transform.position + Vector3.left, Quaternion.identity);
            } 
            if (rightTile == null && magpieDirection != Vector2Int.right) {
                Instantiate(edgeColliderPrefab, transform.position + Vector3.right, Quaternion.identity);
            } 
            if (topTile == null && magpieDirection != Vector2Int.up) {
                Instantiate(edgeColliderPrefab, transform.position + Vector3.forward, Quaternion.identity);
            } 
            if (bottomTile == null && magpieDirection != Vector2Int.down) {
                Instantiate(edgeColliderPrefab, transform.position + Vector3.back, Quaternion.identity);
            }
        }
    }

    public bool isEdge() {
        return tileType is TileType.EDGE_LEFT or TileType.EDGE_RIGHT or TileType.EDGE_TOP or TileType.EDGE_BOTTOM;
    }

    public Vector2Int GetEdgeDirection() {
        // in current implementation, top is -x and right is +z
        switch (tileType) {
            case TileType.EDGE_LEFT:
                return new Vector2Int(-1, 0);
            case TileType.EDGE_RIGHT:
                return new Vector2Int(1, 0);
            case TileType.EDGE_TOP:
                return new Vector2Int(0, 1);
            case TileType.EDGE_BOTTOM:
                return new Vector2Int(0, -1);
            default:
                return new Vector2Int(0, 0);
        }
    }

    public TileType GetTileType() {
        return tileType;
    }

    public void DisableEdgeCollider() {
        if (!edgeCollider) {
            return;
        }
        edgeCollider.SetActive(false);
    }

    public void EnableEdgeCollider() {
        if (!edgeCollider) {
            return;
        }
        edgeCollider.SetActive(true);
    }

    /* static methods */
    public static Vector2Int WorldToGridCoordinates(Vector3 worldPosition) {
        int x = (int) Math.Floor(worldPosition.x);
        int y = (int) Math.Floor(worldPosition.z);
        Vector2Int coordinates = new Vector2Int(x, y);
        return coordinates;
    }

    public static Vector3 GridToWorldCoordinates(Vector2Int gridCoordinates) {
        return new Vector3(gridCoordinates.x + 0.5f, 0, gridCoordinates.y + 0.5f);
    }

    public static Vector3 SnapToGrid(Vector3 worldPosition) {
        return GridToWorldCoordinates(WorldToGridCoordinates(worldPosition));
    }

    public static Vector2Int ComputeGridDirection(Vector3 rawDirection) {
        if (Math.Abs(rawDirection.x) > Math.Abs(rawDirection.z)) {
            int x = rawDirection.x >= 0 ? 1 : -1;
            return new Vector2Int(x, 0);
        } else {
            int y = rawDirection.z >= 0 ? 1 : -1;
            return new Vector2Int(0, y);
        }
    }
}