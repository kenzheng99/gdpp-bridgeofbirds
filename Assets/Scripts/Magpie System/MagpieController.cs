using System.Collections.Generic;
using UnityEngine;

public class MagpieController : MonoBehaviour {
    [SerializeField] private GameObject magpieOutlinePrefab;
    [SerializeField] private GameObject magpiePrefab;
    [SerializeField] private GameObject wallPrefab;

    private Vector2Int direction = Vector2Int.right;

    private TileManager tileManager;
    private Queue<GameObject> magpieOutlines;
    private List<Magpie> magpies;
    private Stack<GameObject> walls;
    private Tile currentTile;
    private Tile lastOutlinedTile;
    private Tile lastStartingTile;
    private Tile lastEndingTile;

    private void Start() {
        tileManager = TileManager.Instance;
        magpieOutlines = new Queue<GameObject>();
        walls = new Stack<GameObject>();
        // TODO make this dynamic
        magpies = new List<Magpie>();
        // magpies.Add(Instantiate(magpiePrefab, Vector3.zero, Quaternion.identity));
        // magpies.Add(Instantiate(magpiePrefab, Vector3.left, Quaternion.identity));
        // magpies.Add(Instantiate(magpiePrefab, Vector3.right, Quaternion.identity));
    }

    private void Update() {
        if (magpies.Count == 0) {
            return;
        }
        if (DialogueManager.Instance && DialogueManager.Instance.triggeredTeachMagpieBridgeDialogue == false && magpies.Count > 0)
        {
            DialogueManager.Instance.StartTeachMagpieBridgeDialogue();
            GameManager.Instance.dialogueIsPlaying = true;
            DialogueManager.Instance.triggeredTeachMagpieBridgeDialogue = true;
        }
        currentTile = tileManager.GetTile(transform.position);
        if (!currentTile) {
            return;
        }


        if (currentTile != lastOutlinedTile) {
            direction = currentTile.GetMagpieDirection();
            if (direction != Vector2Int.zero) {
                ClearMagpieOutlines();
                SpawnMagpieOutlines();
                lastOutlinedTile = currentTile;
            } else {
                ClearMagpieOutlines();
                lastOutlinedTile = null;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.E)) {
            SummonMagpies();
        }
    }

    public void AddMagpie(Magpie magpie) {
        magpies.Add(magpie);
    }

    public int GetMagpieCount() {
        return magpies.Count;
    }

    private void SpawnMagpieOutlines() {
        for (int i = 0; i < magpies.Count; i++) {
            Vector2Int spawnCoordinates = currentTile.coordinates + (i + 1) * direction;
            if (tileManager.tileExists(spawnCoordinates)) {
                break;
            }
            Vector3 spawnPosition = Tile.GridToWorldCoordinates(spawnCoordinates);
            GameObject magpie = Instantiate(magpieOutlinePrefab, spawnPosition, Quaternion.identity);
            magpieOutlines.Enqueue(magpie);
        }
    }

    private void ClearMagpieOutlines() {
        while (magpieOutlines.Count > 0) {
            Destroy(magpieOutlines.Dequeue());
        }
    }

    private void SummonMagpies() {
        if (magpieOutlines.Count == 0) {
            return;
        }
        
        // reset colliders
        lastStartingTile?.EnableEdgeCollider();
        lastEndingTile?.EnableEdgeCollider();
        
        currentTile.DisableEdgeCollider();
        lastStartingTile = currentTile;

        while (walls.Count > 0) {
            Destroy(walls.Pop());
        }
        
        int i = 0;
        Vector3 direction3d = new Vector3(direction.x, 0, direction.y);
        while (magpieOutlines.Count > 0) {
            GameObject outline = magpieOutlines.Dequeue();
            Vector3 spawnPosition = outline.transform.position;
            // magpies[i].transform.position = spawnPosition;
            magpies[i].FlyToPosition(spawnPosition);

            Vector3 leftWallPos = spawnPosition + Quaternion.AngleAxis(-90, Vector3.up) * direction3d;
            Vector3 rightWallPos = spawnPosition + Quaternion.AngleAxis(90, Vector3.up) * direction3d;

            if (!tileManager.tileExists(leftWallPos)) {
                walls.Push(Instantiate(wallPrefab, leftWallPos, Quaternion.identity));
            }
            
            if (!tileManager.tileExists(rightWallPos)) {
                walls.Push(Instantiate(wallPrefab, rightWallPos, Quaternion.identity));
            }
            
            Destroy(outline);
            i++;
        }

        Vector3 endingPosition = magpies[i - 1].targetPosition + direction3d;
        Tile endingTile = tileManager.GetTile(endingPosition);
        if (endingTile) {
            endingTile.DisableEdgeCollider();
        } else {
            walls.Push(Instantiate(wallPrefab, endingPosition, Quaternion.identity));
        }
        lastEndingTile = endingTile;

    }
}