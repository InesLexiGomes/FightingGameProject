using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject p1;
    [SerializeField] private GameObject p2;

    [SerializeField] private GameObject stagecollisions;
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

    private void Start()
    {
        cam = GetComponent<Camera>();

        cam.fieldOfView = defaultFOV;
        gameObject.transform.localPosition = new Vector3 (0, defaultHeight, defaultDistance);
    }

    private void Update()
    {
        DoCameraBox();
        ChangeCameraFOV();
    }

    private void DoCameraBox()
    {
        float characterCenter = (p1.transform.position.x + p2.transform.position.x)/2;

        Vector3 targetPosition = transform.position;
        Vector3 stageCollisionPosition = stagecollisions.transform.position;
        Vector3 rotationCenterPosition = rotationCenter.transform.position;

        if (Mathf.Abs(targetPosition.x - characterCenter) > cameraPanOffset)
        {
            targetPosition.x = characterCenter;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * panningSpeed);
        
        stageCollisionPosition.x = transform.position.x;
        stagecollisions.transform.position = stageCollisionPosition;
        
        rotationCenterPosition.x = transform.position.x;
        rotationCenter.transform.position = rotationCenterPosition;
        rotationCenter.transform.rotation = Quaternion.Euler(0, stageCollisionPosition.x / stageCollisionPosition.z, 0);
    }


    private void ChangeCameraFOV()
    {
        float zoomDistance = (p1.transform.position - p2.transform.position).magnitude;

        if (zoomDistance > minZoomOutDistance)
        {
            Vector3 currentCamPos = transform.localPosition;

            float targetDistance = defaultDistance + (zoomDistance - minZoomOutDistance) * distanceMultiplier;
            currentCamPos.z = Mathf.Lerp(transform.localPosition.z, targetDistance, Time.deltaTime);
            transform.localPosition = currentCamPos;

            float targetFOV = defaultFOV + (zoomDistance - minZoomOutDistance) * fovMultiplier;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime);
        }
        else
        {
            Vector3 currentCamPos = transform.localPosition;
            currentCamPos.z = Mathf.Lerp(transform.localPosition.z, defaultDistance, Time.deltaTime);
            transform.localPosition = currentCamPos;

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, Time.deltaTime);
        }
    }
}
