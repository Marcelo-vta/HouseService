using System.Collections;
using System.Linq;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;

public class Broom : MonoBehaviour, IWeapon
{
    public float attackCooldown;

    public GameObject slashPrefab;

    public SpriteRenderer broomSpriteRenderer;
    public Sprite witchSprite;

    public GameObject wet;
    public GameObject mop;

    public Transform hand;
    public bool witch = false;

    private PlayerStates playerStates;

    public void Start()
    {
        playerStates = GameObject.FindGameObjectWithTag("Player")
            .GetComponent<PlayerStates>();

    }

    public void Fire(Transform playerTransform = null)
    {
        GameObject hit = Instantiate(slashPrefab, playerTransform);
        Slash hitScript = hit.GetComponent<Slash>();

        playerStates.AttackCooldown(attackCooldown);
    }

    public void Update()
    {

        if (playerStates.powerUps.Contains("witch"))
        {
            witch = true;
            broomSpriteRenderer.sprite = witchSprite;
        }

        if (playerStates.powerUps.Contains("wet"))
        {
            wet.SetActive(true);
        }

        if (playerStates.powerUps.Contains("mop"))
        {
            mop.SetActive(true);
        }

    }
}
