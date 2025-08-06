using UnityEngine;

public class RandoColor : MonoBehaviour
{
   // public Color objectColor;

    [SerializeField] private Renderer renderer;
    [SerializeField] string nameOfParameter;
    [SerializeField] float maxToPlayWith;
    float rVal;
    float gVal;
    float bVal;

    void Start()
    {
        MaterialPropertyBlock _mpb = new MaterialPropertyBlock();

        // Access the object's renderer

        renderer.GetPropertyBlock(_mpb);
        float randR;
        float randG;
        float randB;
        int order;
        order = Random.Range(0, 6);
        if(order == 0)
        {
            randR = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randR;
            randG = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randG;
            randB = Mathf.Clamp(maxToPlayWith, 0, 255);
            maxToPlayWith -= randB;
        }
        else if(order == 1)
        {
            randR = Random.Range(0, Mathf.Clamp(maxToPlayWith,0,255));
            maxToPlayWith -= randR;
            randB = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randB;
            randG = Mathf.Clamp(maxToPlayWith, 0, 255); 
            maxToPlayWith -= randG;
           
        }
        else if( order == 2)
        {
            randB = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randB;
            randR = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randR;
            randG = Mathf.Clamp(maxToPlayWith, 0, 255); 
            maxToPlayWith -= randG;
        }
        else if(order == 3)
        {
            randB = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randB;
            randG = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randG;
            randR = Mathf.Clamp(maxToPlayWith, 0, 255); 
            maxToPlayWith -= randR;
        }
        else if (order == 4)
        {
            randG = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randG;
            randB = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randB;
            randR = Mathf.Clamp(maxToPlayWith, 0, 255); 
            maxToPlayWith -= randR;
        }
        else
        {
            randG = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randG;
            randR = Random.Range(0, Mathf.Clamp(maxToPlayWith, 0, 255));
            maxToPlayWith -= randR;
            randB  = Mathf.Clamp(maxToPlayWith, 0, 255); 
            maxToPlayWith -= randB;
        }
        float rVal = randR;
        float gVal = randG;
        float bVal = randB;
        // Set your custom value
        _mpb.SetColor(nameOfParameter, new Color(rVal/255,gVal/255,bVal / 255,1));
        renderer.SetPropertyBlock(_mpb);
    }
}
