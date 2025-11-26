// PlayerInteractor2D.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractor2D : MonoBehaviour
{
    [Header("Input")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Distances")]
    public float maxConsiderDistance = 4f; // distancia maxima para considerar highlight/pulse
    public float interactDistance = 1.5f; // distancia maxima para realmente "interagir" (E ativa)

    [Header("Tags (opcionais)")]
    public string[] interactableTags = new string[] { "Selectable", "Interactable" };

    private readonly List<IInteractable> _nearby = new List<IInteractable>();
    private IInteractable _currentHighlighted = null;
    private Transform _playerTransform;

    void Awake()
    {
        _playerTransform = transform;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("PlayerInteractor2D: collider deve ser isTrigger para detectar proximidade.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable ia = other.GetComponent<IInteractable>() ?? other.GetComponentInParent<IInteractable>();
        if (ia != null && !_nearby.Contains(ia))
            _nearby.Add(ia);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable ia = other.GetComponent<IInteractable>() ?? other.GetComponentInParent<IInteractable>();
        if (ia != null)
            _nearby.Remove(ia);
    }

    void Update()
    {
        if (_nearby.Count == 0)
        {
            SetHighlighted(null, 0f);
            return;
        }

        float bestSqr = maxConsiderDistance * maxConsiderDistance;
        IInteractable best = null;
        float bestDist = maxConsiderDistance;

        foreach (var ia in _nearby)
        {
            MonoBehaviour mb = ia as MonoBehaviour;
            if (mb == null) continue;
            float dist = Vector2.Distance(mb.transform.position, _playerTransform.position);
            if (dist <= maxConsiderDistance && dist < bestDist)
            {
                bestDist = dist;
                best = ia;
            }
        }

        float intensity = best != null ? DistanceToIntensity(bestDist, maxConsiderDistance) : 0f;
        SetHighlighted(best, intensity);

        // MODIFIED: Added InputManager check
        if (best != null && (Input.GetKeyDown(interactKey) || InputManager.Instance.InteractInput))
        {
            // so interage se estiver dentro do interactDistance
            if (bestDist <= interactDistance)
            {
                best.OnInteract(gameObject);
            }
            else
            {
                // opcional: pulse fraco para sinalizar "muito longe"
                IPulsable p = (best as MonoBehaviour)?.GetComponent<IPulsable>() ?? (best as MonoBehaviour)?.GetComponentInParent<IPulsable>();
                if (p != null) p.Pulse(DistanceToIntensity(bestDist, maxConsiderDistance) * 0.3f);
                // ou feedback sonoro, mensagem, etc.
            }
        }
    }

    // converte distancia->intensity (0..1): 1 quando no jogador, 0 em maxDistance
    private float DistanceToIntensity(float distance, float maxDistance)
    {
        if (maxDistance <= 0f) return 0f;
        float t = Mathf.Clamp01(distance / maxDistance);
        // queremos intensidade maior quando mais perto:
        return 1f - t;
    }

    private void SetHighlighted(IInteractable target, float intensity)
    {
        if (_currentHighlighted == target) return;

        if (_currentHighlighted != null)
            _currentHighlighted.ShowHighlight(false);

        _currentHighlighted = target;

        if (_currentHighlighted != null)
        {
            _currentHighlighted.ShowHighlight(true);

            // chama Pulse com intensidade proporcional (opcional)
            IPulsable p = (_currentHighlighted as MonoBehaviour)?.GetComponent<IPulsable>() ?? (_currentHighlighted as MonoBehaviour)?.GetComponentInParent<IPulsable>();
            if (p != null) p.Pulse(intensity);
        }
    }

    // API publica para interacao via codigo
    public void InteractCurrent()
    {
        if (_currentHighlighted != null)
        {
            // so interage se estiver proximo o suficiente
            MonoBehaviour mb = _currentHighlighted as MonoBehaviour;
            if (mb != null)
            {
                float dist = Vector2.Distance(mb.transform.position, _playerTransform.position);
                if (dist <= interactDistance)
                    _currentHighlighted.OnInteract(gameObject);
            }
        }
    }
}
