using UnityEngine;
    
[ExecuteInEditMode]
public class GridSnap : MonoBehaviour
{

    public float gridSize = 1f;
    [TagSelector]
    public string[] tagsToSnap;

    private void Update()
    {
        if (!Application.isPlaying)
        {
            foreach (string tag in tagsToSnap)
            {
                if (CompareTag(tag))
                {
                    SnapToGrid();
                    break;
                }
            }
        }
    }

    private void SnapToGrid()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.z = Mathf.Round(pos.z / gridSize) * gridSize;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Entered trigger with: {other.gameObject.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exited trigger with: {other.gameObject.name}");
    }
}
