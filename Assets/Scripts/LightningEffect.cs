using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningEffect : MonoBehaviour
{
    private Light2D light2D;

    [Header("Configuração de brilho")]
    public float normalIntensity = 3f;
    public float flashIntensity = 8f;
    public float flashDuration = 0.1f;
    public float fadeOutSpeed = 8f;

    [Header("Frequência dos relâmpagos")]
    public float minDelay = 4f;
    public float maxDelay = 12f;

    [Header("Câmera")]
    public Camera mainCamera;
    public float shakeMagnitude = 0.2f;   // intensidade do tremor
    public float shakeDuration = 0.3f;    // duração do tremor

    private Vector3 initialCamPos;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        if (light2D == null)
        {
            Debug.LogWarning("LightningEffect precisa de um Light2D!");
            enabled = false;
            return;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;

        light2D.intensity = normalIntensity;
        StartCoroutine(LightningRoutine());
    }

    System.Collections.IEnumerator LightningRoutine()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
            StartCoroutine(Flash());
        }
    }

    System.Collections.IEnumerator Flash()
    {
        // pico de clarão
        light2D.intensity = flashIntensity;

        // inicia tremor na câmera
        if (mainCamera != null)
            StartCoroutine(ShakeCamera());

        yield return new WaitForSeconds(flashDuration);

        // suaviza o retorno
        while (light2D.intensity > normalIntensity)
        {
            light2D.intensity = Mathf.Lerp(light2D.intensity, normalIntensity, Time.deltaTime * fadeOutSpeed);
            yield return null;
        }

        light2D.intensity = normalIntensity;
    }

    System.Collections.IEnumerator ShakeCamera()
    {
        initialCamPos = mainCamera.transform.localPosition;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = initialCamPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = initialCamPos;
    }
}
