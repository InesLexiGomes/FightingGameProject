using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private GameObject model;
    private CapsuleCollider capsuleCollider;
    private Rigidbody rb;

    [Header("Stage config")] // will be moved to the stage itself so depending on the stage the characters autoadjust
    [SerializeField] private float characterDistanceRadius;
    [SerializeField] private float startRotation;

    [Header("Character movement config")]
    [SerializeField] private float walkSpeed;

    public Transform CharacterTransform { get { return model.transform; } }
    private float walkSpeedRotation;

    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        capsuleCollider.center = new Vector3(0, 1, characterDistanceRadius);
        model.transform.localPosition = new Vector3(model.transform.localPosition.x, model.transform.localPosition.y, characterDistanceRadius);
        walkSpeedRotation = walkSpeed / characterDistanceRadius;
        gameObject.transform.rotation = Quaternion.Euler(0, startRotation, 0);
    }

    private void FixedUpdate()
    {
        float doRotation = walkSpeedRotation * Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, doRotation, 0));
    }
}
