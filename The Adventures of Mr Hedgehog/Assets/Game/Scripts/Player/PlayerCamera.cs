using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private float mouseSense = 5f;

    [Header("Vill ha Target look under Player.")][SerializeField]
    private Transform target; // Kameran snurrar runt denna target Transform.

    [Space][SerializeField]
    private float distanceFromTarget = 5f;

    [SerializeField]
    private Vector2 pitchMinMax = new Vector2(-15, 40); // Gräns för hur mycket kameran kan röra sig upp och ned.
    private float yaw = 45, pitch = 15;

    [SerializeField]
    private float rotationSmoothTime = 0.12f;
    private Vector3 rotationSmoothVelocity, currentRotation;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine("UpdateCameraRotation");
    }

    /// <summary>
    ///  Både FixedUpdate och LateUpdate orsakade jittering i kamrans rörelse eftersom spelarens movement uppdateras i FixedUpdate.
    ///  Detta är ett försök till LateFixedUpdate.
    /// </summary>
    private IEnumerator UpdateCameraRotation()
    {
        while (true) {
            yield return new WaitForFixedUpdate();
            yaw += Input.GetAxis("Mouse X") * mouseSense;
            pitch -= Input.GetAxis("Mouse Y") * mouseSense;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            currentRotation = Vector3.SmoothDamp(currentRotation, new Vector2(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotation;

            transform.position = target.position - transform.forward * distanceFromTarget;
        }
    }

    public void SetCameraRotation(Transform t, float pitch, float yaw)
    {
        StopCoroutine("UpdateCameraRotation");
        transform.position = t.position + transform.forward * 10 + new Vector3(0,6,1f);
        transform.LookAt(target);
        currentRotation = transform.position;
        this.pitch = pitch;
        this.yaw = yaw;
        StartCoroutine("UpdateCameraRotation");
    }
}
