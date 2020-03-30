// Copyright (c) 2016 Unity Technologies. MIT license - license_unity.txt
// #NVJOB FPS counter and graph. MIT license - license_nvjob.txt
// #NVJOB FPS counter and graph V2.0 - https://nvjob.github.io/unity/nvjob-fps-counter-and-graph
// #NVJOB Nicholas Veselov (independent developer) - https://nvjob.github.io


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.UI;

[HelpURL("https://nvjob.github.io/unity/nvjob-fps-counter-and-graph")]
[AddComponentMenu("#NVJOB/Tools/FPS Counter and Graph")]


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


public class FPSCounter : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    [Header("Settings")]
    public float timeUpdate = 1.0f;
    public int highestPossibleFPS = 300;
    public Color graphColor = new Color(1, 1, 1, 0.5f);
    public bool logWrite = true;

    [Header("Information")] // These variables are only information.
    public string HelpURL = "nvjob.github.io/unity/nvjob-fps-counter-and-graph";
    public string ReportAProblem = "nvjob.github.io/support";
    public string Patrons = "nvjob.github.io/patrons";

    //--------------

    GameObject counter, graph;
    Transform graphTr;
    Vector3Int allFps;
    Text counterText;
    float ofsetX;
    int lineCount;

    //--------------

    static WaitForSeconds stGraphUpdate;
    static GameObject[] stLines;
    static int stNumLines;


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void Awake()
    {
        //--------------

        Application.targetFrameRate = highestPossibleFPS;
        CreateCounter();

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void OnEnable()
    {
        //--------------

        StartCoroutine(DrawGraph());

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void OnApplicationQuit()
    {
        //--------------

        if (logWrite == true) StFPS.LogWrite();

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void Update()
    {
        //--------------

        // StFPS.Counter(Time Update).x - min fps
        // StFPS.Counter(Time Update).y - avg fps
        // StFPS.Counter(Time Update).z - max fps

        allFps = StFPS.Counter(timeUpdate);
        counterText.text = "MIN " + allFps.x.ToString() + " | AVG " + allFps.y.ToString() + " | MAX " + allFps.z.ToString();

        //-------------- 

        if (Input.GetKeyDown(KeyCode.F1)) // Hide Counter
        {
            counter.SetActive(!counter.activeSelf);
            graph.SetActive(!graph.activeSelf);
        }

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    IEnumerator DrawGraph()
    {
        //--------------

        while (true)
        {
            GameObject obj = GiveLine();
            Image img = obj.GetComponent<Image>();
            RectTransform imgRT = img.rectTransform;
            imgRT.anchorMin = new Vector2(ofsetX, 0);
            float anchorMaxY = 1.0f / highestPossibleFPS * allFps.y;
            if (anchorMaxY > 1) anchorMaxY = 1;
            imgRT.anchorMax = new Vector2(ofsetX + 0.01f, anchorMaxY);
            imgRT.offsetMax = imgRT.offsetMin = Vector2.zero;
            obj.SetActive(true);

            if (lineCount++ > 49) // The number of lines in the chart.
            {
                foreach (Transform child in graphTr) child.gameObject.SetActive(false);
                ofsetX = lineCount = 0;
            }
            else ofsetX += 0.02f; // The distance between the lines.

            yield return stGraphUpdate;
        }

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    void CreateCounter()
    {
        //--------------

        stGraphUpdate = new WaitForSeconds(timeUpdate);

        counter = transform.Find("Counter").gameObject;
        counterText = transform.Find("Counter/CounterText").gameObject.GetComponent<Text>();
        transform.Find("Counter/MaxFPSText").gameObject.GetComponent<Text>().text = highestPossibleFPS.ToString();
        transform.Find("Counter/HalfFPSText").gameObject.GetComponent<Text>().text = Mathf.Round(highestPossibleFPS * 0.5f).ToString();

        graphTr = transform.Find("Graph");
        graph = graphTr.gameObject;

        stNumLines = 100; // The number of lines in the chart. If there are not enough lines, increase this value.
        stLines = new GameObject[stNumLines];

        for (int i = 0; i < stNumLines; i++)
        {
            stLines[i] = new GameObject();
            stLines[i].SetActive(false);
            stLines[i].name = "Line_" + i;
            stLines[i].transform.parent = graphTr;
            Image img = stLines[i].AddComponent<Image>();
            RectTransform imgRT = img.rectTransform;
            imgRT.localScale = Vector3.one;
            imgRT.localPosition = Vector3.zero;
            img.color = graphColor;
        }

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    static GameObject GiveLine()
    {
        //--------------

        for (int i = 0; i < stNumLines; i++) if (!stLines[i].activeSelf) return stLines[i];
        return null;

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


public static class StFPS
{
    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    static List<float> fpsBuffer = new List<float>();
    static float fpsB, timeCounter;
    static Vector3Int fps;
    static List<Vector3Int> logWrite = new List<Vector3Int>();


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public static Vector3Int Counter(float timeUpdate)
    {
        //--------------

        int fpsBCount = fpsBuffer.Count;

        if (timeCounter <= timeUpdate)
        {
            timeCounter += Time.deltaTime;
            fpsBuffer.Add(1.0f / Time.deltaTime);
        }
        else
        {
            fps.x = Mathf.RoundToInt(fpsBuffer.Min());
            fps.z = Mathf.RoundToInt(fpsBuffer.Max());
            for (int f = 0; f < fpsBCount; f++) fpsB += fpsBuffer[f];
            fpsBuffer = new List<float> { 1.0f / Time.deltaTime };
            fpsB = fpsB / fpsBCount;
            fps.y = Mathf.RoundToInt(fpsB);
            fpsB = timeCounter = 0;
            if (Time.timeScale == 1) logWrite.Add(fps);
            else logWrite.Add(Vector3Int.zero);
        }

        if (Time.timeScale == 1 && fps.y > 0) return fps;
        else return Vector3Int.zero;

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public static void LogWrite()
    {
        //--------------

        string filePath = Directory.GetCurrentDirectory() + "/fpslog/";
        if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);
        string date = System.DateTime.Now.ToString("_yyyy.MM.dd_HH.mm.ss");
        filePath = filePath + "log" + date + ".csv";
        StreamWriter writer = new StreamWriter(filePath);
        writer.WriteLine("MIN;AVG;MAX");
        for (int i = 0; i < logWrite.Count; ++i) writer.WriteLine(logWrite[i].x + ";" + logWrite[i].y + ";" + logWrite[i].z);
        writer.Flush();
        writer.Close();

        //--------------
    }


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}