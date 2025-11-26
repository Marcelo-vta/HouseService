using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InputManager");
                    _instance = go.AddComponent<InputManager>();
                }
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool FireInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool InteractInput { get; private set; }

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction fireAction;
    private InputAction dashAction;
    private InputAction interactAction;

    // Mobile Virtual Joysticks (set by MobileJoystick scripts)
    public Vector2 VirtualMoveInput { get; set; }
    public Vector2 VirtualLookInput { get; set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeActions();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeActions()
    {
        // Move Action (WASD, Left Stick)
        moveAction = new InputAction("Move", binding: "<Gamepad>/leftStick");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        // Look Action (Mouse Delta, Right Stick)
        lookAction = new InputAction("Look", binding: "<Gamepad>/rightStick");
        lookAction.AddBinding("<Pointer>/delta");

        // Fire Action (Mouse Left Click, Gamepad Right Trigger)
        fireAction = new InputAction("Fire", binding: "<Gamepad>/rightTrigger");
        fireAction.AddBinding("<Mouse>/leftButton");

        // Dash Action (Space, Gamepad South/A)
        dashAction = new InputAction("Dash", binding: "<Gamepad>/buttonSouth");
        dashAction.AddBinding("<Keyboard>/space");

        // Interact Action (E, Gamepad West/X)
        interactAction = new InputAction("Interact", binding: "<Gamepad>/buttonWest");
        interactAction.AddBinding("<Keyboard>/e");
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        lookAction?.Enable();
        fireAction?.Enable();
        dashAction?.Enable();
        interactAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        lookAction?.Disable();
        fireAction?.Disable();
        dashAction?.Disable();
        interactAction?.Disable();
    }

    private void Update()
    {
        // Combine Physical Input with Virtual Input
        Vector2 physicalMove = moveAction.ReadValue<Vector2>();
        MoveInput = physicalMove != Vector2.zero ? physicalMove : VirtualMoveInput;

        Vector2 physicalLook = lookAction.ReadValue<Vector2>();
        LookInput = physicalLook != Vector2.zero ? physicalLook : VirtualLookInput;

        // Fire Logic: Physical Button OR Right Joystick (Auto-Fire)
        bool isUsingJoystickAim = VirtualLookInput.sqrMagnitude > 0.01f;
        FireInput = fireAction.IsPressed() || isUsingJoystickAim;

        // One-shot buttons: Physical OR Mobile Flag
        DashInput = dashAction.WasPressedThisFrame() || _mobileDashPressed;
        InteractInput = interactAction.WasPressedThisFrame() || _mobileInteractPressed;

        // Reset flags after consumption
        _mobileDashPressed = false;
        _mobileInteractPressed = false;
    }

    // Methods for Mobile UI Buttons to call
    public void MobileFire(bool pressed) => FireInput = pressed; 
    
    private bool _mobileDashPressed;
    private bool _mobileInteractPressed;

    public void TriggerMobileDash() => _mobileDashPressed = true;
    public void TriggerMobileInteract() => _mobileInteractPressed = true;
}
