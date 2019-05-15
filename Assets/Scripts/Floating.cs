using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    //FLOATING TIENE UN ERROR, ORIGINALPOS Y ORIGINALLOCALPOS NO SE UPDATEAN Y ALTERAN EL MOVIMIENTO
    public float amplitude = 1f, oscSpeed = 1f, oscOffsset = 0f;

    public Vector3 oscAxis = Vector3.up;

    public bool randomOffset = true, localValues = true;

    Vector3 originalPos, amplitudeVec, originalLocalPos;

    // Use this for initialization
    void Awake()
    {
        originalPos = transform.transform.position;
        originalLocalPos = transform.localPosition;
        if (randomOffset) oscOffsset = UnityEngine.Random.Range(0, Mathf.PI * 2);

        oscAxis = oscAxis.normalized;

        if (localValues)
            amplitudeVec = transform.localToWorldMatrix * oscAxis * amplitude;
        else
            amplitudeVec = oscAxis * amplitude;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Cos(Time.frameCount * 0.04f) * 0.001f, transform.position.z);

        //transform.localPosition = originalLocalPos + amplitudeVec * Mathf.Sin(oscOffsset + oscSpeed * Time.time);
        if (localValues)
            transform.localPosition = originalLocalPos + amplitudeVec * Mathf.Sin(oscOffsset + oscSpeed * Time.time);
        else
            transform.position = originalPos + amplitudeVec * Mathf.Sin(oscOffsset + oscSpeed * Time.time);
    }
}
