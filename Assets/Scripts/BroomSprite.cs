using System.Linq;
using UnityEngine;

public class BroomSprite : MonoBehaviour
{
    bool changeApplied = false;
    private PlayerStates playerStates; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStates = GetComponentInParent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerStates.powerUps.Contains("long") && !changeApplied)
        {
            Vector3 currentPos = transform.localPosition;
            currentPos.y += .2f;

            transform.localPosition = currentPos;

            changeApplied = true;
        }
    }
}
