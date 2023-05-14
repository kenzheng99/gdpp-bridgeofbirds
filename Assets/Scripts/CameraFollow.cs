using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed = 1.0f;

    public CameraShake cameraShake;

    void Start() {
        transform.position = getTargetPosition();
        cameraShake = transform.GetChild(0).GetComponent<CameraShake>();
    }

    void Update() {
        Vector3 targetPosition = getTargetPosition();
        float distance = (targetPosition - transform.position).magnitude;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * distance * Time.deltaTime);
        transform.position = newPosition;
    }
    
    private Vector3 getTargetPosition() {
        Vector3 targetPosition = objectToFollow.position + offset;
        return targetPosition;
    }
}
