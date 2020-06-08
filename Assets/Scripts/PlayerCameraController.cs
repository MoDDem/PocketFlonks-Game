using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    public Camera cameraInst;

    public float sensitivity = .5f;

    public bool reverseHorizontal = false;
    public bool reverseVertical = false;

    public bool centerCamera = true;
    public int delayBeforeCenter = 10;

    [Header("Camera min/max")]
    public float maxXCameraPos = 0f;
    public float maxYCameraPos = 0f;
    [Space(5)]
    public float minXCameraPos = 0f;
    public float minYCameraPos = 0f;
    [Space(10)]

    private Vector2 deviceInput;
    private Vector3 centerCameraPos;
    private Vector3 localPos;

    private float startTime = 0f;
    private float distanceToPlayer;

	void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        centerCameraPos = cameraInst.transform.localPosition;
        localPos = new Vector3(cameraInst.transform.localPosition.x, 0, cameraInst.transform.localPosition.z);
    }

	public void OnLook(InputValue value) => deviceInput = value.Get<Vector2>();

    void Update() {
        LookAround();

        if(Cursor.visible & Cursor.lockState == CursorLockMode.Locked)
            Cursor.visible = false;
    }

    private void LookAround() {
        if(Mouse.current.rightButton.wasPressedThisFrame) {
            if(Cursor.lockState == CursorLockMode.Locked) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else
                Cursor.lockState = CursorLockMode.Locked;
        }

        localPos += (Vector3.right * deviceInput.x + Vector3.forward * deviceInput.y) * sensitivity * Time.deltaTime;
        localPos.x = Mathf.Clamp(localPos.x, minXCameraPos, maxXCameraPos);
        localPos.z = Mathf.Clamp(localPos.z, minYCameraPos, maxYCameraPos);
        var localPosWithY = localPos + Vector3.up * cameraInst.transform.localPosition.y;

		if(centerCamera) {
            distanceToPlayer = Vector3.Distance(centerCameraPos, localPosWithY);
            if(distanceToPlayer > .2f & deviceInput == Vector2.zero) {
                if(startTime == 0f) {
                    startTime = Time.time;
                    return;
                }

                if(Time.time - startTime > delayBeforeCenter) {
                    float distCovered = (Time.time - startTime) * sensitivity * Time.deltaTime;
                    float fraction = distCovered / distanceToPlayer;

                    localPos = Vector3.Lerp(localPosWithY, centerCameraPos, fraction) - Vector3.up * cameraInst.transform.localPosition.y;
                }
            } else if(startTime != 0f) { startTime = 0f; }
        }

        cameraInst.transform.localPosition = localPosWithY;
	}
}
