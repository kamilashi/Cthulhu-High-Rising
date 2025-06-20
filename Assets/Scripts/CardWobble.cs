using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class CardWobble : MonoBehaviour
{
    private Transform tf;

    private Vector3 localEuler;
    private Vector3 localPos;

    private float random;

    public Vector3 hoverValue = new Vector3(0, 0, 5f);
    public Vector3 wobbleValue = new Vector3(5, 0, 1f);

    // Start is called before the first frame update
    void Start()
    {
        random = Random.value;
        tf = GetComponent<Transform>();
        localEuler = tf.localEulerAngles;
        localPos = tf.localPosition;
    }

// Update is called once per frame
void Update()
    {
        hoverValue = new Vector3(
            Mathf.Lerp(hoverValue.x, hoverValue.y, Time.deltaTime * hoverValue.z),
            hoverValue.y,
            hoverValue.z);

        wobbleValue = new Vector3(
            Mathf.Lerp(wobbleValue.x, wobbleValue.y, Time.deltaTime * wobbleValue.z),
            wobbleValue.y,
            wobbleValue.z);

        float xFactor = Mathf.Sin((Time.time + random * 1.5f) * 5f) * wobbleValue.x;
        float yFactor = Mathf.Cos((Time.time + random * 2f) * 5f) * wobbleValue.x;
        float zFactor = xFactor * 0.5f + yFactor * 0.5f;

        tf.localEulerAngles = new Vector3(xFactor, yFactor, zFactor) + localEuler;
        tf.localPosition = new Vector3(0, 0.1f * hoverValue.x, 0) + localPos;
    }

    public void SetHoverState(bool hovered)
    {
        hoverValue = new Vector3(
            hoverValue.x,
            hovered ? 1 : 0,
            hoverValue.z);

        wobbleValue = new Vector3(
            15,
            0,
            wobbleValue.z);
    }
}
