using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders (optional)")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    // Exposed parameter names (must match the names you exposed in the AudioMixer)
    public string masterParam = "Master";
    public string musicParam = "Music";
    public string sfxParam = "SFX";

    // PlayerPrefs keys
    const string MasterKey = "volume_master";
    const string MusicKey = "volume_music";
    const string SfxKey = "volume_sfx";

    void Awake()
    {
        // Load saved slider values (default 1 = full volume)
        float m = PlayerPrefs.GetFloat(MasterKey, 1f);
        float mu = PlayerPrefs.GetFloat(MusicKey, 1f);
        float s = PlayerPrefs.GetFloat(SfxKey, 1f);

        // Apply sliders if they exist and add listeners
        if (masterSlider != null)
        {
            masterSlider.value = m;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }
        else SetMasterVolume(m);

        if (musicSlider != null)
        {
            musicSlider.value = mu;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else SetMusicVolume(mu);

        if (sfxSlider != null)
        {
            sfxSlider.value = s;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        else SetSFXVolume(s);
    }

    // Callbacks for sliders (value is 0..1)
    public void SetMasterVolume(float sliderValue)
    {
        float dB = SliderToDb(sliderValue);
        audioMixer.SetFloat(masterParam, dB);
        PlayerPrefs.SetFloat(MasterKey, sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        float dB = SliderToDb(sliderValue);
        audioMixer.SetFloat(musicParam, dB);
        PlayerPrefs.SetFloat(MusicKey, sliderValue);
    }

    public void SetSFXVolume(float sliderValue)
    {
        float dB = SliderToDb(sliderValue);
        audioMixer.SetFloat(sfxParam, dB);
        PlayerPrefs.SetFloat(SfxKey, sliderValue);
    }

    float SliderToDb(float sliderValue)
    {
        // Clamp
        sliderValue = Mathf.Clamp01(sliderValue);

        // If slider is at 0, return -80 dB (practically silent).
        if (sliderValue <= 0.0001f) return -80f;

        // Logarithmic mapping: dB = 20 * log10(value)
        // This maps 1 -> 0 dB, 0.1 -> -20 dB, etc.
        float dB = 20f * Mathf.Log10(sliderValue);

        // Optionally clamp to a minimum (AudioMixer often uses -80 dB as silence)
        return Mathf.Max(dB, -80f);
    }

    // Optional: call this to reset to defaults
    public void ResetVolumes()
    {
        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetSFXVolume(1f);

        if (masterSlider != null) masterSlider.value = 1f;
        if (musicSlider != null) musicSlider.value = 1f;
        if (sfxSlider != null) sfxSlider.value = 1f;
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
