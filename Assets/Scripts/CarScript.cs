using UnityEngine;
using System.Collections;

public class CarScript : MonoBehaviour {
    public float speedX = 1.0f;
    private Rigidbody playerBody;
    private float CurrentSpeedMultipier = 1;
    private float speedMultipier = 1.2f;
    private Transform player;
    private PlayerMovementScript playerMovementScript;

    public void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMovementScript = player.gameObject.GetComponent<PlayerMovementScript>();
        speedMultipier = playerMovementScript.speedMultipier;
    }
    public void Update()
    {
        CurrentSpeedMultipier = (player.position.z / 20) > 1 ? (Mathf.RoundToInt(player.position.z / 20) * (speedMultipier - 1)) + 1 : 1; 
        transform.position += new Vector3(speedX*CurrentSpeedMultipier * Time.deltaTime, 0.0f, 0.0f);
    }

    void OnTriggerEnter(Collider other) {
        // When collide with player, flatten it!
        if (other.gameObject.tag == "Player")
        { 
            
            PlayerMovementScript playerMovementScript = other.GetComponent<PlayerMovementScript>(); 
            playerMovementScript.GameOver();
        }
    }
}
