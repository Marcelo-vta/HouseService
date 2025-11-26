// SetSpritePixelHeight.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SetSpritePixelHeight : MonoBehaviour
{
    [Tooltip("Desired height in pixels at the reference resolution (e.g. 96).")]
    public int desiredPixelHeight = 96;

    [Tooltip("Project Pixels Per Unit (set to 16 if your sprites use 16 PPU).")]
    public float pixelsPerUnit = 16f;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null) return;

        float worldDesired = desiredPixelHeight / pixelsPerUnit;              // e.g., 96/16 = 6 units tall
        float spriteWorldHeight = sr.sprite.bounds.size.y;                   // sprite's height at localScale = 1
        if (spriteWorldHeight <= 0.0001f) return;

        Vector3 s = transform.localScale;
        s.y = worldDesired / spriteWorldHeight;
        s.x = Mathf.Abs(s.y) * (sr.sprite.bounds.size.x / sr.sprite.bounds.size.y);
        // keep the original aspect ratio. If you want uniform scale, you can
        // set s.x = s.y

        transform.localScale = s;
    }
}