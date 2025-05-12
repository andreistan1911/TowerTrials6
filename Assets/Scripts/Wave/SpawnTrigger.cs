using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class SpawnTrigger : MonoBehaviour
{
    public AbstractWaveManager waveManager;
    public GameObject towerPurchaseZones;

    private Collider triggerCollider;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        triggerCollider = GetComponent<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();

        Assert.IsNotNull(waveManager);
        Assert.IsNotNull(towerPurchaseZones);
        Assert.IsNotNull(triggerCollider);

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning("Trigger collider should be set as trigger!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CharacterController>() == null)
            return;

        if (WaveStateManager.Instance.CurrentPhase != WaveStateManager.WavePhase.BeforeWaveTime)
            return;

        StartCoroutine(HandleWaveTrigger());
    }

    private IEnumerator HandleWaveTrigger()
    {
        // Disable Spawn Trigger and purchase zones
        DisableTrigger();
        towerPurchaseZones.SetActive(false);
        if (GameObject.Find("NextWave Image") != null)
            GameObject.Find("NextWave Image").SetActive(false);

        yield return new WaitForSeconds(4f);

        waveManager.Spawn();

        // Wait until wave is ended, and then reenable
        yield return new WaitUntil(() =>
            WaveStateManager.Instance.CurrentPhase == WaveStateManager.WavePhase.BeforeWaveTime ||
            WaveStateManager.Instance.CurrentPhase == WaveStateManager.WavePhase.AfterLastWave
        );

        EnableTrigger();
        towerPurchaseZones.SetActive(true);
    }

    private void DisableTrigger()
    {
        triggerCollider.enabled = false;
        //meshRenderer.enabled = false;
    }

    private void EnableTrigger()
    {
        triggerCollider.enabled = true;
        //meshRenderer.enabled = true;
    }
}
