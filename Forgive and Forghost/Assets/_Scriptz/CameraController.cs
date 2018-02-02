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

	[Header("New Settings")]
	public bool rotateYAroundGhosty;
	public Transform verticalAxisParent;
	public float verticalClampAngle;
	public Camera myCamera;
	public float fullspeedFov;
	public float fullSpeedCameraPullbackDistance;

    private float lerpAmount = 0;

    private Vector3 targetPosition;
    private Vector3 positionDampVel = Vector3.zero;
    private float smoothDampVel = 0;
    private Material playerMaterial;
    private Color playerColor;
    private float rotationLerpVal;
    private float rotationDampVel;

    private Quaternion playerStartRotation;
    private Quaternion thirdPersonStartRotation;
    private Quaternion firstPersonStartRotation;

    private float playerRotationDampVel;

	private float _currentVerticalAngle = 0;
	private float _minSpeedFov;
	private float _targetLerpValue;
	private float _currentLerpValue;
	private Vector3 _originalThirdPersonStartPosition;

	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        playerMaterial = playerRenderer.material;
        playerColor = playerMaterial.GetColor("_Color");
        transform.parent = null;
        playerStartRotation = playerRenderer.transform.localRotation;
        thirdPersonStartRotation = thirdPersonPosition.localRotation;
        firstPersonStartRotation = firstPersonPosition.localRotation;
		_minSpeedFov = myCamera.fieldOfView;
		_originalThirdPersonStartPosition = thirdPersonPosition.localPosition;
	}
		
	void Update () {
		if (Input.GetKey(KeyCode.N))
		{
			myCamera.fieldOfView -= Time.deltaTime * 12f;
		}
		else if (Input.GetKey(KeyCode.M))
		{
			myCamera.fieldOfView += Time.deltaTime * 12f;
		}

		if (Input.GetKey(KeyCode.Comma))
		{
			thirdPersonPosition.position -= thirdPersonPosition.forward * Time.deltaTime * 12f;
		}
		else if (Input.GetKey(KeyCode.Period))
		{
			thirdPersonPosition.position += thirdPersonPosition.forward * Time.deltaTime * 12f;
		}

		if (Input.GetKey(KeyCode.H))
		{
			mouseSensitivity -= Time.deltaTime * 20f;
		}
		else if (Input.GetKey(KeyCode.J))
		{
			mouseSensitivity += Time.deltaTime * 20f;
		}

		if (Input.GetKey(KeyCode.K))
		{
			smoothDampTime -= Time.deltaTime;
		}
		else if (Input.GetKey(KeyCode.L))
		{
			smoothDampTime += Time.deltaTime;
		}


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
        if (Input.GetKeyDown(KeyCode.R))
        {
            firstPersonPosition.localRotation = firstPersonStartRotation;
            thirdPersonPosition.localRotation = thirdPersonStartRotation;
            playerRenderer.transform.localRotation = playerStartRotation;
        }
        float xInput = Input.GetAxis("Mouse X");
		if (Input.GetKey(KeyCode.LeftArrow))
        {
            xInput = -1;
        }
		if (Input.GetKey(KeyCode.RightArrow))
        {
            xInput = 1;
        }
        playerRenderer.transform.Rotate(Vector3.up, xInput * Time.deltaTime * mouseSensitivity);
        //thirdPersonPosition.transform.Rotate(playerRenderer.transform.up, xInput * Time.deltaTime * mouseSensitivity, Space.World);


        float yInput = -Input.GetAxis("Mouse Y");
		if (Input.GetKey(KeyCode.DownArrow)){
            yInput = 1;
        }
		if (Input.GetKey(KeyCode.UpArrow))
        {
            yInput = -1;
        }

		if (rotateYAroundGhosty)
		{
			float verticalDelta = yInput * Time.deltaTime * mouseSensitivity;
			_currentVerticalAngle += verticalDelta;

			if (_currentVerticalAngle >= verticalClampAngle)
			{
				verticalDelta = verticalClampAngle - _currentVerticalAngle;
				_currentVerticalAngle = verticalClampAngle;
			}
			else if (_currentVerticalAngle <= -verticalClampAngle)
			{
				verticalDelta = -verticalClampAngle - _currentVerticalAngle;
				_currentVerticalAngle = -verticalClampAngle;
			}

			verticalAxisParent.localRotation = Quaternion.AngleAxis(_currentVerticalAngle, Vector3.right);
		}
		else
		{
			thirdPersonPosition.transform.Rotate(Vector3.right, yInput * Time.deltaTime * mouseSensitivity);
			firstPersonPosition.transform.Rotate(Vector3.right, yInput * Time.deltaTime * mouseSensitivity);
		}
    }

	public void WaitAFrame()
	{
		//StartCoroutine(DoWaitAFrame());
	}

	protected IEnumerator DoWaitAFrame()
	{
		//Transform originalParent = thirdPersonPosition.parent;
		//thirdPersonPosition.SetParent(null);
		//Quaternion originalHorizontalRot = playerRenderer.transform.rotation;
		Quaternion originalVerticalRot = verticalAxisParent.transform.rotation;
		yield return null;
		//thirdPersonPosition.SetParent(originalParent);
		//playerRenderer.transform.rotation = originalHorizontalRot;
		verticalAxisParent.transform.rotation = originalVerticalRot;
	}

	public void UpdateSpeedRelatedLerpThings(float lerpAmount)
	{
		_targetLerpValue = lerpAmount;
		_currentLerpValue = Mathf.Lerp(_currentLerpValue, _targetLerpValue, Time.deltaTime * 0.5f);

		myCamera.fieldOfView = Mathf.Lerp(_minSpeedFov, fullspeedFov, _currentLerpValue);
		thirdPersonPosition.localPosition = Vector3.Lerp(_originalThirdPersonStartPosition, _originalThirdPersonStartPosition + (_originalThirdPersonStartPosition.normalized) * -fullSpeedCameraPullbackDistance, _currentLerpValue);
	}
}
