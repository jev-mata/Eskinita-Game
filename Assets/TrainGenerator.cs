using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrainGenerator : MonoBehaviour
{
    public enum Direction { Left = -1, Right = 1 };

    public bool randomizeValues = false;

    public Direction direction;
    public float speed = 2.0f;
    public float interval = 6.0f;
    public float leftX = -20.0f;
    public float rightX = 20.0f;

    public GameObject[] carPrefabs;

    private float elapsedTime;
    private float deleydTime;

    private List<GameObject> cars;

    public Animator animator;

    public void Start()
    {
        if (randomizeValues)
        {
            direction = Random.value < 0.5f ? Direction.Left : Direction.Right;
            speed = Random.Range(speed - 2, speed);
            interval = Random.Range(7.0f, interval);
        }

        elapsedTime = interval;
        cars = new List<GameObject>();
    }

    public void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > interval)
        {

            animator.SetBool("isSpawning", true);
            deleydTime += Time.deltaTime;
            if (deleydTime > 2)
            {
                elapsedTime = 0.0f;
                deleydTime = 0.0f;
                SpawnTrain();
            }
            // TODO extract 0.375f and -0.5f to outside -- probably along with genericization
        }

        foreach (var o in cars.ToArray())
        {
            if (direction == Direction.Left && o.transform.position.x < leftX || direction == Direction.Right && o.transform.position.x > rightX)
            {
                Destroy(o);
                cars.Remove(o);

                animator.SetBool("isSpawning", false);
            }
        }
    }
    public void SpawnTrain()
    {
        var position = transform.position + new Vector3(direction == Direction.Left ? rightX : leftX, 0.6f, 0);
        var o = (GameObject)Instantiate(carPrefabs[Random.Range(0, carPrefabs.Length)], position, Quaternion.Euler(-90, 90, 0));
        o.GetComponent<CarScript>().speedX = (int)direction * speed;

        if (direction < 0)
            o.transform.rotation = Quaternion.Euler(-90, 270, 0);
        else
            o.transform.rotation = Quaternion.Euler(-90, 90, 0);

        cars.Add(o);
    }
    public void OnDestroy()
    {
        foreach (var o in cars)
        {
            Destroy(o);
        }
    }
}
