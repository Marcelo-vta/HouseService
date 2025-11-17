using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartsUIManager : MonoBehaviour
{
    [Header("Refs")]
    public Transform heartsContainer;    // assign the RectTransform of the UI container
    public GameObject heartPrefab;       // the HeartPrefab (single Image)

    [Header("Sprites")]
    public Sprite heartEmptySprite;      // heart_empty
    public Sprite heartHalfSprite;       // heart_half
    public Sprite heartFullSprite;       // heart_full

    [Header("Health (units)")]
    public int maxHearts = 5;            // e.g. 5 hearts
    // currentUnits is in half-heart units (0..maxHearts*2). Example: 7 = 3 full + 1 half
    [Tooltip("Current health in half-heart units (each heart = 2 units).")]
    public int currentUnits = 10;        // default full: 5 hearts -> 10 units

    private List<Image> hearts = new List<Image>();
    private int lastMax = -1;
    private int lastUnits = -1;

    void Start()
    {
        BuildHearts();
        UpdateHeartsVisual();
    }

    void Update()
    {
        // Polling fallback: if your health system drives currentUnits externally, this keeps the UI in sync
        if (currentUnits != lastUnits || maxHearts != lastMax)
        {
            if (maxHearts != lastMax) BuildHearts();
            UpdateHeartsVisual();
        }
    }

    public void BuildHearts()
    {
        // clear existing
        foreach (var img in hearts)
            if (img != null) Destroy(img.gameObject);
        hearts.Clear();

        if (heartPrefab == null || heartsContainer == null)
        {
            Debug.LogWarning("HeartsUIManager: assign heartPrefab and heartsContainer.");
            return;
        }

        for (int i = 0; i < maxHearts; i++)
        {
            GameObject go = Instantiate(heartPrefab, heartsContainer);
            go.name = "Heart_" + i;
            go.SetActive(true);
            Image im = go.GetComponent<Image>();
            if (im == null)
            {
                Debug.LogError("Heart prefab needs an Image component.");
                Destroy(go);
                continue;
            }
            hearts.Add(im);
        }

        lastMax = maxHearts;
    }

    public void UpdateHeartsVisual()
    {
        lastUnits = currentUnits;
        int units = Mathf.Clamp(currentUnits, 0, maxHearts * 2);

        for (int i = 0; i < hearts.Count; i++)
        {
            Image heartImage = hearts[i];
            int heartIndex = i;

            // Each heart represents two units:
            // heart 0 -> units 0..1 (leftmost)
            int heartUnitStart = heartIndex * 2;
            int heartUnitEnd = heartUnitStart + 1;

            // Determine state for this heart:
            // - full if both units present
            // - half if only the first of the two units is present
            // - empty if none present
            Sprite chosen = heartEmptySprite;

            if (units >= heartUnitEnd + 1)
            {
                // both units present -> full
                chosen = heartFullSprite;
            }
            else if (units >= heartUnitStart + 1)
            {
                // only first unit present -> half
                chosen = heartHalfSprite;
            }
            else
            {
                // none -> empty
                chosen = heartEmptySprite;
            }

            heartImage.sprite = chosen;
            heartImage.SetNativeSize(); // optional: keep pixel perfect sizing
        }
    }

    // Convenience API: call this from your health system when health changes.
    // currentInUnits = currentHp * 2 (if your HP is in whole hearts).
    public void SetHealthUnits(int currentInUnits, int maxHeartsOverride = -1)
    {
        currentUnits = Mathf.Clamp(currentInUnits, 0, (maxHeartsOverride > 0 ? maxHeartsOverride * 2 : maxHearts * 2));
        if (maxHeartsOverride > 0 && maxHeartsOverride != maxHearts)
        {
            maxHearts = maxHeartsOverride;
            BuildHearts();
        }
        UpdateHeartsVisual();
    }

    // For convenience if your game uses whole-heart numbers:
    public void SetHealthHearts(float currentHearts, int maxHeartsOverride = -1)
    {
        int units = Mathf.RoundToInt(currentHearts * 2f); // e.g. 3.5 hearts -> 7 units
        SetHealthUnits(units, maxHeartsOverride);
    }
}