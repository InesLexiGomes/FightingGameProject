using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject rotationCenter;

    [SerializeField] private float panningSpeed;

    [SerializeField] private float minZoomOutDistance;

    [SerializeField] private float defaultDistance;
    [SerializeField] private float defaultHeight;
    [SerializeField] private float defaultFOV;

    [SerializeField] private float distanceMultiplier;
    [SerializeField] private float fovMultiplier;

    private Vector3 targetPosition;
    private Vector3 targetStagePosition;

    private CharacterMovement[] players; 
    private Camera[] cameras;

    private void Start()
    {
        players = FindObjectsByType<CharacterMovement>(FindObjectsSortMode.None);

        cameras = FindObjectsByType<Camera>(FindObjectsSortMode.None);

        foreach (Camera cam in cameras)
            cam.fieldOfView = defaultFOV;

        gameObject.transform.position = new Vector3 (0, defaultHeight, defaultDistance);
        targetPosition = transform.position;
        targetStagePosition = rotationCenter.transform.position;
    }

    private void Update()
    {
        DoCameraFollow();
        ChangeCameraFOV();
    }

    private void FixedUpdate()
    {
        StageRotation();
    }

    private void DoCameraFollow()
    {
        targetPosition.x = Mathf.Lerp (targetPosition.x, (players[0].transform.position.x + players[1].transform.position.x) 
            / 2, Time.deltaTime * panningSpeed);

        transform.position = targetPosition;
    }

    private void StageRotation()
    {
        rotationCenter.transform.rotation = Quaternion.Euler(0, transform.position.x, 0);

        targetStagePosition.x = targetPosition.x;
        rotationCenter.transform.position = targetStagePosition;
    }


    private void ChangeCameraFOV()
    {
        float zoomDistance = (players[0].transform.position - players[1].transform.position).magnitude;

        if (zoomDistance > minZoomOutDistance)
        {
            Vector3 currentCamPos = transform.position;

            float targetDistance = defaultDistance + (zoomDistance - minZoomOutDistance) * distanceMultiplier;
            currentCamPos.z = Mathf.Lerp(transform.position.z, targetDistance, Time.deltaTime);
            transform.position = currentCamPos;

            float targetFOV = defaultFOV + (zoomDistance - minZoomOutDistance) * fovMultiplier;
            foreach(Camera cam in cameras)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime);
        }
        else
        {
            Vector3 currentCamPos = transform.position;
            currentCamPos.z = Mathf.Lerp(transform.position.z, defaultDistance, Time.deltaTime);
            transform.position = currentCamPos;

            foreach(Camera cam in cameras)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, Time.deltaTime);
        }
    }
}
