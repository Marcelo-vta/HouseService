using System.Data.Common;
using UnityEditor.Animations;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [Header("Character Animators")]
    public RuntimeAnimatorController pizzaGuyAnimator;
    public RuntimeAnimatorController cleaningGuyAnimator;

    [Header("Character Weapons")]
    public GameObject pizzaBox;
    public GameObject broom;

    private GameObject selectedWeapon;
    private string lastCharacter = "start";

    private GameObject weapons;
    private Animator animator;
    private PlayerStates playerStates;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weapons = GetComponentInChildren<WeaponControl>().gameObject;
        animator = GetComponentInChildren<Animator>();
        playerStates = GetComponent<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerStates.characterType)
        {
            case "pizzaGuy":
                animator.runtimeAnimatorController = pizzaGuyAnimator;
                selectedWeapon = pizzaBox;
                break;
            case "cleaner":
                animator.runtimeAnimatorController = cleaningGuyAnimator;
                selectedWeapon = broom;
                break;
            case "default":
                print("Invalid character selected");
                selectedWeapon = null;
                break;
        }

        if (lastCharacter == "start")
        {
            lastCharacter = playerStates.characterType;
            Instantiate(selectedWeapon, weapons.transform);
        }

        if (lastCharacter != playerStates.characterType)
        {
            lastCharacter = playerStates.characterType;
            Destroy(weapons.transform.GetChild(0).gameObject);
            Instantiate(selectedWeapon, weapons.transform);
        }
    }
}
