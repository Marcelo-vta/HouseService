using UnityEngine;

public static class GameController
{
    // mantém a seleção do jogador entre cenas
    public static int SelectedPlayerId = -1;
    public static string SelectedPlayerName = "";

    public static void Init()
    {
        // sua inicialização existente aqui
        Debug.Log("GameController.Init() chamado. SelectedPlayerId = " + SelectedPlayerId);
    }

    // Opcional: helper para setar
    public static void SetSelectedPlayer(int id, string name = "")
    {
        SelectedPlayerId = id;
        SelectedPlayerName = name;
    }
}