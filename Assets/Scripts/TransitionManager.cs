using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [Header("Floors (arraste 4 de cada)")]
    public List<GameObject> floor1Prefabs;
    public List<GameObject> floor2Prefabs;

    [Header("Global Volumes (URP)")]
    public GameObject volumeFloor1;
    public GameObject volumeFloor2;
    public GameObject volumeBoss;

    [Header("Regras de navegação")]
    public bool allowBackWithinFloor = true;

    [Header("Boss (opcional)")]
    public GameObject bossPrefab;
    public string bossMessage = "BOSS FIGHT";
    public float bossTitleHold = 1.2f;

    [Header("Referências")]
    public Transform player;
    public Image fadeImage;
    public Graphic interFloorLabel;
    public Transform salasContainer;

    [Header("Mensagens")]
    public string floor1To2Message = "Andar 2";
    public float interFloorHoldSeconds = 1.2f;

    [Header("Fade")]
    public float fadeDuracao = 0.35f;

    // ---- estado ----
    private List<GameObject> allPrefabs = new List<GameObject>();
    private int cutIndex = 0;
    private List<int> ordemF1, ordemF2;
    private Dictionary<int, int> nextMap, prevMap;

    private GameObject salaAtualInst;
    private int salaAtualIndex = -1;

    // atalhos
    private int firstF1Idx, lastF1Idx, firstF2Idx = -1, lastF2Idx = -1;
    private const string ANCHOR_ENTRADA = "Entrada";
    private const string ANCHOR_SAIDA = "Saida";
    private const int BOSS = -999;

    //Geracao de items
    private int f1_itemIdx;
    private int f2_itemIdx;

    private int f1_mimicIdx;
    private int f2_mimicIdx;

    private List<string>meleeItems = new List<string>(){"wet", "long", "mop", "witch"};
    private List<string>rangedItems = new List<string>(){"spicy", "pepperoni", "cheese"};

    void Awake()
    {
        if (!player)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        // Desliga todos volumes no início
        SetVolume(volumeFloor1, false);
        SetVolume(volumeFloor2, false);
        SetVolume(volumeBoss, false);

        if (fadeImage)
        {
            var c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, 0f);
            fadeImage.gameObject.SetActive(false);
        }
        if (interFloorLabel)
        {
            var col = interFloorLabel.color;
            interFloorLabel.color = new Color(col.r, col.g, col.b, 0f);
            interFloorLabel.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        if (floor1Prefabs == null || floor1Prefabs.Count == 0)
        {
            Debug.LogError("TM: Preencha Floor1Prefabs.");
            return;
        }
        allPrefabs.AddRange(floor1Prefabs);
        cutIndex = floor1Prefabs.Count;

        bool temF2 = floor2Prefabs != null && floor2Prefabs.Count > 0;
        if (temF2) allPrefabs.AddRange(floor2Prefabs);

        // gera permutações
        ordemF1 = Perm(0, cutIndex);
        firstF1Idx = ordemF1[0];
        lastF1Idx = ordemF1[ordemF1.Count - 1];

        if (temF2)
        {
            ordemF2 = Perm(cutIndex, allPrefabs.Count);
            firstF2Idx = ordemF2[0];
            lastF2Idx = ordemF2[ordemF2.Count - 1];
            (nextMap, prevMap) = BuildMapsF1F2();
        }
        else
        {
            (nextMap, prevMap) = BuildMapsOnlyF1();
        }

        f1_itemIdx = Random.Range(0,floor1Prefabs.Count);
        f2_itemIdx = Random.Range(0,floor2Prefabs.Count);

        while (f1_mimicIdx == f1_itemIdx)
        {
            f1_mimicIdx = Random.Range(0,floor1Prefabs.Count);
        }

        while (f2_mimicIdx == f2_itemIdx)
        {
            f2_mimicIdx = Random.Range(0,floor1Prefabs.Count);
        }

        // sala inicial
        CreateSala(firstF1Idx, spawnNaEntrada: true);
        SetVolume(volumeFloor1, true);
    }

    public void GoForwardRandom()
    {
        if (salaAtualIndex == BOSS) return;

        // Se tem F2 e estamos no último do F2 -> boss
        if (lastF2Idx >= 0 && salaAtualIndex == lastF2Idx)
        {
            StartCoroutine(CO_GoToBoss());
            return;
        }

        if (nextMap.TryGetValue(salaAtualIndex, out int destino))
        {
            bool atravessaF1F2 = (salaAtualIndex < cutIndex) && (destino >= cutIndex);
            StartCoroutine(CO_TrocarSala(destino, true,
                showTitle: atravessaF1F2,
                title: atravessaF1F2 ? floor1To2Message : null,
                hold: interFloorHoldSeconds));
        }
    }

    public void GoBack()
    {
        if (salaAtualIndex == BOSS) return;
        if (salaAtualIndex == firstF2Idx) return;
        if (salaAtualIndex == firstF1Idx) return;
        if (!allowBackWithinFloor) return;

        if (prevMap != null && prevMap.TryGetValue(salaAtualIndex, out int destino))
        {
            StartCoroutine(CO_TrocarSala(destino, false, false, null, 0f));
        }
    }

    private void CreateSala(int index, bool spawnNaEntrada)
    {
        int currentFloor;

        currentFloor = index < cutIndex ? 1 : 2;

        int floorBasedIndex = currentFloor == 1 ?
            index 
        : 
            index - cutIndex;

        var inst = Instantiate(allPrefabs[index],
                               salasContainer ? salasContainer : null);
        salaAtualInst = inst;
        salaAtualIndex = index;

        string anchor = spawnNaEntrada ? ANCHOR_ENTRADA : ANCHOR_SAIDA;
        var ponto = FindAnchor(inst.transform, anchor);
        if (player && ponto) player.position = ponto.position;

        PlayerStates playerStates = player.gameObject.GetComponent<PlayerStates>();
        List<string> accurateItems = playerStates.cleaner ? meleeItems : rangedItems;

        int itemRoomIdx = currentFloor == 1 ? f1_itemIdx : f2_itemIdx;
        int mimicRoomIdx = currentFloor == 1 ? f1_mimicIdx : f2_mimicIdx;

        bool itemRoom = floorBasedIndex == itemRoomIdx;
        bool mimicRoom = floorBasedIndex == mimicRoomIdx;

        var floorVolume = currentFloor == 1 ? volumeFloor1 : volumeFloor2;

        // alterna volume com base no andar

        SetVolume(floorVolume, true);
        var itemSpawner = inst.GetComponentInChildren<ItemSpawner>();


        itemSpawner
            .gameObject
            .SetActive(itemRoom || mimicRoom);

        if (itemRoom)
        {
            int itemIndex = Random.Range(0, accurateItems.Count);

            itemSpawner.selectedItem = accurateItems[itemIndex];
            accurateItems.RemoveAt(itemIndex);
        }

        if (mimicRoom)
        {
            itemSpawner.isTrap = true;
        }
        
        Debug.Log($"TM: Sala criada: {inst.name} (idx {index})");
    }

    private IEnumerator CO_TrocarSala(int destinoIndex, bool spawnNaEntrada, bool showTitle, string title, float hold)
    {
        yield return FadeBoth(0f, 1f, title);
        if (showTitle && !string.IsNullOrEmpty(title) && hold > 0f)
            yield return new WaitForSeconds(hold);

        var antiga = salaAtualInst;
        CreateSala(destinoIndex, spawnNaEntrada);
        if (antiga) Destroy(antiga);

        yield return FadeBoth(1f, 0f, title, deactivateAtEnd: true);
    }

    private IEnumerator CO_GoToBoss()
    {
        if (!bossPrefab) yield break;

        yield return FadeBoth(0f, 1f, bossMessage);
        if (!string.IsNullOrEmpty(bossMessage) && bossTitleHold > 0f)
            yield return new WaitForSeconds(bossTitleHold);

        var antiga = salaAtualInst;

        var boss = Instantiate(bossPrefab, salasContainer ? salasContainer : null);
        salaAtualInst = boss;
        salaAtualIndex = BOSS;

        var entrada = FindAnchor(boss.transform, ANCHOR_ENTRADA);
        if (player && entrada) player.position = entrada.position;

        if (antiga) Destroy(antiga);

        // troca volume
        SetVolume(volumeFloor1, false);
        SetVolume(volumeFloor2, false);
        SetVolume(volumeBoss, true);

        DisablePortasAndTriggers(boss.transform);

        yield return FadeBoth(1f, 0f, bossMessage, deactivateAtEnd: true);
    }

    // === Volume helper ===
    private void SetVolume(GameObject vol, bool ativo)
    {
        if (vol) vol.SetActive(ativo);
    }

    // === Restante (permuta, fade, etc) ===
    private (Dictionary<int, int>, Dictionary<int, int>) BuildMapsOnlyF1()
    {
        var next = new Dictionary<int, int>();
        var prev = new Dictionary<int, int>();

        for (int i = 0; i < ordemF1.Count; i++)
        {
            int cur = ordemF1[i];
            int nxt = ordemF1[(i + 1) % ordemF1.Count];
            next[cur] = nxt;
            if (i > 0) prev[cur] = ordemF1[i - 1];
        }
        return (next, prev);
    }

    private (Dictionary<int, int>, Dictionary<int, int>) BuildMapsF1F2()
    {
        var next = new Dictionary<int, int>();
        var prev = new Dictionary<int, int>();

        for (int i = 0; i < ordemF1.Count; i++)
        {
            int cur = ordemF1[i];
            next[cur] = (i < ordemF1.Count - 1) ? ordemF1[i + 1] : firstF2Idx;
            if (i > 0) prev[cur] = ordemF1[i - 1];
        }

        for (int i = 0; i < ordemF2.Count; i++)
        {
            int cur = ordemF2[i];
            if (i < ordemF2.Count - 1) next[cur] = ordemF2[i + 1];
            if (i > 0) prev[cur] = ordemF2[i - 1];
        }
        return (next, prev);
    }

    private IEnumerator FadeBoth(float from, float to, string title, bool deactivateAtEnd = false)
    {
        if (!fadeImage) yield break;

        fadeImage.gameObject.SetActive(true);
        var ci = fadeImage.color;
        fadeImage.color = new Color(ci.r, ci.g, ci.b, from);

        if (!string.IsNullOrEmpty(title) && interFloorLabel)
        {
            interFloorLabel.gameObject.SetActive(true);
            SetLabelText(title);
            SetLabelAlpha(from);
        }

        float t = 0f;
        while (t < fadeDuracao)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / fadeDuracao);
            float eased = (to > from) ? 1f - Mathf.Pow(1f - p, 3f) : Mathf.Pow(p, 3f);

            float a = Mathf.Lerp(from, to, eased);
            var c = fadeImage.color;
            fadeImage.color = new Color(c.r, c.g, c.b, a);
            if (!string.IsNullOrEmpty(title) && interFloorLabel) SetLabelAlpha(a);
            yield return null;
        }

        var cf = fadeImage.color;
        fadeImage.color = new Color(cf.r, cf.g, cf.b, to);
        if (!string.IsNullOrEmpty(title) && interFloorLabel) SetLabelAlpha(to);

        if (deactivateAtEnd && Mathf.Approximately(to, 0f))
        {
            fadeImage.gameObject.SetActive(false);
            if (interFloorLabel)
            {
                interFloorLabel.gameObject.SetActive(false);
                SetLabelAlpha(0f);
            }
        }
    }

    private List<int> Perm(int startInclusive, int endExclusive)
    {
        var lst = new List<int>(endExclusive - startInclusive);
        for (int i = startInclusive; i < endExclusive; i++) lst.Add(i);
        for (int i = lst.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lst[i], lst[j]) = (lst[j], lst[i]);
        }
        return lst;
    }

    private Transform FindAnchor(Transform root, string name)
    {
        var t = root.Find(name);
        if (t) return t;
        foreach (Transform c in root.GetComponentsInChildren<Transform>(true))
            if (c.name == name) return c;
        return null;
    }

    private void DisablePortasAndTriggers(Transform root)
    {
        foreach (var p in root.GetComponentsInChildren<Porta>(true)) p.enabled = false;
        foreach (var col in root.GetComponentsInChildren<Collider2D>(true)) col.enabled = false;
    }

    private void SetLabelText(string msg)
    {
        if (!interFloorLabel) return;
        var uText = interFloorLabel.GetComponent<UnityEngine.UI.Text>();
        if (uText) { uText.text = msg; return; }
#if TMP_PRESENT
        var tmp = interFloorLabel.GetComponent<TMPro.TMP_Text>();
        if (tmp) { tmp.text = msg; return; }
#endif
    }

    private void SetLabelAlpha(float a)
    {
        if (!interFloorLabel) return;
        var c = interFloorLabel.color;
        interFloorLabel.color = new Color(c.r, c.g, c.b, a);
    }
}
