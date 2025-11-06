using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TransitionManager : MonoBehaviour
{
    public Image fadeImage; // arraste o FadeImage aqui
    public float fadeDuration = 0.8f;

    // Mapeamento de direções opostas
    private readonly Dictionary<string, string> direcoesOpostas = new Dictionary<string, string>
    {
        { "SaidaNorte", "SaidaSul" },
        { "SaidaSul", "SaidaNorte" },
        { "SaidaLeste", "SaidaOeste" },
        { "SaidaOeste", "SaidaLeste" }
    };

    public void StartTransition(string salaDestino, string direcao)
    {
        StartCoroutine(FadeAndTeleport(salaDestino, direcao));
    }

    private IEnumerator FadeAndTeleport(string salaDestino, string direcao)
    {
        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

        // Fade In (escurece)
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // 🔁 Define direção oposta correta
        string direcaoOposta = direcoesOpostas.ContainsKey(direcao) ? direcoesOpostas[direcao] : direcao;

        // Teleporte
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject novaSala = GameObject.Find(salaDestino);

        if (player != null && novaSala != null)
        {
            Transform novaPos = novaSala.transform.Find(direcaoOposta);
            if (novaPos != null)
            {
                player.transform.position = novaPos.position;
                Debug.Log($"🚪 Player teleportado de {direcao} → {direcaoOposta} na {salaDestino}");
            }
            else
            {
                Debug.LogWarning($"⚠️ Direção oposta '{direcaoOposta}' não encontrada em {salaDestino}");
            }
        }

        yield return new WaitForSeconds(0.3f);

        // Fade Out (clareia)
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        fadeImage.gameObject.SetActive(false);
    }
}
