using UnityEngine;
using UnityEngine.Rendering;

public class SlashAnimation : MonoBehaviour
{
    private float timer = 0f;
    public SpriteRenderer spriteRenderer;
    public Sprite secondSprite;

    void Start()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= .15f)
        {
            spriteRenderer.sprite = secondSprite;
        }

        if (timer >= .3f)
        {
            Destroy(gameObject);
        }
    }
}
