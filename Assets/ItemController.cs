using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public RawImage texture;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }public void SetImage(Texture sprite)
    {
        texture.texture = sprite;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
