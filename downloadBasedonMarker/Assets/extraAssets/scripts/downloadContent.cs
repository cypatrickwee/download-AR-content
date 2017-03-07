using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using Vuforia;
using System.Security.Cryptography;
//temporary dropbox url
//https://dl.dropboxusercontent.com/s/itpazdiif7uyqcy/medieval_building.1?dl=0
//https://dl.dropboxusercontent.com/s/ee7d5wn9qx1n9j9/testbake1.1?dl=0
public class downloadContent : DefaultTrackableEventHandler
{
    private float loadingImageObj_counter = 0f;
    private float loadingImageColor_counter = 0f;
    private float currentAmount;
    //private DefaultTrackableEventHandler trackedName;
    private string assetName;
    public Color[] colorValue = new Color[8];
    public UnityEngine.UI.Image[] arrayImage = new UnityEngine.UI.Image[8];
    public string linkURL_design;
    public string linkURL_building;
    public int version;
    public int currentVersion;
    public GameObject[] surfacedObj = new GameObject[2];
    public CanvasGroup canvasDownload;
    public Transform loadingGraphics;
    public Transform textPercentage;



    void OnEnable()
    {
        //trackedName = (DefaultTrackableEventHandler)surfacedObj[0].GetComponent(typeof(DefaultTrackableEventHandler));
        //if (trackedName.AssoName == "marker_1")
        if (DefaultTrackableEventHandler.associateName == "marker_1")
        {
            assetName = "testbake1";
            //StartCoroutine(CacheLoadDOwnload(assetName));
            StartCoroutine(DownloadStream(assetName, linkURL_design));
        }
        //else if (trackedName.AssoName == "marker_2")
        else if (DefaultTrackableEventHandler.associateName == "marker_2")
        {
            assetName = "medieval_building";
            //StartCoroutine(CacheLoadDOwnload(assetName));
            StartCoroutine(DownloadStream(assetName, linkURL_building));
        }
    }
    void Start()
    {
        //trackedName = (DefaultTrackableEventHandler)imageMarker[0].GetComponent(typeof(DefaultTrackableEventHandler)); not necessary

    }

    void Update()
    {


        //image.transform.Rotate(new Vector3(0, 0, (Time.deltaTime * 500)));
        //image_2.transform.Rotate(new Vector3(0, 0, (Time.deltaTime * 70)));
        //Time.deltaTime (time or seconds took to complete one frame)

        loadingImageObj_counter += Time.deltaTime * 10;
        loadingImageColor_counter += Time.deltaTime * 5;
        if (loadingImageObj_counter >= 8)
        {
            loadingImageObj_counter = 0;
            //Debug.Log(timeValue);
        }
        if (loadingImageColor_counter >= 8)
        {
            loadingImageColor_counter = 0;
        }

        arrayImage[7 - (int)loadingImageObj_counter].color = colorValue[(int)loadingImageColor_counter];

    }
    private IEnumerator DownloadStream(string assetBundleName, string bundleURL)
    {
        Debug.Log("Enter Start Coroutine");

        //if the required assetbundle files do not exist(new download)
        if (!File.Exists(Application.persistentDataPath + "/" + assetBundleName + "." + version.ToString()))
        {
            #region DOWNLOAD_ALGO
            //proceed to download
            using (WWW www = new WWW(bundleURL))
            {
                yield return null;
                if (www.error != null)
                    throw new Exception("WWW download had an error:" + www.error);
                #region PROGRESS
                //while not complete
                while (!www.isDone)
                {
                    yield return null;
                    float pValues = www.progress * 100;
                    if (pValues < 100)
                    {
                        canvasDownload.alpha = 1f;
                        currentAmount = www.progress * 100;
                        textPercentage.GetComponent<Text>().text = "Loading " + ((int)currentAmount).ToString() + " %";
                        //loadingObject.gameObject.SetActive(true);

                    }
                    else
                    {
                        textPercentage.GetComponent<Text>().text = "Done";
                        //loadingObject.gameObject.SetActive(false);
                        StartCoroutine(WaitForRequest());
                        canvasDownload.alpha = 0f;
                    }
                    loadingGraphics.GetComponent<UnityEngine.UI.Image>().fillAmount = currentAmount / 100;
                }
                yield return www;
                #endregion //PROGRESS

                //write all the bytes to the local phone storage once the download is completed
                System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + assetBundleName + "." + version.ToString(), www.bytes);
                //load the bundle from local drive
                AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + assetBundleName + "." + version.ToString());
                GameObject cube = Instantiate(bundle.LoadAsset(assetBundleName)) as GameObject;
                //cube.transform.position = new Vector3(1.5f, 0f, 0f);
                if (assetBundleName == "testbake1")
                {
                    cube.transform.parent = surfacedObj[0].transform;
                    cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                    cube.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                }
                else if (assetBundleName == "medieval_building")
                {
                    cube.transform.parent = surfacedObj[1].transform;
                    cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                    cube.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                //cube.transform.position = new Vector3(0f, 0f, 0f);
                // unload the assetbundle, freeing memory
                bundle.Unload(false);
            }//by implement using, we can ensure www.dispose is called after the download
            #endregion DOWNLOAD_ALGO
        }
        //if the file exist
        else
        {
            if (version < currentVersion)
            {
                using (WWW www = new WWW(bundleURL))
                {
                    if (www.error != null)
                        throw new Exception("WWW download had an error:" + www.error);
                    #region PROGRESS
                    //while not complete
                    while (!www.isDone)
                    {
                        yield return null;
                        float pValues = www.progress * 100;
                        if (pValues < 100)
                        {
                            canvasDownload.alpha = 1f;
                            currentAmount = www.progress * 100;
                            textPercentage.GetComponent<Text>().text = "Loading " + ((int)currentAmount).ToString() + " %";
                            //loadingObject.gameObject.SetActive(true);

                        }
                        else
                        {
                            textPercentage.GetComponent<Text>().text = "Done";
                            //loadingObject.gameObject.SetActive(false);
                            StartCoroutine(WaitForRequest());
                            canvasDownload.alpha = 0f;
                        }
                        loadingGraphics.GetComponent<UnityEngine.UI.Image>().fillAmount = currentAmount / 100;
                    }
                    yield return www;
                    #endregion //PROGRESS
                    version = currentVersion;
                    //write all the bytes to the local phone storage once the download is completed
                    System.IO.File.WriteAllBytes(Application.persistentDataPath + "/" + assetBundleName + "." + version.ToString(), www.bytes);
                    //load the bundle from local drive
                    AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + assetBundleName + "." + version.ToString());
                    GameObject cube = Instantiate(bundle.LoadAsset(assetBundleName)) as GameObject;

                    if (assetBundleName == "testbake1")
                    {
                        cube.transform.parent = surfacedObj[0].transform;
                        cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                        cube.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    }
                    else if (assetBundleName == "medieval_building")
                    {
                        cube.transform.parent = surfacedObj[1].transform;
                        cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                        cube.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    //cube.transform.position = new Vector3(0f, 0f, 0f);
                    bundle.Unload(false);
                }
            }
            else
            {
                AssetBundle bundle = AssetBundle.LoadFromFile(Application.persistentDataPath + "/" + assetBundleName + "." + version.ToString());
                GameObject cube = Instantiate(bundle.LoadAsset(assetBundleName)) as GameObject;
                //cube.transform.position = new Vector3(1.5f, 0f, 0f);
                if (assetBundleName == "testbake1")
                {
                    cube.transform.parent = surfacedObj[0].transform;
                    cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                    cube.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                }
                else if (assetBundleName == "medieval_building")
                {
                    cube.transform.parent = surfacedObj[1].transform;
                    cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                    cube.transform.localScale = new Vector3(1f, 1f, 1f);
                }
                //cube.transform.position = new Vector3(0f, 0f, 0f);
                bundle.Unload(false);
            }
        }
    }


    private IEnumerator CacheLoadDOwnload(string assetBundleName)
    {
        while (!Caching.ready)
            yield return null;

        // if the version is the same get from cache, else download
        using (WWW www = WWW.LoadFromCacheOrDownload("url_link", version))
        {
            yield return null;
            if (www.error != null)
                throw new Exception("WWW download had an error:" + www.error);
            //while not complete
            while (!www.isDone)
            {
                yield return null;
                float pValues = www.progress * 100;
                if (pValues < 100)
                {
                    canvasDownload.alpha = 1f;
                    currentAmount = www.progress * 100;
                    textPercentage.GetComponent<Text>().text = "Loading " + ((int)currentAmount).ToString() + " %";
                    //loadingObject.gameObject.SetActive(true);

                }
                else
                {
                    textPercentage.GetComponent<Text>().text = "Done";
                    //loadingObject.gameObject.SetActive(false);
                    StartCoroutine(WaitForRequest());
                    canvasDownload.alpha = 0f;
                }
                loadingGraphics.GetComponent<UnityEngine.UI.Image>().fillAmount = currentAmount / 100;
            }
            yield return www;
            AssetBundle bundle = www.assetBundle;
            GameObject cube = Instantiate(bundle.LoadAsset(assetBundleName)) as GameObject;
            //cube.transform.position = new Vector3(1.5f, 0f, 0f);

            if (assetBundleName == "testbake1")
            {
                cube.transform.parent = surfacedObj[0].transform;
                cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                cube.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            }
            else if (assetBundleName == "medieval_building")
            {
                cube.transform.parent = surfacedObj[1].transform;
                cube.transform.localPosition = new Vector3(0f, 0f, 0f);
                cube.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            bundle.Unload(true);

        }
    }

    private IEnumerator WaitForRequest()
    {
        yield return new WaitForSeconds(0.5f);
    }

    #region HASH_COMPUTATION
    static string computeHash(string fileName)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(fileName))
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }
    }
    #endregion //HASH_COMPUTATION
}