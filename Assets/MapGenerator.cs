using UnityEngine;
using System.Collections.Generic;
public enum RowType
{
    Grass,
    Road,
    Water
}
 
public class MapGenerator : MonoBehaviour
{
    public GameObject grassRowPrefab;
    public GameObject roadRowPrefab;
    public GameObject waterRowPrefab;


    public List<GameObject> obstacles = new List<GameObject>();
    public List<GameObject> collectables = new List<GameObject>(); 


    public Transform player;
    public int rowsAhead = 20;
    public float rowSpacing = 1f;

    private int currentRow = 0;
    private List<GameObject> spawnedRows = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < rowsAhead; i++)
        {
            GenerateNextRow();
        }
    }
    void Update()
    {
        if (player.position.z > (currentRow - rowsAhead / 2) * rowSpacing)
        {
            GenerateNextRow();
        }
    }
    public void GenerateNextRow()
    {
        RowType rowType = GetRandomRowType();

        GameObject rowPrefab = GetRowPrefab(rowType);
        Vector3 spawnPos = new Vector3(0, 0, currentRow * rowSpacing);
        GameObject newRow = Instantiate(rowPrefab, spawnPos, Quaternion.identity, transform);

        spawnedRows.Add(newRow);
        currentRow++;
    }

    RowType GetRandomRowType()
    {
        int value = Random.Range(0, 3); // 0-Grass, 1-Road, 2-Water
        return (RowType)value;
    }

    GameObject GetRowPrefab(RowType type)
    {
        switch (type)
        {
            case RowType.Grass: return grassRowPrefab;
            case RowType.Road: return roadRowPrefab;
            case RowType.Water: return waterRowPrefab;
            default: return grassRowPrefab;
        }
    }
}
