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

    [Header("Player")]
    private PlayerStates player;

    [Header("Health")]
    private int maxHearts;
    // currentUnits is in half-heart units (0..maxHearts*2). Example: 7 = 3 full + 1 half
    private int currentUnits;

    private List<Image> hearts = new List<Image>();
    private int lastMax = -1;
    private int lastUnits = -1;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerStates>();
        maxHearts = player.max_health;
        currentUnits = (int) player.health * 2;
        BuildHearts(maxHearts, currentUnits);
        UpdateHeartsVisual(maxHearts, currentUnits);
    }

    void Update()
    {
        maxHearts = player.max_health;
        currentUnits = Mathf.RoundToInt(player.health * 2);
        // Polling fallback: if your health system drives currentUnits externally, this keeps the UI in sync
        if (currentUnits != lastUnits || maxHearts != lastMax)
        {
            if (maxHearts != lastMax) BuildHearts(maxHearts, currentUnits);
            UpdateHeartsVisual(maxHearts, currentUnits);
        }
    }

    public void BuildHearts(int maxHearts, int currentUnits)
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

    public void UpdateHeartsVisual(int maxHearts, int currentUnits)
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
        maxHearts = player.max_health;
        currentUnits = Mathf.Clamp(currentInUnits, 0, (maxHeartsOverride > 0 ? maxHeartsOverride * 2 : maxHearts * 2));
        if (maxHeartsOverride > 0 && maxHeartsOverride != maxHearts)
        {
            maxHearts = maxHeartsOverride;
            BuildHearts(maxHearts, currentUnits);
        }
        UpdateHeartsVisual(maxHearts, currentUnits);
    }

    // For convenience if your game uses whole-heart numbers:
    public void SetHealthHearts(float currentHearts, int maxHeartsOverride = -1)
    {
        int units = Mathf.RoundToInt(currentHearts * 2f); // e.g. 3.5 hearts -> 7 units
        SetHealthUnits(units, maxHeartsOverride);
    }
}