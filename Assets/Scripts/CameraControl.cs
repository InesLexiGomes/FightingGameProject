using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;

    [SerializeField] private GameObject rotationCenter;

    [SerializeField] private float cameraPanOffset;
    [SerializeField] private float panningSpeed;

    [SerializeField] private float minZoomOutDistance;

    [SerializeField] private float defaultDistance;
    [SerializeField] private float defaultHeight;
    [SerializeField] private float defaultFOV;

    [SerializeField] private float distanceMultiplier;
    [SerializeField] private float fovMultiplier;

    private Camera cam;

    private Vector3 targetCamPosition;

    private void Start()
    {
        cam = GetComponent<Camera>();

        cam.fieldOfView = defaultFOV;
        gameObject.transform.position = new Vector3 (0, defaultHeight, defaultDistance);
        targetCamPosition = transform.position;
    }

    private void Update()
    {
        DoCameraBox();
        ChangeCameraFOV();
    }

    private void FixedUpdate()
    {
        StageRotation();
    }

    private void DoCameraBox()
    {
        float characterCenter = (p1.transform.position.x + p2.transform.position.x)/2;

        if (Mathf.Abs(targetCamPosition.x - characterCenter) > cameraPanOffset)
        {
            targetCamPosition.x = Mathf.Lerp (targetCamPosition.x, characterCenter, Time.deltaTime * panningSpeed);
        }

        transform.position = Vector3.Lerp(transform.position, targetCamPosition, Time.deltaTime * panningSpeed);
    }

    private void StageRotation()
    {
        rotationCenter.transform.rotation = Quaternion.Euler(0, transform.position.x, 0);
    }


    private void ChangeCameraFOV()
    {
        float zoomDistance = (p1.transform.position - p2.transform.position).magnitude;

        if (zoomDistance > minZoomOutDistance)
        {
            Vector3 currentCamPos = transform.position;

            float targetDistance = defaultDistance + (zoomDistance - minZoomOutDistance) * distanceMultiplier;
            currentCamPos.z = Mathf.Lerp(transform.position.z, targetDistance, Time.deltaTime);
            transform.position = currentCamPos;

            float targetFOV = defaultFOV + (zoomDistance - minZoomOutDistance) * fovMultiplier;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime);
        }
        else
        {
            Vector3 currentCamPos = transform.position;
            currentCamPos.z = Mathf.Lerp(transform.position.z, defaultDistance, Time.deltaTime);
            transform.position = currentCamPos;

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, Time.deltaTime);
        }
    }
}
