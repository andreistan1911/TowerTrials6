using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public List<GameObject> weapons;

    private InputAction clickAction;
    private int currentWeaponNr;
    private bool isClicking;

    private void Awake()
    {
        // Setup click action;
        isClicking = false;
        clickAction = new InputAction(binding: "<Mouse>/leftButton");

        clickAction.started += ctx => OnClickStarted();
        clickAction.canceled += ctx => OnClickCanceled();

        Global.LockCursor();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
            currentWeaponNr = 0;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            weapons[0].SetActive(false);
            weapons[2].SetActive(false);
            currentWeaponNr = 1;
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            currentWeaponNr = 2;
        }

        if (isClicking && currentWeaponNr == 2)
            weapons[2].GetComponent<HitscanWeapon>().Fire();
    }

    private void OnEnable()
    {
        clickAction.Enable();
        InputSystem.EnableDevice(Keyboard.current);
    }

    private void OnDisable()
    {
        clickAction.Disable();
        InputSystem.DisableDevice(Keyboard.current);
    }

    private void OnClickStarted()
    {
        if (weapons.Count == 0)
            return;

        isClicking = true;

        weapons[currentWeaponNr].SetActive(true);

        if (currentWeaponNr != 2) // a bit hardcoded but it's fine
            weapons[currentWeaponNr].GetComponent<ParticleSystem>().Play();
        else
            weapons[2].GetComponent<HitscanWeapon>().Fire();
    }

    private void OnClickCanceled()
    {
        isClicking = false;

        if (currentWeaponNr != 2) // a bit hardcoded but it's fine
        {
            if (gameObject.activeSelf) // when you lose/win, the routine would have still happened otherwise
                StartCoroutine(SetInactiveAfterParticlesFinished(weapons[currentWeaponNr]));
        }
        else
            weapons[2].SetActive(false);
    }

    private IEnumerator SetInactiveAfterParticlesFinished(GameObject weapon)
    {
        weapons[currentWeaponNr].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);

        ParticleSystem particleSystem = weapon.GetComponent<ParticleSystem>();
        float remainingDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;
        float timer = 0;

        while (timer <= remainingDuration || particleSystem.isPlaying)
        {
            if (particleSystem.isPlaying)
                yield return null;

            timer += Time.deltaTime;
        }

        if (timer > remainingDuration)
            weapon.SetActive(false);
    }
}
