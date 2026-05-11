using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float clampAngle = 80f;

    private float _pitch = 0f;

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, -clampAngle, clampAngle);
        transform.localEulerAngles = new Vector3(_pitch, 0f, 0f);
    }
}
