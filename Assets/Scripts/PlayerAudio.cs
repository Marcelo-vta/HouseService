using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerStates))]
public class PlayerAudio : MonoBehaviour
{
    [Header("Referências")]
    public PlayerStates playerStates;

    [Tooltip("Fonte para efeitos sonoros (SFX).")]
    public AudioSource sfxSource;

    [Tooltip("Fonte para passos.")]
    public AudioSource footstepsSource;

    [Tooltip("Fonte para música de fundo.")]
    public AudioSource musicSource;

    [Header("Passos")]
    public AudioClip[] footstepsClips;
    public float baseStepInterval = 0.35f;
    public float minStepInterval = 0.2f;

    [Header("SFX de Baú / Itens")]
    public AudioClip openChestClip;     // LOOP enquanto interactingState = true
    public AudioClip obtainItemClip;    // som de pegar item
    public AudioClip placeItemClip;     // som de guardar item

    [Header("SFX Gerais / Estados")]
    public AudioClip hurtClip;
    public AudioClip scareClip;
    public AudioClip rollClip;

    [Header("Ataque")]
    public AudioClip cleanerAttackClip;
    public AudioClip pizzaGuyAttackClip;

    [Header("Músicas por Insanidade")]
    public AudioClip insanity0Music;
    public AudioClip insanity1Music;
    public AudioClip insanity2Music;

    [Header("Ajustes")]
    [Range(0f, 0.3f)] public float pitchVariation = 0.1f;

    // estados anteriores
    private bool wasObtaining;
    private bool wasHurt;
    private bool wasScared;
    private bool wasRolling;

    private bool lastAttackState = false;
    private float stepTimer = 0f;
    private int lastInsanityLevel = -1;

    void Awake()
    {
        if (!playerStates)
            playerStates = GetComponent<PlayerStates>();

        if (!sfxSource)
            sfxSource = GetComponent<AudioSource>();

        if (!footstepsSource)
            footstepsSource = sfxSource;

        if (!musicSource)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }
    }

    void Start()
    {
        UpdateMusicImmediate();
    }

    void Update()
    {
        HandleChestOpeningLoop();   // **AGORA EM INTERACTING STATE**
        HandleFootsteps();
        HandleStateTransitions();
        HandleAttackSound();
        SavePreviousStates();
    }

    // ──────────────────────────────────────────────
    //  EFEITO DE ABRIR O BAÚ (EM INTERACTING STATE)
    // ──────────────────────────────────────────────
    void HandleChestOpeningLoop()
    {
        if (playerStates.interactingState)
        {
            // começar o loop do som de baú
            if (!sfxSource.isPlaying || sfxSource.clip != openChestClip)
            {
                if (openChestClip)
                {
                    sfxSource.clip = openChestClip;
                    sfxSource.loop = true;
                    sfxSource.volume = 1f;
                    sfxSource.Play();
                }
            }
        }
        else
        {
            // parar imediatamente ao sair do estado
            if (sfxSource.clip == openChestClip && sfxSource.isPlaying)
            {
                sfxSource.Stop();
                sfxSource.loop = false;
                sfxSource.clip = null;
            }
        }
    }

    // ──────────────────────────────────────────────
    //  PASSOS
    // ──────────────────────────────────────────────
    void HandleFootsteps()
    {
        bool isWalking =
            playerStates.walkingState &&
            playerStates.ableToWalk &&
            !playerStates.rollingState &&
            !playerStates.scaredState &&
            !playerStates.interactingState;

        if (isWalking)
        {
            float interval = Mathf.Max(baseStepInterval, minStepInterval);

            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = interval;
            }
        }
        else
        {
            stepTimer = 0f;

            if (footstepsSource && footstepsSource.isPlaying)
                footstepsSource.Stop();
        }
    }

    void PlayFootstep()
    {
        if (footstepsClips.Length == 0) return;
        if (!footstepsSource) return;

        footstepsSource.pitch = 1f + Random.Range(-pitchVariation, pitchVariation);

        footstepsSource.clip = footstepsClips[
            Random.Range(0, footstepsClips.Length)
        ];

        footstepsSource.loop = false;
        footstepsSource.Play();
    }

    // ──────────────────────────────────────────────
    //  TRANSIÇÕES DE ESTADO
    // ──────────────────────────────────────────────
    void HandleStateTransitions()
    {
        // PEGAR ITEM
        if (playerStates.obtainingState && !wasObtaining)
        {
            // tocar pegar item
            PlaySFX(obtainItemClip);

            // tocar "guardar" depois
            if (obtainItemClip && placeItemClip)
            {
                StopCoroutine(nameof(PlayPlaceItemAfterPickup));
                StartCoroutine(PlayPlaceItemAfterPickup());
            }
        }

        // DANO
        if (playerStates.hurtState && !wasHurt)
            PlaySFX(hurtClip);

        // SUSTO → som → trocar música depois
        if (playerStates.scaredState && !wasScared)
        {
            PlaySFX(scareClip);

            int newLevel = GetInsanityLevel();
            if (newLevel != lastInsanityLevel)
            {
                float delay = scareClip ? scareClip.length : 0f;
                StopCoroutine(nameof(ChangeMusicAfterDelay));
                StartCoroutine(ChangeMusicAfterDelay(delay));
            }
        }

        // ROLL
        if (playerStates.rollingState && !wasRolling)
            PlaySFX(rollClip);
    }

    void SavePreviousStates()
    {
        wasObtaining = playerStates.obtainingState;
        wasHurt = playerStates.hurtState;
        wasScared = playerStates.scaredState;
        wasRolling = playerStates.rollingState;
    }

    // ──────────────────────────────────────────────
    //  SOM DE GUARDAR ITEM (DEPOIS DO SOM DE PEGAR)
    // ──────────────────────────────────────────────
    IEnumerator PlayPlaceItemAfterPickup()
    {
        yield return new WaitForSeconds(obtainItemClip.length);
        PlaySFX(placeItemClip);
    }

    void PlaySFX(AudioClip clip)
    {
        if (!clip || !sfxSource) return;
        sfxSource.pitch = 1 + Random.Range(-pitchVariation, pitchVariation);
        sfxSource.PlayOneShot(clip);
    }

    // ──────────────────────────────────────────────
    //  ATAQUE
    // ──────────────────────────────────────────────
    void HandleAttackSound()
    {
        bool attackPressed =
            Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Mouse0) ||
            Input.GetKeyDown(KeyCode.J);

        if (attackPressed && !lastAttackState && playerStates.handsUsable)
        {
            if (playerStates.cleaner)
                PlaySFX(cleanerAttackClip);
            else if (playerStates.pizzaGuy)
                PlaySFX(pizzaGuyAttackClip);
        }

        lastAttackState = attackPressed;
    }

    // ──────────────────────────────────────────────
    //  MÚSICA DINÂMICA
    // ──────────────────────────────────────────────
    int GetInsanityLevel()
    {
        return playerStates.insanity < 1 ? 0 :
               playerStates.insanity < 2 ? 1 : 2;
    }

    void UpdateMusicImmediate()
    {
        lastInsanityLevel = GetInsanityLevel();
        AudioClip newClip = ChooseMusic(lastInsanityLevel);

        if (newClip != null)
        {
            musicSource.clip = newClip;
            musicSource.volume = 1f;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    AudioClip ChooseMusic(int lvl)
    {
        if (lvl == 0) return insanity0Music;
        if (lvl == 1) return insanity1Music;
        return insanity2Music;
    }

    IEnumerator ChangeMusicAfterDelay(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        int lvl = GetInsanityLevel();
        lastInsanityLevel = lvl;

        AudioClip newClip = ChooseMusic(lvl);

        if (newClip != null && musicSource.clip != newClip)
        {
            musicSource.clip = newClip;
            musicSource.volume = 1f;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
