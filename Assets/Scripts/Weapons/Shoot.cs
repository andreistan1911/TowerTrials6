using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public List<GameObject> weapons;

    private InputAction clickAction;
    private int currentWeaponNr;
    private bool isClicking;
    private AudioSource audioSource;
    public AudioSource fireSource;
    public AudioSource waterSource;
    public AudioSource lightningSource;
    private bool hasStarted = false;

    private void Awake()
    {
        StartCoroutine(EnableInputAfterDelay());
        audioSource = GetComponent<AudioSource>();
        // Setup click action;
        isClicking = false;
        clickAction = new InputAction(binding: "<Mouse>/leftButton");
        Global.LockCursor();
    }

    private IEnumerator EnableInputAfterDelay()
    {
        yield return null; // wait one frame
        hasStarted = true;

        // Only start listening for input after the delay
        clickAction.started += ctx => OnClickStarted();
        clickAction.canceled += ctx => OnClickCanceled();
    }



    private void Update()
    {
        if (Cursor.lockState==CursorLockMode.None)
        {
            return;
        }

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            weapons[1].SetActive(false);
            weapons[2].SetActive(false);
            StopWeaponAudio(1);
            StopWeaponAudio(2);
            currentWeaponNr = 0;

        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            weapons[0].SetActive(false);
            weapons[2].SetActive(false);
            StopWeaponAudio(0);
            StopWeaponAudio(2);
            currentWeaponNr = 1;

        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            weapons[0].SetActive(false);
            weapons[1].SetActive(false);
            StopWeaponAudio(0);
            StopWeaponAudio(1);
            currentWeaponNr = 2;
        }

        if (isClicking && currentWeaponNr == 2)
            weapons[2].GetComponent<HitscanWeapon>().Fire();
    }

    private void PlayWeaponAudio(int weaponIndex)
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            return;
        }

        switch (weaponIndex)
        {
            case 0:
                if (fireSource != null && !fireSource.isPlaying) fireSource.Play();
                break;
            case 1:
                if (waterSource != null && !waterSource.isPlaying) waterSource.Play();
                break;
            case 2:
                if (lightningSource != null && !lightningSource.isPlaying) lightningSource.Play();
                break;
        }
    }

    private void StopWeaponAudio(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                if (fireSource != null && fireSource.isPlaying) fireSource.Stop();
                break;
            case 1:
                if (waterSource != null && waterSource.isPlaying) waterSource.Stop();
                break;
            case 2:
                if (lightningSource != null && lightningSource.isPlaying) lightningSource.Stop();
                break;
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
        if (!hasStarted) return;
        if (weapons.Count == 0)
            return;

        isClicking = true;

        weapons[currentWeaponNr].SetActive(true);

        if (currentWeaponNr != 2)
        { // a bit hardcoded but it's fine
            weapons[currentWeaponNr].GetComponent<ParticleSystem>().Play();
            PlayWeaponAudio(currentWeaponNr);
        }
        else
        {
            weapons[2].GetComponent<HitscanWeapon>().Fire();
            PlayWeaponAudio(currentWeaponNr);
        }
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
        {
            weapons[2].SetActive(false);
            StopWeaponAudio(currentWeaponNr);
        }
    }

    private IEnumerator SetInactiveAfterParticlesFinished(GameObject weapon)
    {
        weapons[currentWeaponNr].GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        StopWeaponAudio(currentWeaponNr);

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
