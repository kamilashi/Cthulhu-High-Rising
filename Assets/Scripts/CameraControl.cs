using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform cameraHeight;
    public Transform cameraRotation;
    public float maxHeight = 20f;
    public Vector2 mouseDeltaModifier = new Vector2(0.05f, 0.2f);
    public bool click;

    public Vector3 heightVelocity;
    public Vector3 rotationalVelocity;

    public Vector3 mouseInputLast;
    public Vector3 mouseInputCurrent;
    public Vector3 mouseDelta;

    void Update()
    {
        mouseInputCurrent = Input.mousePosition;
        mouseDelta = mouseInputCurrent - mouseInputLast; 
        mouseInputLast = mouseInputCurrent + new Vector3(0,0,0);

        click = Input.GetMouseButton(1);
        float active = click ? 1 : 0;
        
        float deltaHeightChange = Input.mouseScrollDelta.y + mouseDelta.y * active * mouseDeltaModifier.y;

        heightVelocity = new Vector3(
            Mathf.Lerp(heightVelocity.x, deltaHeightChange, heightVelocity.z * Time.deltaTime), 
            heightVelocity.y, 
            heightVelocity.z);

        cameraHeight.position = new Vector3(cameraHeight.position.x, Mathf.Clamp(cameraHeight.position.y + heightVelocity.x, 0, maxHeight), cameraHeight.position.z);

        float deltaCameraRotationChange = mouseDelta.x * active * mouseDeltaModifier.x;
        
        rotationalVelocity = new Vector3(
            Mathf.Lerp(rotationalVelocity.x, deltaCameraRotationChange, rotationalVelocity.z * Time.deltaTime), 
            rotationalVelocity.y, 
            rotationalVelocity.z);
        
        cameraRotation.Rotate(0, rotationalVelocity.x, 0);
    }


}
