using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public float deadzoneX;
    public float deadzoneY;


    private int resolutionX = 900;
    private int resolutionY = 600;

    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        transform.position = player.position;
    }
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 absoluteMousePos = Input.mousePosition - (new Vector3(resolutionX,resolutionY,0) * 0.5f);
    }

}
