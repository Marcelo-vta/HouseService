// InteractableAdapter.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class InteractableAdapter : MonoBehaviour
{
    public string playerType;
    private bool isHovered;

    void Update()
    {
        if(Input.GetMouseButtonDown(1) && isHovered)
        {
            SceneManager.LoadScene(1);
        }
    }

    void OnMouseEnter()
    {
        isHovered = true;
    }

    void OnMouseExit()
    {
        isHovered = false;
    }


}