using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject entrance;
    public GameObject exit;

    public GameObject spawners;

    private GameObject player;

    private bool started = false;
    private bool ended = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        exit.SetActive(false);
        entrance.SetActive(false);

        StartCoroutine(initialCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (started && !ended)
        {
            if (spawners.transform.childCount == 0)
            {
                ended = true;
                PlayerAudio playerAudio = player.GetComponent<PlayerAudio>();
                playerAudio.clearedRoom();
            }
        }

        exit.SetActive(ended);
    }

    IEnumerator initialCoroutine()
    {
        player.SetActive(false);

        yield return new WaitForSeconds(.5f);
        entrance.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        player.SetActive(true);
        started = true;

        yield return new WaitForSeconds(1);

        for (int i = 0; i < spawners.transform.childCount; i++)
        {
            Transform spawner = spawners.transform.GetChild(i);
            spawner.gameObject.SetActive(true);
        }
    }
}
