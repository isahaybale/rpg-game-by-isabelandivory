using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Keeps a GameObject locked to the pixel grid for crisp pixel art.
/// Attach this to your player, NPCs, or other moving sprites.
/// </summary>
[ExecuteAlways] // Runs in Play mode and Edit mode
public class PixelSnapper : MonoBehaviour
{
    [Tooltip("Pixels per unit (PPU) used for your sprites. Must match your sprite import settings.")]
    public float pixelsPerUnit = 16f;

    void LateUpdate()
    {
        SnapToPixelGrid();
    }

    void SnapToPixelGrid()
    {
        if (pixelsPerUnit <= 0) return;

        Vector3 pos = transform.position;

        pos.x = Mathf.Round(pos.x * pixelsPerUnit) / pixelsPerUnit;
        pos.y = Mathf.Round(pos.y * pixelsPerUnit) / pixelsPerUnit;

        transform.position = pos;
    }
}
