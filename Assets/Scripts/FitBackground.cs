// FitBackground.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitBackground : MonoBehaviour
{
    [Tooltip("Small multiplier >1 to avoid gaps on extreme aspect ratios.")]
    public float safetyMargin = 1.05f;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("FitBackground: No main camera found.");
            return;
        }

        // world size of sprite (bounds are in world units when sprite PPU is used)
        float spriteWorldWidth = sr.sprite.bounds.size.x;
        float spriteWorldHeight = sr.sprite.bounds.size.y;

        // world size of the camera view
        float worldScreenHeight = cam.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight * cam.aspect;

        Vector3 scale = transform.localScale;
        scale.x = (worldScreenWidth / spriteWorldWidth) * safetyMargin;
        scale.y = (worldScreenHeight / spriteWorldHeight) * safetyMargin;

        transform.localScale = scale;
    }
}
