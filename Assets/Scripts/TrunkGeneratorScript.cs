using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TrunkGeneratorScript : MonoBehaviour
{
    public enum Direction { Left = -1, Right = 1 };

    public bool randomizeValues = false;

    public Direction direction;
    public float speed = 2.0f;
    public float length = 2.0f;
    public float interval = 2.0f;
    public float leftX = -20.0f;
    public float rightX = 20.0f;

    public List<GameObject> trunkPrefab;

    private float elapsedTime;

    private List<GameObject> trunks;

    private Transform player;
    private float speedMultipier = 1.2f;
    private float CurrentSpeedMultipier = 1;
    private PlayerMovementScript playerMovementScript;
    public void Start()
    {
        if (randomizeValues)
        {
            // direction = Random.value < 0.5f ? Direction.Left : Direction.Right;
            speed = Random.Range(1.0f, speed);
            length = Random.Range(1, length);
            interval = length / speed + Random.Range(4.0f, interval);
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerMovementScript = player.gameObject.GetComponent<PlayerMovementScript>();
            speedMultipier = playerMovementScript.speedMultipier;
        }

        elapsedTime = interval;
        trunks = new List<GameObject>();
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        CurrentSpeedMultipier = (player.position.z / 20) > 1 ? (Mathf.RoundToInt(player.position.z / 20) * (speedMultipier - 1)) + 1 : 1;
        if (elapsedTime > (interval / CurrentSpeedMultipier))
        {
            elapsedTime = 0.0f;

        if (randomizeValues)
        {
            // direction = Random.value < 0.5f ? Direction.Left : Direction.Right;
            speed = Random.Range(1.0f, speed);
            length = Random.Range(1, length);
            interval = length / speed + Random.Range(4.0f, interval);
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerMovementScript = player.gameObject.GetComponent<PlayerMovementScript>();
            speedMultipier = playerMovementScript.speedMultipier;
        }

            var position = transform.position + new Vector3(direction == Direction.Left ? rightX : leftX, 0, 0);

            var o = (GameObject)Instantiate(trunkPrefab[Random.Range(0, trunkPrefab.Count)], position, Quaternion.identity);
            var trunk = o.GetComponent<TrunkFloatingScript>();
            if (trunk != null)
            {
                trunk.speedX = (int)direction * speed;
            }

            var car = o.GetComponent<CarScript>();
            if (car != null)
            {
                length = 1;
                car.speedX = (int)direction * speed;
            }

            var scale = o.transform.localScale;
            if (direction == Direction.Left)
            {
                o.transform.rotation = Quaternion.Euler(0, 180, 0); // Face left (flip on Y)
            }
            else
            {
                o.transform.rotation = Quaternion.Euler(0, 0, 0); // Face right (default)
            }
            o.transform.localScale = new Vector3(scale.x * length, scale.y, scale.z);

            trunks.Add(o);
        }

        foreach (var o in trunks.ToArray())
        {
            if (direction == Direction.Left && o.transform.position.x < leftX || direction == Direction.Right && o.transform.position.x > rightX)
            {
                Destroy(o);
                trunks.Remove(o);
            }
        }
    }

    void OnDestroy()
    {
        foreach (var o in trunks)
        {
            Destroy(o);
        }
    }
}
