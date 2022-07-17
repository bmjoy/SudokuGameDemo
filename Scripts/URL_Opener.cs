using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URL_Opener : MonoBehaviour
{
    public string url;

    public void Open()
    { 
        Application.OpenURL(url);
    }
}
