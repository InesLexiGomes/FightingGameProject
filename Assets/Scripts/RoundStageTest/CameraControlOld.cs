using UnityEngine;

public class CameraControlOld : MonoBehaviour
{
    [SerializeField] private Player p1;
    [SerializeField] private Player p2;

    [SerializeField] private float cameraRotationOffset;

    [SerializeField] private float minZoomOutDistance;

    [SerializeField] private float defaultDistance;
    [SerializeField] private float defaultHeight;
    [SerializeField] private float defaultFOV;

    [SerializeField] private float distanceMultiplier;
    [SerializeField] private float fovMultiplier;

    private Camera cam;

    private Vector3 playersCenterPosition;
    private Vector3 currentLookAtPosition;

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();

        cam.fieldOfView = defaultFOV;
        cam.gameObject.transform.localPosition = new Vector3 (0, defaultHeight, defaultDistance);
    }

    private void Update()
    {
        DoCameraBox();
        ChangeCameraFOV();
    }

    private void DoCameraBox()
    {
        // Calculate the center between the two players
        playersCenterPosition = (p1.CharacterTransform.position + p2.CharacterTransform.position) / 2;

        // Adjust camera look at position based on offset
        Vector3 targetPosition = currentLookAtPosition;

        // Check if the camera should move to center between the players
        if ((currentLookAtPosition - playersCenterPosition).magnitude > cameraRotationOffset)
        {
            targetPosition = playersCenterPosition;
        }

        // Move the camera smoothly towards the target position
        currentLookAtPosition = Vector3.Lerp(currentLookAtPosition, targetPosition, Time.deltaTime * 5f);

        // Make sure to look at the players' center
        transform.LookAt(playersCenterPosition);
    }


    private void ChangeCameraFOV()
    {
        float zoomDistance = (p1.CharacterTransform.position - p2.CharacterTransform.position).magnitude;

        if (zoomDistance > minZoomOutDistance)
        {
            Vector3 currentCamPos = cam.transform.localPosition;

            float targetDistance = defaultDistance + (zoomDistance - minZoomOutDistance) * distanceMultiplier;
            currentCamPos.z = Mathf.Lerp(cam.transform.localPosition.z, targetDistance, Time.deltaTime);
            cam.transform.localPosition = currentCamPos;

            float targetFOV = defaultFOV + (zoomDistance - minZoomOutDistance) * fovMultiplier;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime);
        }
        else
        {
            Vector3 currentCamPos = cam.transform.localPosition;
            currentCamPos.z = Mathf.Lerp(cam.transform.localPosition.z, defaultDistance, Time.deltaTime);
            cam.transform.localPosition = currentCamPos;

            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, Time.deltaTime);
        }
    }
}
