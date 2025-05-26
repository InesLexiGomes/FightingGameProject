using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    [SerializeField] private float playerSpacing;
    [SerializeField] private float playerDistanceRadius;

    private float player1StartingAngle;
    private float player2StartingAngle;

    private void Start()
    {
        player1StartingAngle = -Mathf.Asin(0.5f * playerSpacing / playerDistanceRadius) * Mathf.Rad2Deg;
        player2StartingAngle = -player1StartingAngle;

        Debug.Log(player1StartingAngle);
        Debug.Log(player2StartingAngle);
    }


}
