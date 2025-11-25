#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileUISetup : MonoBehaviour
{
    [MenuItem("Mobile/Setup Mobile UI")]
    public static void SetupMobileUI()
    {
        // 1. Create Canvas
        GameObject canvasGO = new GameObject("MobileHUD");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasGO.AddComponent<GraphicRaycaster>();

        // 2. Create EventSystem if missing
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<EventSystem>();
            es.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }

        // 3. Create Left Joystick (Movement)
        GameObject leftJoy = CreateJoystick(canvasGO.transform, "LeftJoystick", true);
        RectTransform leftRect = leftJoy.GetComponent<RectTransform>();
        leftRect.anchorMin = new Vector2(0, 0);
        leftRect.anchorMax = new Vector2(0, 0);
        leftRect.pivot = new Vector2(0, 0);
        leftRect.anchoredPosition = new Vector2(100, 100);

        // 4. Create Right Joystick (Aim)
        GameObject rightJoy = CreateJoystick(canvasGO.transform, "RightJoystick", false);
        RectTransform rightRect = rightJoy.GetComponent<RectTransform>();
        rightRect.anchorMin = new Vector2(1, 0);
        rightRect.anchorMax = new Vector2(1, 0);
        rightRect.pivot = new Vector2(1, 0);
        rightRect.anchoredPosition = new Vector2(-100, 100);

        // 5. Create Dash Button (Left of Joystick)
        GameObject dashBtn = CreateButton(canvasGO.transform, "DashButton", MobileButton.ButtonType.Dash);
        RectTransform dashRect = dashBtn.GetComponent<RectTransform>();
        dashRect.anchorMin = new Vector2(1, 0);
        dashRect.anchorMax = new Vector2(1, 0);
        dashRect.pivot = new Vector2(1, 0);
        dashRect.anchoredPosition = new Vector2(-350, 150); // Left of Right Joystick

        // 6. Create Interact Button (Above Joystick)
        GameObject interactBtn = CreateButton(canvasGO.transform, "InteractButton", MobileButton.ButtonType.Interact);
        RectTransform interactRect = interactBtn.GetComponent<RectTransform>();
        interactRect.anchorMin = new Vector2(1, 0);
        interactRect.anchorMax = new Vector2(1, 0);
        interactRect.pivot = new Vector2(1, 0);
        interactRect.anchoredPosition = new Vector2(-150, 350); // Above Right Joystick

        Debug.Log("Mobile UI generated successfully!");
    }

    private static GameObject CreateButton(Transform parent, string name, MobileButton.ButtonType type)
    {
        GameObject btnGO = new GameObject(name);
        btnGO.transform.SetParent(parent, false);

        Image bg = btnGO.AddComponent<Image>();
        bg.color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
        RectTransform rect = btnGO.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(100, 100);

        MobileButton btn = btnGO.AddComponent<MobileButton>();
        btn.buttonType = type;

        // Add text label
        GameObject textGO = new GameObject("Label");
        textGO.transform.SetParent(btnGO.transform, false);
        Text text = textGO.AddComponent<Text>();
        text.text = type.ToString();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.black;
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return btnGO;
    }

    private static GameObject CreateJoystick(Transform parent, string name, bool isLeft)
    {
        GameObject joyGO = new GameObject(name);
        joyGO.transform.SetParent(parent, false);
        
        // Background
        Image bg = joyGO.AddComponent<Image>();
        bg.color = new Color(1, 1, 1, 0.3f); // Semi-transparent white
        RectTransform bgRect = joyGO.GetComponent<RectTransform>();
        bgRect.sizeDelta = new Vector2(200, 200);

        // Handle
        GameObject handleGO = new GameObject("Handle");
        handleGO.transform.SetParent(joyGO.transform, false);
        Image handleImg = handleGO.AddComponent<Image>();
        handleImg.color = new Color(1, 1, 1, 0.8f);
        RectTransform handleRect = handleGO.GetComponent<RectTransform>();
        handleRect.sizeDelta = new Vector2(100, 100);

        // Script
        MobileJoystick joystick = joyGO.AddComponent<MobileJoystick>();
        
        // Use SerializedObject to set private fields if needed, or just rely on public/SerializeField
        // Since we can't easily set private serialized fields via code without SerializedObject, 
        // let's assume the user might need to assign them or we change MobileJoystick to find them.
        // Better: Change MobileJoystick to be more robust or use SerializedObject here.
        
        SerializedObject so = new SerializedObject(joystick);
        so.FindProperty("background").objectReferenceValue = bgRect;
        so.FindProperty("handle").objectReferenceValue = handleRect;
        so.FindProperty("handleRange").floatValue = 1f;
        so.FindProperty("isLeftJoystick").boolValue = isLeft;
        so.ApplyModifiedProperties();

        return joyGO;
    }
}
#endif
