using UnityEngine;

[ExecuteInEditMode]
public class FollowPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 10, -10);

    public float leftClampX = 0f;
    public float FoV_min = 3f;
    public float rightClampX = 19f;
    private float speedMultipier = 1.2f;
    private float CurrentSpeedMultipier = 1;
    private PlayerMovementScript playerMovementScript;
    public Camera _camera;
    public void Start()
    {

        playerMovementScript = player.gameObject.GetComponent<PlayerMovementScript>();
        speedMultipier = playerMovementScript.speedMultipier;
    }
    void LateUpdate()
    {
        if (player == null) return;

        CurrentSpeedMultipier = (player.position.z / 20) > 1 ? (Mathf.RoundToInt(player.position.z / 20) * (speedMultipier - 1)) + 1 : 1;
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, FoV_min + (CurrentSpeedMultipier / 2), 0.5f);
        // Follow player's Z and X, but clamp X within limits
        float targetX = Mathf.Clamp(player.position.x, leftClampX, rightClampX);
        float targetZ = player.position.z;

        Vector3 targetPosition = new Vector3(targetX, 0, targetZ) + offset;

        transform.position = targetPosition;
    }
}
