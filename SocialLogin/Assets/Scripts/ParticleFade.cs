using UnityEngine;
using UnityEngine.UI;

public class ParticleFade : MonoBehaviour
{

    public float timeToPause = 2.0f;
    public float lifespan = 5.0f;

    [Header("Monobehaviours")]
    [SerializeField]
    private ParticleCollision particleCollision;
    [SerializeField]
    private FireExtinguisher fireExtinguisher;
    [SerializeField]
    private InstructionManual instructionManual;
    [SerializeField]
    private PointerAnimation fingerPointer;

    [SerializeField]
    private GameObject tick;
    [SerializeField]
    private GameObject cross;
    [SerializeField]
    private Image crosshair;

    [SerializeField]
    private AudioSource fire;

    private ParticleSystem system;
    private float time;

    private ParticleSystem.Particle[] particles;

    private void OnEnable()
    {
        system = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (particleCollision.isColliding && fireExtinguisher.isSqueezeStepCompleted)
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

                    if (color.a < 0f)
					{
                        if (!fireExtinguisher.isFireExtinguished)
						{
                            system.Stop();

                            fingerPointer.Terminate();
                            fingerPointer.gameObject.SetActive(false);

                            instructionManual.ShowInstructions("<color=red>Step 5:</color> Now once again press the lever at the top of the extinguisher to stop the smoke.");

                            if (gameObject.transform.parent.gameObject != null)
                                Destroy(gameObject.transform.parent.gameObject);

                            tick.SetActive(true);
                            cross.SetActive(false);
                            crosshair.gameObject.SetActive(false);

                            fire.Stop();
                            
                            fireExtinguisher.isFireExtinguished = true;
                        }
                    }
                }

                system.SetParticles(particles, particles.Length);
            }
        }
    }

}
