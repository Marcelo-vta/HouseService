using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowMouse : MonoBehaviour
{
    public CinemachineCamera groupCamera;
    
    public float deadzoneX;
    public float deadzoneY;

    private GameObject deadzone;
    public bool onDeadzone;

    private int resolutionX = 900;
    private int resolutionY = 600;

    void Start()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        deadzone = GetComponentInChildren<BoxCollider2D>().gameObject;
        transform.position = player.position;
    }
    void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 absoluteMousePos = Input.mousePosition - (new Vector3(resolutionX,resolutionY,0) * 0.5f);

        onDeadzone = absoluteMousePos.x > -deadzoneX 
            && absoluteMousePos.x < deadzoneX
            && absoluteMousePos.y > -deadzoneY
            && absoluteMousePos.y < deadzoneY;

        if (onDeadzone) groupCamera.Priority = 0;
        if (!onDeadzone) groupCamera.Priority = 10;
            
    }

}
