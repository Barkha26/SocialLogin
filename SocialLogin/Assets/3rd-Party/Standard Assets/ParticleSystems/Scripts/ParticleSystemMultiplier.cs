using UnityEngine;

namespace UnityStandardAssets.Effects
{

    public class ParticleSystemMultiplier : MonoBehaviour
    {
        // a simple script to scale the size, speed and lifetime of a particle system

        public float multiplier = 1;


        private void Start()
        {
            var systems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < systems.Length; i++)
			{
                ParticleSystem.MainModule mainModule = systems[i].main;
                mainModule.startSizeMultiplier *= multiplier;
                mainModule.startSpeedMultiplier *= multiplier;
                mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
                systems[i].Clear();
                systems[i].Play();
            }
        }
    }

}
