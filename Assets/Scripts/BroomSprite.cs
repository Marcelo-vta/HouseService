using System.Linq;
using UnityEngine;

public class BroomSprite : MonoBehaviour
{
    public Broom broom;
    bool changeApplied = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (broom.powerUps.Contains("long") && !changeApplied)
        {
            Vector3 currentPos = transform.localPosition;
            currentPos.y += .2f;

            transform.localPosition = currentPos;

            changeApplied = true;
        }
    }
}
