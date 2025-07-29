using UnityEngine; 
[ExecuteInEditMode]
public class GridSnapManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [TagSelector]
    public string[] tagsToSnap;
    public float gridSize = 1f;

    void Start()
    {
        foreach (string tag in tagsToSnap)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in taggedObjects)
            {
                if (!obj.GetComponent<GridSnap>())
                {
                    GridSnap snap = obj.AddComponent<GridSnap>();
                    snap.tagsToSnap = tagsToSnap;
                    snap.gridSize = gridSize;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
