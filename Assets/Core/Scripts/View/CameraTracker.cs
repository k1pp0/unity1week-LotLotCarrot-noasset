using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraTracker : MonoBehaviour
{
    private Camera mainCamera;
    private Transform cameraTransform;
    private Transform stabilizer;
    private Transform target;

    [SerializeField] private float distance;
    [SerializeField] private float speed;
    [SerializeField] private float rot;

    private void Awake()
    {
        cameraTransform = GetComponent<Transform>();
        mainCamera = GetComponent<Camera>();
        stabilizer = new GameObject("Stabilizer").transform;
        target = new GameObject("Target").transform;
    }

    public void SetTargetPosition(Vector3 centerPosition)
    {
        var targetPosition = target.position;
        var cameraPosition = cameraTransform.position;

        targetPosition = Vector3.Lerp(targetPosition, centerPosition, speed * Time.deltaTime);
        target.position = targetPosition;

        cameraPosition = Vector3.Lerp(cameraPosition,
            targetPosition + new Vector3(Mathf.Cos(rot), 0.75f, Mathf.Sin(rot)).normalized * distance,
            speed * Time.deltaTime);
        cameraTransform.position = cameraPosition;

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, 60, speed * Time.deltaTime);

        stabilizer.position = Vector3.Lerp(stabilizer.position, cameraPosition, speed * Time.deltaTime);
        stabilizer.LookAt(target);

        cameraTransform.rotation =
            Quaternion.Lerp(cameraTransform.rotation, stabilizer.rotation, speed * Time.deltaTime);
    }

    public void SetDistance(float d, float r)
    {
        distance = d;
        rot = r;
    }
}