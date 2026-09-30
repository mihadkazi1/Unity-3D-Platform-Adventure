using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(0f, 5f, -7f);

    public float followSpeed = 8f;

    public float lookHeight = 1f;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Vector3 lookPosition =
            target.position + Vector3.up * lookHeight;

        transform.LookAt(lookPosition);
    }
}