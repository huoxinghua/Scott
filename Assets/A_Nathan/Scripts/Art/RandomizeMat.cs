using UnityEngine;

public class RandomizeMat : MonoBehaviour
{
    public Color objectColor;

    [SerializeField] private Renderer renderer;
    [SerializeField] string nameOfParameter;
    [SerializeField] float minPar;
    [SerializeField] float maxPar;

    void Start()
    {
        MaterialPropertyBlock _mpb = new MaterialPropertyBlock();

        // Access the object's renderer

        renderer.GetPropertyBlock(_mpb);

        // Set your custom value
        _mpb.SetFloat(nameOfParameter, Random.Range(minPar,maxPar)); 
        renderer.SetPropertyBlock(_mpb);    
    }
}
