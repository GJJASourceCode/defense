using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceText : MonoBehaviour
{
    Text text;
    public static int resourceValue;
    
    void Start()
    {
        text = GetComponent<Text>();        
    }


    void Update()
    {
        text.text = "Resource : " + resourceValue;
    }
}
