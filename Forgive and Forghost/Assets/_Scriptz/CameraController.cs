using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float minXRot;
    public float maxXRot;
    public float mouseSensitivity = 3;
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
    private float rotationLerpVal;
    private float rotationDampVel;

    private float playerRotationDampVel;

	void Start () {
        playerMaterial = playerRenderer.material;
        playerColor = playerMaterial.GetColor("_Color");
        transform.parent = null;
	}
		
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            targetFirstPersonLerpAmount -= .1f;
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            targetFirstPersonLerpAmount += .1f;
        }
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
            //Vector3 euler = playerRenderer.transform.localRotation.eulerAngles;
            //euler.y = Mathf.SmoothDamp(euler.y, 0, ref playerRotationDampVel, smoothDampTime, 100, Time.deltaTime);
            //playerRenderer.transform.localRotation= Quaternion.Euler(euler);
        }
        else
        {
            if(Quaternion.Angle(transform.rotation, firstPersonPosition.rotation) > .01f )
            {
                rotationLerpVal = 0;
            }
            rotationLerpVal = Mathf.SmoothDamp(rotationLerpVal, 1, ref rotationDampVel, smoothDampTime, 100, Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, firstPersonPosition.rotation, rotationLerpVal);
       }
        float xInput = Input.GetAxis("Mouse X");
        if (Input.GetKey(KeyCode.J))
        {
            xInput = -1;
        }
        if (Input.GetKey(KeyCode.L))
        {
            xInput = 1;
        }
        playerRenderer.transform.Rotate(Vector3.up, xInput * Time.deltaTime * mouseSensitivity);
        thirdPersonPosition.transform.Rotate(playerRenderer.transform.up, xInput * Time.deltaTime * mouseSensitivity, Space.World);


        float yInput = -Input.GetAxis("Mouse Y");
        if (Input.GetKey(KeyCode.I)){
            yInput = 1;
        }
        if (Input.GetKey(KeyCode.K))
        {
            yInput = -1;
        }
        thirdPersonPosition.transform.Rotate(Vector3.right, yInput * Time.deltaTime * mouseSensitivity);
        firstPersonPosition.transform.Rotate(Vector3.right, yInput * Time.deltaTime * mouseSensitivity);       
    
    }
}
