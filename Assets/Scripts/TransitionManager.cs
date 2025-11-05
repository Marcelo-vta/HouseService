using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instancia;
    public Image fadeImage; // arraste um Image UI preta com Alpha 0

    private void Awake()
    {
        instancia = this;
    }

    public void TrocarSala(string salaAtual, string salaDestino, string saida)
    {
        StartCoroutine(TrocarCoroutine(salaAtual, salaDestino, saida));
    }

    IEnumerator TrocarCoroutine(string salaAtual, string salaDestino, string saida)
    {
        yield return StartCoroutine(Fade(true));

        GameObject salaAntiga = Andar1Generator.instancia.GetSala(salaAtual);
        GameObject salaNova = Andar1Generator.instancia.GetSala(salaDestino);

        salaAntiga.SetActive(false);
        salaNova.SetActive(true);

        // Move player para a entrada correspondente da nova sala
        string direcaoOposta = DirecaoOposta(saida);
        Transform entradaNova = FindRecursivo(salaNova.transform, direcaoOposta);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (entradaNova != null && player != null)
            player.transform.position = entradaNova.position;

        yield return StartCoroutine(Fade(false));
    }

    IEnumerator Fade(bool fadeIn)
    {
        float duracao = 0.5f;
        float tempo = 0f;
        Color cor = fadeImage.color;
        float alvo = fadeIn ? 1f : 0f;
        float inicial = fadeIn ? 0f : 1f;

        while (tempo < duracao)
        {
            cor.a = Mathf.Lerp(inicial, alvo, tempo / duracao);
            fadeImage.color = cor;
            tempo += Time.deltaTime;
            yield return null;
        }

        cor.a = alvo;
        fadeImage.color = cor;
    }

    string DirecaoOposta(string direcao)
    {
        if (direcao.Contains("Norte")) return "SaidaSul";
        if (direcao.Contains("Sul")) return "SaidaNorte";
        if (direcao.Contains("Leste")) return "SaidaOeste";
        if (direcao.Contains("Oeste")) return "SaidaLeste";
        return "SaidaSul";
    }

    Transform FindRecursivo(Transform pai, string nome)
    {
        foreach (Transform filho in pai)
        {
            if (filho.name == nome)
                return filho;

            Transform achado = FindRecursivo(filho, nome);
            if (achado != null)
                return achado;
        }
        return null;
    }
}
