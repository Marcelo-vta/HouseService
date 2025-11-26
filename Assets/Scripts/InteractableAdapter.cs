// InteractableAdapter.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class InteractableAdapter : MonoBehaviour
{
    public GameObject playerType;
    public GameObject hoverBg;

    private bool isHovered;
    private bool started = false;


    void Update()
    {
        if(Input.GetMouseButtonDown(0) && isHovered && !started)
        {
            // StartCoroutine(StartGame());
            started = true;
            GameObject player = Instantiate(playerType);
            SceneManager.LoadScene(1);
        }
    }

    // IEnumerator StartGame(){

    //     print("click!");
        
    //     started = true;
    //     GameObject player = Instantiate(playerType);
    //     SceneManager.LoadScene(1);
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mouse"))
        {
            isHovered = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Mouse"))
        {
            isHovered = false;
        }
    }

}