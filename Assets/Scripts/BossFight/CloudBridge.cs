using UnityEngine;

public class CloudBridge : MonoBehaviour {
    [SerializeField] private GameObject bridgeModel;
    
    private TileManager tileManager;
    private Tile startTile;
    private Tile endTile;
    void Start() {
        tileManager = TileManager.Instance;
        // hardcoded to be a distance of 3 away from bridge, don't change the level layout anymore lol
        startTile = tileManager.GetTile(transform.position + transform.rotation * Vector3.right * 3);
        endTile = tileManager.GetTile(transform.position - transform.rotation * Vector3.right * 3);
        bridgeModel.SetActive(false);
    }

    public void ActivateBridge() {
        bridgeModel.SetActive(true);
        startTile.DisableEdgeCollider();
        endTile.DisableEdgeCollider();
    }
    
    public void DeactivateBridge() {
        bridgeModel.SetActive(false);
        startTile.EnableEdgeCollider();
        endTile.EnableEdgeCollider();
    }

}
