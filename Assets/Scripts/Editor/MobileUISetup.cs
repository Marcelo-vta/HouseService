using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileUISetup : EditorWindow
{
    [MenuItem("Tools/Create Mobile UI")]
    public static void CreateMobileUI()
    {
        // 1. Create GameInput if missing
        GameInput gameInput = FindFirstObjectByType<GameInput>();
        if (gameInput == null)
        {
            GameObject go = new GameObject("GameInput");
            gameInput = go.AddComponent<GameInput>();
            Debug.Log("Created GameInput object. PLEASE ASSIGN THE INPUT ACTION ASSET MANUALLY.");
            Selection.activeGameObject = go;
        }

        // 2. Create Canvas
        GameObject canvasObj = new GameObject("MobileHUD");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();
        
        MobileUIManager uiManager = canvasObj.AddComponent<MobileUIManager>();
        // Use SerializedObject to assign private field
        SerializedObject so = new SerializedObject(uiManager);
        so.FindProperty("mobileCanvas").objectReferenceValue = canvasObj;
        so.ApplyModifiedProperties();

        // 3. Create EventSystem if missing
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }

        // Helper to create joystick
        CreateJoystick(canvasObj.transform, "LeftJoystick", VirtualJoystick.JoystickType.Movement, new Vector2(250, 250), Anchor.BottomLeft);
        CreateJoystick(canvasObj.transform, "RightJoystick", VirtualJoystick.JoystickType.Aim, new Vector2(-250, 250), Anchor.BottomRight);

        // Helper to create button
        CreateButton(canvasObj.transform, "DashButton", VirtualButton.ButtonType.Dash, new Vector2(-450, 150), Anchor.BottomRight);
        CreateButton(canvasObj.transform, "InteractButton", VirtualButton.ButtonType.Interact, new Vector2(-600, 150), Anchor.BottomRight);

        Debug.Log("Mobile UI Created Successfully!");
    }

    private enum Anchor { BottomLeft, BottomRight }

    private static void CreateJoystick(Transform parent, string name, VirtualJoystick.JoystickType type, Vector2 pos, Anchor anchor)
    {
        GameObject bgObj = new GameObject(name);
        bgObj.transform.SetParent(parent, false);
        Image bgImage = bgObj.AddComponent<Image>();
        bgImage.color = new Color(1, 1, 1, 0.3f); // Semi-transparent white
        
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(300, 300);
        SetAnchor(bgRect, anchor);
        bgRect.anchoredPosition = pos;

        GameObject handleObj = new GameObject("Handle");
        handleObj.transform.SetParent(bgObj.transform, false);
        Image handleImage = handleObj.AddComponent<Image>();
        handleImage.color = new Color(1, 1, 1, 0.8f); // Opaque white
        
        RectTransform handleRect = handleObj.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(100, 100);

        VirtualJoystick joystick = bgObj.AddComponent<VirtualJoystick>();
        // Use SerializedObject to assign private fields
        SerializedObject so = new SerializedObject(joystick);
        so.FindProperty("joystickType").enumValueIndex = (int)type;
        so.FindProperty("background").objectReferenceValue = bgRect;
        so.FindProperty("handle").objectReferenceValue = handleRect;
        so.FindProperty("handleRange").floatValue = 100f;
        so.ApplyModifiedProperties();
    }

    private static void CreateButton(Transform parent, string name, VirtualButton.ButtonType type, Vector2 pos, Anchor anchor)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);
        Image btnImage = btnObj.AddComponent<Image>();
        btnImage.color = new Color(1, 0, 0, 0.5f); // Semi-transparent red

        RectTransform btnRect = btnObj.GetComponent<RectTransform>();
        btnRect.sizeDelta = new Vector2(120, 120);
        SetAnchor(btnRect, anchor);
        btnRect.anchoredPosition = pos;

        // Add Text Label
        GameObject textObj = new GameObject("Label");
        textObj.transform.SetParent(btnObj.transform, false);
        Text text = textObj.AddComponent<Text>();
        text.text = (type == VirtualButton.ButtonType.Interact) ? "E" : "D";
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.fontSize = 60;
        text.resizeTextForBestFit = true;
        text.resizeTextMinSize = 40;
        text.resizeTextMaxSize = 80;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        VirtualButton btn = btnObj.AddComponent<VirtualButton>();
        SerializedObject so = new SerializedObject(btn);
        so.FindProperty("buttonType").enumValueIndex = (int)type;
        so.ApplyModifiedProperties();
    }

    private static void SetAnchor(RectTransform rect, Anchor anchor)
    {
        if (anchor == Anchor.BottomLeft)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
            rect.pivot = Vector2.zero;
        }
        else
        {
            rect.anchorMin = Vector2.right;
            rect.anchorMax = Vector2.right;
            rect.pivot = Vector2.right;
        }
    }
}
