using System.Linq;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private PlayerStates playerStates;

    private Stopwatch obtainTime = new Stopwatch();
    private string obtainedItemName;

    void Start()
    {
        playerStates = GetComponent<PlayerStates>();
    }

    void Update()
    {
        if (playerStates.obtainingState)
        {
            
            if (obtainTime.ElapsedTimeSec() > 2)
            {
                playerStates.obtainingState = false;
            }
        }
    }

    public void ObtainItem(string itemName)
    {
        obtainedItemName = itemName;
        playerStates.obtainingState = true;

        playerStates.powerUps.Add(itemName);

        obtainTime.Restart();
    }
}
