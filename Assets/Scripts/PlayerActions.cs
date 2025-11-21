using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerActions : MonoBehaviour, IDamageable
{
    private PlayerStates playerStates;

    private Stopwatch obtainTime = new Stopwatch();
    private Stopwatch scaredTime = new Stopwatch();

    private bool scare;
    private string obtainedItemName;
    private bool dead = false;

    private GameObject itemReceived;
    private Animator animator;

    void Start()
    {
        playerStates = GetComponent<PlayerStates>();
        itemReceived = GameObject.FindGameObjectWithTag("Item");
        
        animator = playerStates.gameObject.GetComponentInChildren<Animator>();
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
        playerStates.insanity += .5f;
        scare = true;

        scaredTime.Restart();
    }

    public void TakeDamage(float damageAmount, float knockbackForce = 0f)
    {
        if (playerStates.damageable)
        {
            playerStates.health -= damageAmount;
            playerStates.hurtState = true;
            animator.SetTrigger("hurt");

            StartCoroutine(HurtCoroutine());
        }


    }

    IEnumerator HurtCoroutine()
    {  
        playerStates.ivulnerability = true;
        yield return new WaitForSeconds(.8f);

        playerStates.hurtState = false;
        
        yield return new WaitForSeconds(1);
        playerStates.ivulnerability = false;
    }



}
