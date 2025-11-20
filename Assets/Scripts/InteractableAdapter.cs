// InteractableAdapter.cs
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableAdapter : MonoBehaviour, IInteractable, IPulsable
{
    [Header("Visuals")]
    public GameObject shiningBorder;   // borda/outline que você já usa
    public GameObject mensagem;        // "Aperte E" ou similar

    [Header("Pulse Settings")]
    public float pulseBaseDuration = 0.6f;
    public int pulseRepeat = 1;

    // Nome do método a ser chamado ao interagir (compatibilidade)
    public string interactionMessage = "DoInteraction";

    Coroutine _pulseRoutine;

    void Reset()
    {
        if (shiningBorder == null)
        {
            Transform t = transform.Find("ShiningBorder");
            if (t != null) shiningBorder = t.gameObject;
        }
        if (mensagem == null)
        {
            Transform t = transform.Find("Mensagem");
            if (t != null) mensagem = t.gameObject;
        }
    }

    // IInteractable
    public void ShowHighlight(bool show)
    {
        if (shiningBorder != null)
            shiningBorder.SetActive(show);

        if (mensagem != null)
            mensagem.SetActive(show);
    }

    public void OnInteract(GameObject interactor)
    {
        if (!string.IsNullOrEmpty(interactionMessage))
            gameObject.SendMessage(interactionMessage, interactor, SendMessageOptions.DontRequireReceiver);

        Debug.Log($"OnInteract em {name} por {interactor.name}");
    }

    // IPulsable
    // intensity: 0..1
    public void Pulse(float intensity)
    {
        // intensidade controlará duração e/ou escala do pulso.
        if (_pulseRoutine != null)
            StopCoroutine(_pulseRoutine);

        _pulseRoutine = StartCoroutine(PulseRoutine(Mathf.Clamp01(intensity)));
    }

    IEnumerator PulseRoutine(float intensity)
    {
        if (shiningBorder == null)
            yield break;

        // Exemplo simples: usar intensity para modular duração e número de flashes:
        float duration = Mathf.Lerp(pulseBaseDuration * 0.4f, pulseBaseDuration, 1f - intensity);
        int repeats = Mathf.Max(1, Mathf.RoundToInt(Mathf.Lerp(1, pulseRepeat, intensity)));

        // opcional: ajustar escala ou alpha do shiningBorder se tiver componente apropriado
        // aqui só ligamos/desligamos para dar feedback, com variação em tempo.
        for (int i = 0; i < repeats; i++)
        {
            shiningBorder.SetActive(true);
            yield return new WaitForSeconds(duration * 0.5f);
            shiningBorder.SetActive(false);
            yield return new WaitForSeconds(duration * 0.5f);
        }

        _pulseRoutine = null;
    }
}