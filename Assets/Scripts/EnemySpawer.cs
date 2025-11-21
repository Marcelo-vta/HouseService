using System.Collections.Generic;
using UnityEngine;

public class EnemySpawer : MonoBehaviour
{
    public List<GameObject> enemiesToSpawn;

    private GameObject enemySpawned;
    private bool spawned = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int chosenEnemy = Random.Range(0, enemiesToSpawn.Count);
        enemySpawned = Instantiate(enemiesToSpawn[chosenEnemy], transform);
        spawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawned == null && spawned)
        {
            Destroy(gameObject);
        }
    }
}
