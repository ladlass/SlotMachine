using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

namespace SlotMachine
{
    public class SlotFxComponent : MonoBehaviour
    {
        [SerializeField] private ParticleSystem fxPrefab;
        [SerializeField] private float fxDuration = 3;
        [SerializeField] private Transform fxParent;
        private List<ParticleSystem> particlePool;
        private WaitForSeconds secsToPool = new WaitForSeconds(4);

        public void AwakeComponent()
        {
            particlePool = new List<ParticleSystem>();
            
            ParticleSystem particle = Instantiate(fxPrefab, fxParent);
            particle.gameObject.SetActive(false);
            particlePool.Add(particle);
        }
        public void SpawnCoinFx(int amountMultiplier)
        {
            if (fxPrefab == null || fxParent == null) return;

            ParticleSystem particle = CreateParticle();
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
        }

        private IEnumerator PoolParticle(ParticleSystem particle)
        {
            yield return secsToPool;

            particle.gameObject.SetActive(false);
            particlePool.Add(particle);
        }

        private ParticleSystem CreateParticle()
        {
            if (particlePool == null) return null;

            if(particlePool.Count > 0)
            {
                ParticleSystem newParticle = particlePool[0];
                particlePool.RemoveAt(0);
                newParticle.gameObject.SetActive(true);
                StartCoroutine(PoolParticle(newParticle));
                return newParticle;
            }
            ParticleSystem cloneParticle = Instantiate(fxPrefab, fxParent);
            particlePool.Add(cloneParticle);

            return cloneParticle;
        }

        public float GetDuration()
        {
            return fxDuration;
        }
    }
}