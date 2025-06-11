using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject characterCollision;
    [SerializeField] private GameObject stage;

    [SerializeField] private float panningSpeed;

    [SerializeField] private float minZoomOutDistance;

    [SerializeField] private float defaultDistance;
    [SerializeField] private float defaultHeight;
    [SerializeField] private float defaultFOV;

    [SerializeField] private float fovMultiplier;

    private Vector3 targetPosition;
    private Vector3 targetCollisionPosition;
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
        targetStagePosition = characterCollision.transform.position;
        targetStagePosition = stage.transform.position;
    }

    private void Update()
    {
        ChangeCameraFOV();
    }

    private void FixedUpdate()
    {
        DoCameraFollow();
        StageManagement();
    }

    private void DoCameraFollow()
    {
        targetPosition.x = Mathf.Lerp (targetPosition.x, (players[0].transform.position.x + players[1].transform.position.x) 
            / 2, Time.deltaTime * panningSpeed);

        transform.position = targetPosition;
    }

    private void StageManagement()
    {
        stage.transform.rotation = Quaternion.Euler(0, transform.position.x, 0);

        targetStagePosition.x = targetPosition.x;
        stage.transform.position = targetStagePosition;

        targetCollisionPosition.x = targetPosition.x;
        characterCollision.transform.position = targetCollisionPosition;
    }

    private void ChangeCameraFOV()
    {
        float zoomDistance = (players[0].transform.position - players[1].transform.position).magnitude;

        if (zoomDistance > minZoomOutDistance)
        {
            float targetFOV = defaultFOV + (zoomDistance - minZoomOutDistance) * fovMultiplier;
            foreach(Camera cam in cameras)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime);
        }
        else
        {
            foreach(Camera cam in cameras)
                cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, Time.deltaTime);
        }
    }
}
