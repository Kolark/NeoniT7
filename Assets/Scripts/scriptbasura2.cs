using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
public class scriptbasura2 : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("collision Exit");
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collision Enter");
    }
}
