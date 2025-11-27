using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private InputActionAsset inputActionAsset;

    [Header("Debug")]
    [SerializeField] private bool simulateMobile = false;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction attackAction;
    private InputAction interactAction;
    private InputAction jumpAction; // Used for dash
    
    private Vector2 virtualMovementInput;
    private Vector2 virtualAimInput;
    private bool isVirtualFiring;
    private bool isVirtualDashing;
    private bool isVirtualInteracting;

    private bool isMobileInputActive;

    public bool IsMobileMode
    {
        get
        {
#if UNITY_EDITOR
            if (simulateMobile) return true;
#endif
            return Application.isMobilePlatform;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (inputActionAsset == null)
        {
            Debug.LogError("GameInput: InputActionAsset is not assigned!");
            return;
        }

        var playerMap = inputActionAsset.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        lookAction = playerMap.FindAction("Look");
        attackAction = playerMap.FindAction("Attack");
        interactAction = playerMap.FindAction("Interact");
        jumpAction = playerMap.FindAction("Jump");

        playerMap.Enable();
    }

    private void Update()
    {
        // Reset mobile active state if no virtual input is happening
        if (virtualMovementInput == Vector2.zero && 
            virtualAimInput == Vector2.zero && 
            !isVirtualFiring && 
            !isVirtualDashing && 
            !isVirtualInteracting)
        {
            isMobileInputActive = false;
        }
    }

    public Vector2 GetMovement()
    {
        if (IsMobileActive())
        {
            return virtualMovementInput;
        }
        return moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
    }

    public Vector2 GetAim()
    {
        if (IsMobileActive())
        {
            return virtualAimInput;
        }
        return lookAction != null ? lookAction.ReadValue<Vector2>() : Vector2.zero;
    }

    public Vector2 GetPointerPosition()
    {
        if (Mouse.current != null)
        {
            return Mouse.current.position.ReadValue();
        }
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            return Touchscreen.current.primaryTouch.position.ReadValue();
        }
        return Vector2.zero;
    }

    public bool IsPointerDown()
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed) return true;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) return true;
        return false;
    }

    public bool IsPointerDownThisFrame()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) return true;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame) return true;
        return false;
    }

    public bool IsFiring()
    {
        if (IsMobileActive())
        {
            return isVirtualFiring;
        }
        return attackAction != null && attackAction.IsPressed();
    }

    public bool IsDashing()
    {
        if (IsMobileActive())
        {
            return isVirtualDashing;
        }
        return jumpAction != null && jumpAction.WasPressedThisFrame();
    }

    public bool IsInteracting()
    {
        if (IsMobileActive())
        {
            return isVirtualInteracting;
        }
        return interactAction != null && interactAction.IsPressed();
    }

    public bool IsMobileActive()
    {
        return IsMobileMode || isMobileInputActive;
    }

    // Mobile UI Setters
    public void SetVirtualMovement(Vector2 input)
    {
        virtualMovementInput = input;
        if (input != Vector2.zero) isMobileInputActive = true;
    }

    public void SetVirtualAim(Vector2 input)
    {
        virtualAimInput = input;
        if (input != Vector2.zero) isMobileInputActive = true;
    }

    public void SetVirtualFiring(bool firing)
    {
        isVirtualFiring = firing;
        if (firing) isMobileInputActive = true;
    }

    public void SetVirtualDashing(bool dashing)
    {
        isVirtualDashing = dashing;
        if (dashing) isMobileInputActive = true;
    }

    public void SetVirtualInteracting(bool interacting)
    {
        isVirtualInteracting = interacting;
        if (interacting) isMobileInputActive = true;
    }
}
