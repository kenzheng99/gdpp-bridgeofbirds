using System.Numerics;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/**
 * Helper functions for things like isometric vector transforms
 */
public static class Utils {
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, -135, 0));

    /**
     * Converts a vector from input coordinates (screen space) to isometric coordinates (world space).
     */
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);

    /**
     * Scales a movement speed by the amount of vertical movement, to create the illusion
     * of the same 2D movement speed in all isometric directions.
     *
     * "direction" param should be in isometric (world) coordinates 
     */
    public static float NormalizeIsoMovementSpeed(float baseMoveSpeed, Vector3 direction) {
        float verticalAmount = Mathf.Abs(Vector3.Dot(direction.normalized, Vector3.forward.ToIso()));
        float adjustedMoveSpeed = baseMoveSpeed + baseMoveSpeed * verticalAmount * 0.5f;
        return adjustedMoveSpeed;
    }

    /**
     * Converts the mouse position to a point on an isometric plane parallel to the ground
     */
    public static Vector3 MouseToGroundPoint(float clickPlaneHeight) {
        Camera camera = Camera.main;
        Plane targetPlane = new Plane(Vector3.up, Vector3.zero + Vector3.up * clickPlaneHeight);

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        float distance;
        targetPlane.Raycast(ray, out distance);
        Vector3 planePoint = ray.GetPoint(distance);
        Vector3 groundPoint = new Vector3(planePoint.x, 0, planePoint.z);
        return groundPoint;
    }
}