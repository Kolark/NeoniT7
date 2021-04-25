using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Comic : MonoBehaviour
{
    [SerializeField] Hoja[] hojas;
    int sheetIndex = 0;

    bool hasFinished = false;

    public void Step()
    {
        if (hasFinished)
        {
            hojas[sheetIndex].NextVignete();
            hasFinished = sheetIndex < hojas.Length;
        }
    }


}
