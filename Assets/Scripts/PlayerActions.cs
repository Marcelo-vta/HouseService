using System.Linq;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    private PlayerStates playerStates;

    private Stopwatch obtainTime = new Stopwatch();
    private Stopwatch scaredTime = new Stopwatch();

    private bool scare;
    private string obtainedItemName;

    private GameObject itemReceived;

    void Start()
    {
        playerStates = GetComponent<PlayerStates>();
        itemReceived = GameObject.FindGameObjectWithTag("Item");
    }

    void Update()
    {
        if (playerStates.obtainingState)
        {
            itemReceived.SetActive(true);
            if (obtainTime.ElapsedTimeSec() > 2)
            {
                itemReceived.SetActive(false);
                playerStates.obtainingState = false;
            }
        }
        else
        {
            itemReceived.SetActive(false);
        }

        if (scare)
        {
            if (scaredTime.ElapsedTimeSec() > .5)
            {
                playerStates.scaredState = true;
            }

            if (scaredTime.ElapsedTimeSec() > 2.5)
            {
                playerStates.scaredState = false;
                scare = false;
            }
        }
    }

    public void ObtainItem(string itemName)
    {
        obtainedItemName = itemName;
        playerStates.obtainingState = true;

        playerStates.powerUps.Add(itemName);

        itemReceived.GetComponent<ItemReceived>().itemName = itemName;
        obtainTime.Restart();
    }

    public void Scare()
    {
        playerStates.insanity += 1;
        scare = true;

        scaredTime.Restart();
    }
}
