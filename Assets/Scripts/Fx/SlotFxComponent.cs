using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace SlotMachine
{
    public class SlotFxComponent : MonoBehaviour
    {
        [SerializeField] private GameObject fxPrefab;
        [SerializeField] private float fxDuration = 3;
        [SerializeField] private Transform fxParent;

        public void SpawnCoinFx(int amountMultiplier)
        {
            if (fxPrefab == null || fxParent == null) return;

            GameObject cloneFx = Instantiate(fxPrefab, fxParent);
            ParticleSystem particle = cloneFx.GetComponent<ParticleSystem>();
            if (particle)
            {
                EmissionModule emissionModule = particle.emission;

                for (int i = 0; i < emissionModule.burstCount; i++)
                {
                    Burst burst = emissionModule.GetBurst(i);
                    burst.count = amountMultiplier;
                    emissionModule.SetBurst(i, burst);
                }

            }
            Destroy(cloneFx, 10);
        }

        public float GetDuration()
        {
            return fxDuration;
        }
    }
}