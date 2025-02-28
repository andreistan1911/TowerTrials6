using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public ParticleSystem FW;
    public ParticleSystem FN;
    public ParticleSystem LW;
    public ParticleSystem NW;

    private void Play(GameObject obj, ParticleSystem ps)
    {
        ParticleSystem vfxInstance = Instantiate(ps, obj.transform.position, obj.transform.rotation);

        vfxInstance.transform.parent = obj.transform;

        vfxInstance.Play();
        StartCoroutine(DestroyAfterParticlesFinished(vfxInstance));
    }

    public void PlayFW(GameObject obj)
    {
        Play(obj, FW);
    }

    public void PlayFN(GameObject obj)
    {
        Play(obj, FN);
    }

    public void PlayLW(GameObject obj)
    {
        Play(obj, LW);
    }

    public void PlayNW(GameObject obj)
    {
        Play(obj, NW);
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
