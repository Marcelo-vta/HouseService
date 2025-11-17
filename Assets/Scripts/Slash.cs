using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Slash : MonoBehaviour
{
    private float timer = 0f;
    private Sprite[] activeSprites;
    private string[] powerUps;

    public SpriteRenderer spriteRenderer;
    public Sprite[] defaultSprites;
    public Sprite[] wetSprites;

    public float damage;
    public float knockback;


    void Start()
    {
        activeSprites = defaultSprites;


        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);


        int flipVertical = rotationZ < -90 || rotationZ > 90 ? -1 : 1;

        Vector3 currentScale = transform.localScale;

        currentScale.y = flipVertical;

        if (powerUps.Contains("long"))
        {
            Vector3 currentPos = transform.localPosition;

            currentScale.x *= 1.2f;
            currentScale.y *= 1.2f;
        }

        transform.localScale = currentScale; 


    }

    // Update is called once per frame
    void Update()
    {
        if (powerUps.Contains("wet"))
        {
            activeSprites = wetSprites;
        }

        if (powerUps.Contains("mop"))
        {
            damage *= 1.5f;
        }

        if (powerUps.Contains("witch"))
        {
            knockback *= 1.5f;
        }

        
        
        timer += Time.deltaTime;


        if (timer < .15f)
        {
            spriteRenderer.sprite = activeSprites[0];
        }
        else
        {
            spriteRenderer.sprite = activeSprites[1];
        }

        if (timer >= .3f)
        {
            Destroy(gameObject);
        }
    }

    public void SetPowerUps(string[] newPowerUps)
    {
        powerUps = newPowerUps;
    }
}
