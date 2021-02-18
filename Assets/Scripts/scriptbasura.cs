using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class scriptbasura : MonoBehaviour
{
    public void Test (Holi jeje) {
        if (jeje == Holi.RamirezGay) print("En efecto");
    }
    private void Start()
    {
    }
}

public enum Holi {
    RamirezPuto,
    RamirezPincheGordo,
    RamirezGay
}
