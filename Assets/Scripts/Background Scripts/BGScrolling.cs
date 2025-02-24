using UnityEngine;

public class BGScrolling : MonoBehaviour
{
    public float scrollSpeed;

    private Material material;
    private Vector2 offset = Vector2.zero;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        offset = material.GetTextureOffset("_MainTex");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        offset = material.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        offset.y += scrollSpeed * Time.deltaTime;
        material.SetTextureOffset("_MainTex", offset);
    }
}