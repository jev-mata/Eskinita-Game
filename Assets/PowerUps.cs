using UnityEditor.UIElements;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Power Up Settings")]
    public PowerUpType powerUpType;


    //life Amount Settings
    [Header("Life Settings")]
    public int lifeAmount;

    [Header("Skill Settings")]
    public float skillDuration;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameStateControllerScript gameStateController = GameObject.Find("GameStateController").GetComponent<GameStateControllerScript>();
            gameStateController.AddLifePoint(lifeAmount);
            LevelControllerScript levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelControllerScript>();
            levelControllerScript.Picked();
            Destroy(gameObject);
        }
        if (other.tag == "gridObstacle")
        {

            int randX = Random.Range(-6, 6);
            int randZ = Random.Range(-2 + (int)transform.position.z, (int)transform.position.z + 2);
            transform.position = new Vector3(randX, transform.position.y, randZ);
        }
    }
}
public enum PowerUpType
{
    Life,
    Skill,

}