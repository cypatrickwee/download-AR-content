using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class loadingBar : MonoBehaviour {

    public CanvasGroup downloadProgress;
    private float fillValue = 0f;

    public Image radialLoading;//will be rotate
    public Image loadingComponent2;// will be filled 
	// Use this for initialization
	void Start () {
        downloadProgress.alpha = 0f;
        loadingComponent2.fillAmount = 0f;
        //radialLoading.GetComponent<Transform>().rotation = 
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
