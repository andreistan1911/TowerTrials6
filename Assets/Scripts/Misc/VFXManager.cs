using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public ParticleSystem FW;
    public ParticleSystem LW;
    public ParticleSystem FL;

    private void Play(GameObject obj, ParticleSystem ps)
    {
        ParticleSystem vfxInstance = Instantiate(ps, obj.transform.position, obj.transform.rotation);
        vfxInstance.transform.parent = obj.transform;

        vfxInstance.Play();
        StartCoroutine(DestroyAfterParticlesFinished(vfxInstance));
    }

    public void PlayFW(GameObject obj)
    {
        Debug.Log("Play vape");
        Play(obj, FW);
    }

    public void PlayLW(GameObject obj)
    {
        Play(obj, LW);
    }

    public void PlayFL(GameObject obj)
    {
        Play(obj, FL);
    }

    private IEnumerator DestroyAfterParticlesFinished(ParticleSystem particleSystem)
    {
        // Wait until the ParticleSystem has stopped emitting or the object is destroyed
        float remainingDuration = particleSystem.main.duration + particleSystem.main.startLifetime.constant;
        float timer = 0;

        while (timer <= remainingDuration)
        {
            if (particleSystem == null)
                yield break;

            if (particleSystem.isPlaying)
                yield return null;

            timer += Time.deltaTime;
        }

        if (timer > remainingDuration)
            Destroy(particleSystem.gameObject);
    }

}
