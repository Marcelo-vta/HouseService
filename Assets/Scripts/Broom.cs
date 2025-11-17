using System.Linq;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;

public class Broom : MonoBehaviour
{
    public GameObject slashPrefab;

    public SpriteRenderer broomSpriteRenderer;
    public Sprite witchSprite;

    public GameObject wet;
    public GameObject mop;

    public Transform hand;
    public bool witch = false;

    public string[] powerUps;


    public void Fire(Transform playerTransform = null)
    {
        GameObject hit = Instantiate(slashPrefab, playerTransform);
        Slash hitScript = hit.GetComponent<Slash>();

        hitScript.SetPowerUps(powerUps);
    }

    public void Update()
    {

        if (powerUps.Contains("witch"))
        {
            witch = true;
            broomSpriteRenderer.sprite = witchSprite;
        }

        if (powerUps.Contains("wet"))
        {
            wet.SetActive(true);
        }

        if (powerUps.Contains("mop"))
        {
            mop.SetActive(true);
        }

    }
}
