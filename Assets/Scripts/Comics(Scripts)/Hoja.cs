using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hoja : MonoBehaviour
{

#if UNITY_EDITOR
    [SerializeField] bool onvalidateeeeee;
#endif

    [SerializeField] List<Viñeta> viñetas = new List<Viñeta>();

    int activated = 0;

    bool hasFinished = false;

    private void Awake()
    {
        for (int i = 0; i < viñetas.Count; i++)
        {
            viñetas[i].INIT();
        }
    }


    public void NextVignete()
    {
        if(activated < viñetas.Count)
        {
            if (!viñetas[activated].HasCompletedAllTexts)
            {
                viñetas[activated].DoTweening();
                if (viñetas[activated].HasCompletedAllTexts)
                {
                    activated++;
                }
            }
        }
    }


    private void OnValidate()
    {
        Image[] images = GetComponentsInChildren<Image>();
        int diff = Mathf.Abs(images.Length - viñetas.Count);
        if(images.Length < viñetas.Count)
        {
            for (int i = 0; i < diff; i++)
            {
                viñetas.RemoveAt(viñetas.Count - 1);
            }
        }
        else if(images.Length > viñetas.Count)
        {
            for (int i = 0; i < diff; i++)
            {
                int index = images.Length-diff + i;
                Viñeta viñeta = new Viñeta(images[index].rectTransform);
                viñetas.Add(viñeta);
            }
        }
    }
}
