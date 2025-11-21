using UnityEngine;

public class PlayerIdentity : MonoBehaviour
{
    [Tooltip("ID único para este jogador (use 0,1,2... ou outro esquema)")]
    public int playerId = 0;

    [Tooltip("Opcional: nome legível do personagem")]
    public string playerName = "Player";
}