using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public List<GameObject> weapons;

    private InputAction clickAction;
    private int currentWeaponNr;

    private void Awake()
    {
        // Setup click action;
        clickAction = new InputAction(binding: "<Mouse>/leftButton");

        clickAction.started += ctx => OnClickStarted();
        clickAction.canceled += ctx => OnClickCanceled();
    }

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            currentWeaponNr = 0;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            currentWeaponNr = 1;
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            currentWeaponNr = 2;
        }
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

        weapons[currentWeaponNr].SetActive(true);
        weapons[currentWeaponNr].GetComponent<ParticleSystem>().Play();
    }

    private void OnClickCanceled()
    {
        StartCoroutine(SetInactiveAfterParticlesFinished(weapons[currentWeaponNr]));
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
