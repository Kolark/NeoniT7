using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class Hoja : MonoBehaviour
{

    [SerializeField] Scrollbar scrollbar;
    [SerializeField] float[] scrollbarValue;

#if UNITY_EDITOR
    [SerializeField] bool onvalidateeeeee;
#endif

    [SerializeField] List<Viñeta> viñetas = new List<Viñeta>();
    [SerializeField] 
    int activated = 0;
    int currentRow = 0;
    bool hasFinished = false;
    public Action onCompleted;
    private void Awake()
    {
        for (int i = 0; i < viñetas.Count; i++)
        {
            viñetas[i].INIT();
        }
    }

    private void Start()
    {
        NextVignete();
    }
    public void NextVignete()
    {
        if(activated < viñetas.Count)
        {
            if (!viñetas[activated].HasCompletedAllTexts)
            {
                MoveScrollbar(viñetas[activated].row);
                currentRow = viñetas[activated].row;
                viñetas[activated].DoTweening();
                
                if (viñetas[activated].HasCompletedAllTexts)
                {
                    activated++;
                }
            }

            //if(!(activated < viñetas.Count))
            //{
            //    Debug.Log("ULTIMO");
            //    onCompleted?.Invoke();
            //}
        }
        else 
        {
            Debug.Log("ULTIMO");
            onCompleted?.Invoke();
        }
    }

    void MoveScrollbar(int row)
    {
        DOTween.To(() => scrollbar.value, x => scrollbar.value = x,scrollbarValue[row], 0.5f).SetEase(Ease.InSine);
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
