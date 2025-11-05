using UnityEngine;

public class Porta : MonoBehaviour
{
    public string nomeSaida; // exemplo: "SaidaLeste"
    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E)) // tecla E para entrar
        {
            GameObject salaAtual = transform.root.gameObject;
            string proximaSala = Andar1Generator.instancia.GetDestino(salaAtual.name, nomeSaida);

            if (proximaSala != null)
                TransitionManager.instancia.TrocarSala(salaAtual.name, proximaSala, nomeSaida);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jogadorPerto = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            jogadorPerto = false;
    }
}
