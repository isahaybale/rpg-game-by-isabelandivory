using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Camera))]
public class CameraEdgeLock : MonoBehaviour
{
    [Header("References")]
    public Transform player;       // Player to follow
    public Tilemap tilemap;        // Tilemap to clamp to

    [Header("Camera Settings")]
    public float smoothSpeed = 0.1f;       // Smooth follow speed
    public Vector3 offset = new Vector3(0, 0, -10);  // Camera Z offset

    private Camera cam;
    private Vector3 minBounds;
    private Vector3 maxBounds;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (!tilemap)
        {
            Debug.LogError("Tilemap not assigned!");
            return;
        }

        // Get tilemap bounds
        Bounds bounds = tilemap.localBounds;
        minBounds = bounds.min;
        maxBounds = bounds.max;
    }

    void LateUpdate()
    {
        if (!player) return;

        float vertExtent = cam.orthographicSize;
        float horzExtent = vertExtent * cam.aspect;

        // Calculate potential camera position following player
        Vector3 targetPos = player.position + offset;

        // Calculate camera edges if it moved there
        float leftEdge = targetPos.x - horzExtent;
        float rightEdge = targetPos.x + horzExtent;
        float bottomEdge = targetPos.y - vertExtent;
        float topEdge = targetPos.y + vertExtent;

        // Lock X axis if edges reach map boundaries
        float clampedX = targetPos.x;
        if (leftEdge < minBounds.x) clampedX = minBounds.x + horzExtent;
        else if (rightEdge > maxBounds.x) clampedX = maxBounds.x - horzExtent;

        // Lock Y axis if edges reach map boundaries
        float clampedY = targetPos.y;
        if (bottomEdge < minBounds.y) clampedY = minBounds.y + vertExtent;
        else if (topEdge > maxBounds.y) clampedY = maxBounds.y - vertExtent;

        // Smooth camera movement
        transform.position = Vector3.Lerp(transform.position, new Vector3(clampedX, clampedY, offset.z), smoothSpeed);
    }
}