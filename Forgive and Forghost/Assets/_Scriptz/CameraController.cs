using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform firstPersonPosition;
    public Transform thirdPersonPosition;
    public Renderer playerRenderer;
    public float smoothDampTime;
    [Range(0,1)]
    public float targetFirstPersonLerpAmount;
    public float startTransparencyLerpDistance;
    public float endTransparencyLerpDistance;

    private float lerpAmount = 0;

    private Vector3 targetPosition;
    private Vector3 positionDampVel = Vector3.zero;
    private float smoothDampVel = 0;
    private Material playerMaterial;
    private Color playerColor;
    private Vector2 mouseInput = Vector2.zero;
    private float rotationLerpVal;
    private float rotationDampVel;

	void Start () {
        playerMaterial = playerRenderer.material;
        playerColor = playerMaterial.GetColor("_Color");
        transform.parent = null;
	}
		
	void Update () {
        lerpAmount = Mathf.SmoothDamp(lerpAmount, targetFirstPersonLerpAmount, ref smoothDampVel, smoothDampTime, 100, Time.deltaTime);
        targetPosition = Vector3.Lerp(thirdPersonPosition.position, firstPersonPosition.position, lerpAmount);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref positionDampVel, smoothDampTime, 100, Time.deltaTime);
        Vector3 deltaVec = (playerRenderer.transform.position - transform.position);
        float deltaMag = deltaVec.magnitude;
        deltaMag *= Vector3.Dot(transform.forward, deltaVec) > .8f? 1.0f : -1.0f;
        playerColor.a = 1.0f-Mathf.InverseLerp(startTransparencyLerpDistance, endTransparencyLerpDistance, deltaMag);
        playerMaterial.SetColor("_Color", playerColor);
       

        if(targetFirstPersonLerpAmount < .9f)
        {
            if(Quaternion.Angle(transform.rotation, thirdPersonPosition.rotation) > .01f)
            {
                rotationLerpVal = 0;
            }
            rotationLerpVal = Mathf.SmoothDamp(rotationLerpVal, 1, ref rotationDampVel, smoothDampTime, 100, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, thirdPersonPosition.rotation, rotationLerpVal);
            
        }
        else
        {
            mouseInput.x = Input.GetAxis("Mouse X");
            mouseInput.y = Input.GetAxis("Mouse Y");

        }

       
    }
}
