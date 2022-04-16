using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    [SerializeField] float mouseSensitivity = 75f;
    [SerializeField] float clampAngle = 70;
    float rotY = 0.0f;

    private void Start() {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
    }

    private void Update() {
        float mouseX = Input.GetAxis("Mouse X");

        rotY += mouseX * mouseSensitivity * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, rotY, 0.0f);
    }
}
