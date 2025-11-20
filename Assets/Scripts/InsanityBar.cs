using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InsanityBar : MonoBehaviour
{
    [Header("References")]
    [Tooltip("RectTransform of the bar background (the masked shape).")]
    public RectTransform bgRect;

    [Tooltip("RectTransform of the fill image (child inside the mask).")]
    public RectTransform fillRect;

    [Tooltip("Optional: Image component on the fill if you want to tint it")]
    public Image fillImage;

    [Header("Smoothing")]
    [Tooltip("Speed of smoothing. 0 = instant.")]
    public float smoothSpeed = 8f;

    [Header("Player")]
    private PlayerStates player;

    // internal target (0..1)
    public float target = 0f;
    public float displayed = 0f;

    void Reset()
    {
        // try to auto-assign sensible defaults
        if (bgRect == null) bgRect = GetComponent<RectTransform>();
        if (fillRect == null && bgRect != null && bgRect.childCount > 0)
            fillRect = bgRect.GetChild(0) as RectTransform;
        if (fillImage == null && fillRect != null)
            fillImage = fillRect.GetComponent<Image>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStates>();
        // safety checks
        if (bgRect == null) Debug.LogError("InsanityBar: bgRect not assigned.", this);
        if (fillRect == null) Debug.LogError("InsanityBar: fillRect not assigned.", this);

        // initial values
        displayed = target = Mathf.Clamp01(player.insanity);
        ApplyWidth(displayed);
    }

    void Update()
    {
        target = player.insanity;
        if (smoothSpeed > 0f)
        {
            displayed = Mathf.MoveTowards(displayed, target, Time.deltaTime * smoothSpeed);
            ApplyWidth(displayed);
        }
        else if (displayed != target)
        {
            displayed = target;
            ApplyWidth(displayed);
        }
    }

    // public API: set insanity in normalized range [0..1]
    public void SetInsanityNormalized(float normalized)
    {
        target = Mathf.Clamp01(normalized);
    }

    // optional: instant set without smoothing
    public void SetInsanityInstant(float normalized)
    {
        target = Mathf.Clamp01(normalized);
        displayed = target;
        ApplyWidth(displayed);
    }

    void ApplyWidth(float t)
    {
        if (bgRect == null || fillRect == null) return;

        // If fillRect uses anchors left-stretch (anchorMin.x = 0, anchorMax.x = 1),
        // we can set anchoredPosition/sizeDelta differently. Here we assume
        // the fill is anchored to left (anchorMin.x = anchorMax.x = 0).
        //
        // Implementation below uses bgRect width and sets fillRect.sizeDelta.x
        // with anchorMin.x = anchorMax.x = 0 (left anchored). If your fill
        // anchors are different, adjust accordingly.

        float fullWidth = bgRect.rect.width;
        float newWidth = fullWidth * Mathf.Clamp01(t);

        // If fillRect anchors are (0,0)-(1,1) (stretch), change approach:
        // fillRect.localScale = new Vector3(t, 1f, 1f); // alternative hack

        Vector2 size = fillRect.sizeDelta;
        // If the fillRect was set up to match bg with sizeDelta.x == 0, 
        // then you may need to use:
        // size.x = newWidth - (fillRect.anchorMax.x - fillRect.anchorMin.x) * fullWidth;
        // Simpler path: ensure fillRect anchors are left (both x anchors = 0).
        size.x = newWidth;
        fillRect.sizeDelta = size;
    }
}
