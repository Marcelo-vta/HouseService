using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleLightFlicker : MonoBehaviour
{
    private Light2D light2D;
    private float baseIntensity;
    private float timeOffset;

    [Header("Intensidade")]
    public float flickerAmount = 0.1f;
    public float flickerSpeed = 1.5f;

    [Header("Cor quente")]
    public Color colorA = new Color(1f, 0.85f, 0.65f);
    public Color colorB = new Color(1f, 0.75f, 0.5f);
    public float colorSpeed = 0.5f;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        baseIntensity = light2D.intensity;
        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, timeOffset);
        light2D.intensity = baseIntensity + (noise - 0.5f) * flickerAmount;

        float t = Mathf.PingPong(Time.time * colorSpeed, 1f);
        light2D.color = Color.Lerp(colorA, colorB, t);
    }
}
