using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FireExtinguisher : MonoBehaviour
{
	 
	[Header("Particle Systems")]
    [SerializeField]
    private ParticleSystem pressurisedSteam;
    [SerializeField]
    private ParticleSystem gas;

	[Header("Monobehaviours")]
	[SerializeField]
	private ParticleLoop particleLoopPressurisedSteam;
	[SerializeField]
	private ParticleLoop particleLoopGas;
	[SerializeField]
	private InstructionManual instructionManual;
	[SerializeField]
	private PointerAnimation fingerPointer;
	[SerializeField]
	private ParticleCollision particleCollision;

	[SerializeField]
	private GameObject tick;
	[SerializeField]
	private GameObject cross;
	[SerializeField]
	private Image crosshair;

	public AudioSource audioSource;

	[HideInInspector]
	public bool isPullStepCompleted;
	[HideInInspector]
	public bool isAimStepCompleted;
	[HideInInspector]
	public bool isSqueezeStepCompleted;
	[HideInInspector]
	public bool isSweepStepCompleted;
	[HideInInspector]
	public bool isFireExtinguished;

	public float rotatespeed = 10f;
	
	private float _startingPosition;
	private float _movingPosition;
	
	private void Awake()
	{
		InitializeOnLoad();
	}

	private void InitializeOnLoad()
	{
		isPullStepCompleted = isAimStepCompleted = isSqueezeStepCompleted = isSweepStepCompleted = isFireExtinguished = false;
		
		fingerPointer.Terminate();
		fingerPointer.gameObject.SetActive(false);

		tick.SetActive(false);

		crosshair.color = Color.red;
		crosshair.gameObject.SetActive(false);

		StopParticles();
	}

	private void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
			{
				if (!isPullStepCompleted)
				{
					if (hit.collider.gameObject.CompareTag("Pin"))
						StartCoroutine(AnimatePin(hit.collider.gameObject.transform));
				}

				if (isPullStepCompleted && isAimStepCompleted && !isSqueezeStepCompleted)
				{
					if (hit.collider.gameObject.CompareTag("Squeezer"))
						StartCoroutine(AnimateSqueezer(hit.collider.gameObject.transform));
				}

				if (isFireExtinguished)
				{
					if (hit.collider.gameObject.CompareTag("Squeezer"))
						StartCoroutine(AnimateSqueezer(hit.collider.gameObject.transform));
				}
			}
		}

		if (Input.GetMouseButton(0))
		{
			if (isPullStepCompleted || isSqueezeStepCompleted)
			{
				RotateAroundAxis();

				if (isPullStepCompleted)
				{
					if (!isAimStepCompleted)
					{
						if (particleCollision.isColliding)
						{
							fingerPointer.Terminate();
							fingerPointer.gameObject.SetActive(false);

							instructionManual.ShowInstructions("<color=red>Step 3:</color> S - Squeeze the lever at the top of the extinguisher.");
							instructionManual.ShowAfterCoachMark(2);
							instructionManual.HideBeforeCoachMark(1);

							isAimStepCompleted = true;
						}
					}

					if (!isFireExtinguished)
					{
						if (particleCollision.isColliding)
						{
							tick.SetActive(true);
							cross.SetActive(false);
							crosshair.color = Color.green;
						}
						else
						{
							cross.SetActive(true);
							tick.SetActive(false);
							crosshair.color = Color.red;
						}
					}
				}

				if (isSqueezeStepCompleted)
					isSweepStepCompleted = true;
			}
		}
	}

	private void RotateAroundAxis()
	{
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			switch (touch.phase)
			{
				case TouchPhase.Began:
					_startingPosition = touch.position.x;
					break;
				case TouchPhase.Moved:
					_movingPosition = touch.position.x;

					if (_movingPosition > _startingPosition)
						transform.Rotate(Vector3.up, -rotatespeed * Time.deltaTime);

					if (_movingPosition < _startingPosition)
						transform.Rotate(Vector3.up, rotatespeed * Time.deltaTime);

					_startingPosition = _movingPosition;
					break;
			}
		}
	}

	private IEnumerator AnimatePin(Transform pin)
	{
		pin.DOLocalMove(new Vector3(0f, 0.3808f, 0.04f), 0.75f).SetEase(Ease.Linear);

		yield return new WaitForSeconds(0.75f);

		isPullStepCompleted = true;

		crosshair.gameObject.SetActive(true);

		instructionManual.ShowInstructions("<color=red>Step 2:</color> A - Aim at the base of the fire.");
		instructionManual.ShowAfterCoachMark(1);
		instructionManual.HideBeforeCoachMark(0);

		fingerPointer.gameObject.SetActive(true);
		fingerPointer.Initialize();

		pin.SetParent(transform.parent, true);
		pin.gameObject.GetComponent<Rigidbody>().useGravity = true;

		yield return new WaitForSeconds(5f);

		pin.gameObject.GetComponent<Rigidbody>().useGravity = false;
		pin.gameObject.SetActive(false);
	}

	private IEnumerator AnimateSqueezer(Transform squeezer)
	{
		if (!isSqueezeStepCompleted)
		{
			squeezer.DOLocalRotate(new Vector3(-100f, 90f, -90f), 0.5f).SetEase(Ease.Linear);

			yield return new WaitForSeconds(0.5f);

			isSqueezeStepCompleted = true;

			instructionManual.ShowInstructions("<color=red>Step 4:</color> S - Swipe the extignuisher from side to side while aiming at the fire.");
			instructionManual.ShowAfterCoachMark(3);
			instructionManual.HideBeforeCoachMark(2);

			fingerPointer.gameObject.SetActive(true);
			fingerPointer.Initialize();

			SimulateSmoke();
		}
		else
		{
			if (isFireExtinguished)
			{
				squeezer.DOLocalRotate(new Vector3(-90f, 0f, 0f), 0.5f).SetEase(Ease.Linear);

				yield return new WaitForSeconds(0f);

				particleLoopGas.enabled = true;
				particleLoopPressurisedSteam.enabled = true;

				GameManager.Instance.isGameCompleted = true;
				GameManager.Instance.EndGame("Complete", success);
			}
		}
	}

    private void success()
    {
    }

    private void SimulateSmoke()
	{
		if (!pressurisedSteam.isPlaying)
			PlayParticles();
	}

	private void StopParticles()
	{
		pressurisedSteam.Stop();
		gas.Stop();
	}

	private void PlayParticles()
	{
		pressurisedSteam.Play();
		gas.Play();

		audioSource.Play();
	}

}
