using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoRotation : MonoBehaviour
{
    public float speed;
    private float rotationValue;

    void Update()
    {
        rotationValue += Time.deltaTime * speed;
        if (rotationValue > 360)
            rotationValue = 0f;

        transform.eulerAngles = new Vector3(0f, 0f, -rotationValue);
    }
}
