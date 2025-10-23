using UnityEngine;

public class playerRotation : MonoBehaviour
{
    private Vector2 mousePos;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;

        mousePos.x -= Screen.width/2;
        mousePos.y -= Screen.height / 2;

        mousePos.x /= Screen.width / 2;
        mousePos.y /= Screen.height / 2;

        animator.SetFloat("mouseX", mousePos.x);
        animator.SetFloat("mouseY", mousePos.y);


        Debug.Log("Mouse Screen Position (Legacy): " + mousePos);
    }
}
