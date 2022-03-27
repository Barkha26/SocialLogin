using UnityEngine;

public class ParticleLoop : MonoBehaviour
{

    public float timeToPause = 2.0f;
    public float lifespan = 5.0f;

    private ParticleSystem system;
    private float time;
    private bool isFadedOut;

    private ParticleSystem.Particle[] particles;

    [SerializeField]
    private FireExtinguisher fireExtinguisher;

    [System.Obsolete]
	private void OnEnable()
    {
        isFadedOut = false;

        system = GetComponent<ParticleSystem>();
		GetComponent<ParticleSystem>().loop = false;
    }

    private void Update()
    {
        time += Time.deltaTime;

        if (!system.isPaused && time > timeToPause)
        {
            particles = new ParticleSystem.Particle[system.particleCount];
            system.GetParticles(particles);
        }

        if (particles != null)
        {
            for (int i = 0; i < particles.Length; i++)
            {
                Color color = particles[i].startColor;
                color.a = ((lifespan - time + timeToPause) / lifespan);

                particles[i].startColor = color;

                if (color.a <= 0f)
                {
                    if (!isFadedOut)
					{
                        system.Stop();

                        fireExtinguisher.audioSource.loop = false;
                        fireExtinguisher.audioSource.Stop();

                        isFadedOut = true;
                    }
                }
            }

            system.SetParticles(particles, particles.Length);
        }
    }
}
