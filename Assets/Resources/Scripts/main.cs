using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System;
public class Main : MonoBehaviour
{
    public static string savePath = Application.dataPath + @"/info.xml", ipAddress;
    public static event Action ArrayChangeEvent;
    private static byte[] _texture;
    internal static byte[] Texture
    {
        get { return _texture; }
        set
        {
            _texture = value;
            ArrayChangeEvent?.Invoke();
        }
    }
    public GameObject content;
    private GraphicRaycaster gr;
    private List<RaycastResult> results = new();
    private bool readyToMove;
    private int[] move;
    private Vector2 onClickPos, onClickSize, moveWindow, mouseLocalpoint, min;
    [SerializeField]
    private Transform settingsWindow, connectWindow;
    [SerializeField]
    private Texture2D resizeCursor, resizeCursor2;
    private bool f1 = true;
    void Awake()
    {
        Debug.Log(Application.dataPath);
        Application.logMessageReceivedThreaded += HandleException;
        //ArrayChangeEvent += SetImgToPC;
        //DontDestroyOnLoad(gameObject);
    }
    public void HandleException(string logString, string stackTrace, LogType type)
    {
        if (type == LogType.Exception)
        {
            GameObject errWindow = Resources.Load<GameObject>("Prefabs/errorWindow");
            errWindow.transform.GetChild(1).GetComponent<Text>().text = logString + '\n' + stackTrace.Trim();
            Instantiate(errWindow, transform);
        }
        if (type == LogType.Warning)
        {
            content.GetComponent<Text>().text += logString;
        }
    }
    private void OnApplicationQuit() => settingsWindow.GetComponent<Settings>().SaveSettingsData();
    private void Start()
    {
        gr = GetComponent<GraphicRaycaster>();
        try
        {
            using (StreamReader sr = new(Application.dataPath + @"/Path to the save file.txt", true))
            {
                savePath = sr.ReadLine();
            }
            if (string.IsNullOrEmpty(savePath))
                throw new Exception("Не получилось получить путь к файлу данных!");
            XDocument doc = XDocument.Load(savePath);
            settingsWindow.GetChild(3).GetComponent<Dropdown>().value = int.Parse(doc.Root.Attribute("theme").Value);
            string[] b = doc.Root.Element("IPsData").Elements("client").AsParallel().Select(e => e.Value).ToArray();
            if (b.Length == 0) throw new Exception("Не получилось получить IP-адреса с конечной точкой для клиента(ов)!");
            connectWindow.GetChild(0).GetChild(2).GetComponent<Button>().onClick.Invoke();
            Transform ab = connectWindow.GetChild(1).GetChild(0).GetChild(0);
            for (int i = 0; i < b.Length; i++)
            {
                ab.GetChild(i).GetChild(1).GetComponent<InputField>().text = b[i];
                ab.GetChild(i).GetChild(1).GetComponent<InputField>().onEndEdit.Invoke(b[i]);
            }
            connectWindow.GetChild(2).GetComponent<Button>().onClick.Invoke();
        }
        catch (Exception ex)
        {
            connectWindow.gameObject.SetActive(true);
            savePath = Application.dataPath + @"/info.xml"; 
            /*if (ex.GetType().ToString() == "FileNotFoundException")
            {
                ex.Message
            }*/
            throw ex;
        }
    }
    void Update()
    {
        if (readyToMove)
        {
            if (Input.GetMouseButton(0))
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out mouseLocalpoint);
                WindowControl();
            }
            if (Input.GetMouseButtonUp(0))
            {
                results.Clear();
                readyToMove = false;
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
        else if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            PointerEventData mouse = new(null) { position = Input.mousePosition };
            gr.Raycast(mouse, results);
            if (results.Count != 0)
            {
                if (results[0].gameObject.CompareTag("onEverything"))
                {
                    moveWindow = results[0].gameObject.transform.localPosition;
                    readyToMove = true;
                    onClickSize = results[0].gameObject.GetComponent<RectTransform>().sizeDelta;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out mouseLocalpoint);
                    onClickPos = mouseLocalpoint;
                    CheckDirection();
                }
                else
                    results.Clear();
            }
        }
    }
    private void CheckDirection()
    {
        Vector2 _ = onClickSize / 2 - (moveWindow - mouseLocalpoint);
        move = new int[2];
        min = new Vector2(results[0].gameObject.GetComponent<LayoutElement>().minWidth, results[0].gameObject.GetComponent<LayoutElement>().minHeight);
        if (_.x < 5 && _.y < 5)
        {
            move[0] = -1;
            move[1] = -1;
        }
        else if (_.x < 5 && _.y > onClickSize.y - 5)
        {
            move[0] = -1;
            move[1] = 1;
        }
        else if (_.x > onClickSize.x - 5 && _.y < 5)
        {
            move[0] = 1;
            move[1] = -1;
        }
        else if (_.x > onClickSize.x - 5 && _.y > onClickSize.y - 5)
        {
            move[0] = 1;
            move[1] = 1;
        }
        else
            move = null;
    }
    private void WindowControl()
    {
        if (move == null)
            results[0].gameObject.transform.localPosition = moveWindow + (mouseLocalpoint - onClickPos);
        else
        {
            Vector2 _ = new(onClickSize.x + (mouseLocalpoint.x - onClickPos.x) * move[0], onClickSize.y + (mouseLocalpoint.y - onClickPos.y) * move[1]);
            if (_.x < min.x)
                _.x = min.x;
            if (_.y < min.y)
                _.y = min.y;
            results[0].gameObject.GetComponent<RectTransform>().sizeDelta = _;
            results[0].gameObject.transform.localPosition = moveWindow + (mouseLocalpoint - onClickPos) / 2;
        }
    }
}
        