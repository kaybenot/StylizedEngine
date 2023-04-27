using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    [SerializeField] private ProgressBar progressBar;
    
    public void RaportProgress(float val, string text = null)
    {
        progressBar.Progress = val;
        if (text != null)
            progressBar.Text = text;
    }
}
