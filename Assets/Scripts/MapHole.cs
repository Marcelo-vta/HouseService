using UnityEngine;

public class MapHole : MonoBehaviour
{
    private PlayerStates player;
    private bool wasRolling = false;

    public BoxCollider2D firstCollider;
    public BoxCollider2D secondCollider;

    public LayerMask playerLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStates>();

        if (player.rollingState && !wasRolling)
        {
            firstCollider.excludeLayers = playerLayer;
            secondCollider.excludeLayers = playerLayer;
            wasRolling = true;
        }

        if(!player.rollingState && wasRolling)
        {
            firstCollider.excludeLayers = new LayerMask();
            secondCollider.excludeLayers = new LayerMask();
            wasRolling = false;
        }

    }
}
