using UnityEngine;

public class SnapGridPlayer : MonoBehaviour
{
    [TagSelector]
    public string targetTag;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {

            // Copy only X from the other, keep current Y and Z
            // transform.position = new Vector3(otherPos.x, myPos.y, myPos.z);
            other.transform.parent = transform;
            if (other.GetComponent<Rigidbody>())
            {
                other.GetComponent<Rigidbody>().useGravity = false;
            }
        }
        Debug.Log($"Entered trigger with: {other.gameObject.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == targetTag)
        {

            // Copy only X from the other, keep current Y and Z
            // transform.position = new Vector3(otherPos.x, myPos.y, myPos.z);
            other.transform.parent = null;
            if (other.GetComponent<Rigidbody>())
            {
                other.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        Debug.Log($"Exited trigger with: {other.gameObject.name}");
    }
}
