using System.Collections.Generic;
using UnityEngine;

public class Andar1Generator : MonoBehaviour
{
    [Header("Prefabs de Salas")]
    public List<GameObject> salas;
    public int quantidadeSalas = 4;

    private Dictionary<string, GameObject> mapaSalas = new Dictionary<string, GameObject>();
    private Dictionary<string, string> conexoes = new Dictionary<string, string>(); // porta → sala destino

    public static Andar1Generator instancia;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        GerarAndar();
    }

    void GerarAndar()
    {
        for (int i = 0; i < quantidadeSalas; i++)
        {
            GameObject novaSala = Instantiate(salas[Random.Range(0, salas.Count)], Vector3.zero, Quaternion.identity);
            novaSala.name = "Sala" + (i + 1);
            novaSala.SetActive(i == 0); // só a primeira começa ativa
            mapaSalas[novaSala.name] = novaSala;
        }

        // Liga as salas entre si logicamente (circular)
        var nomes = new List<string>(mapaSalas.Keys);
        for (int i = 0; i < nomes.Count; i++)
        {
            string atual = nomes[i];
            string proxima = nomes[(i + 1) % nomes.Count];

            conexoes[$"{atual}_SaidaLeste"] = proxima;
            conexoes[$"{proxima}_SaidaOeste"] = atual;
        }

        Debug.Log("✅ Andar gerado com " + quantidadeSalas + " salas conectadas logicamente.");
    }

    public string GetDestino(string salaAtual, string saida)
    {
        string chave = $"{salaAtual}_{saida}";
        return conexoes.ContainsKey(chave) ? conexoes[chave] : null;
    }

    public GameObject GetSala(string nome)
    {
        return mapaSalas.ContainsKey(nome) ? mapaSalas[nome] : null;
    }
}
