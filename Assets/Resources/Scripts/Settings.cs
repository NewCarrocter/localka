using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using System;
using System.Linq;
using SimpleFileBrowser;
/// <summary>
/// Класс предназначеный для окна с настройками
/// </summary>
public class Settings : MonoBehaviour
{
    /// <summary>
    /// Массив картинок значений переключателя
    /// </summary>
    [SerializeField]
    private Sprite[] sprites;
    /// <summary>
    /// Массив объектов которые находятся в окне настроек
    /// </summary>
    [SerializeField]
    private GameObject[] interactebles;
    /// <summary>
    /// Функция которая выполняется после нажатия на крнопку "Настройки по умолчанию"
    /// </summary>
    [SerializeField]
    private void ResetSettings()
    {
        Main.savePath = Application.dataPath + @"/info.xml";
        interactebles[0].GetComponent<Dropdown>().value = 0;
        interactebles[1].GetComponentInChildren<Image>().sprite = sprites[1];
        interactebles[2].GetComponentInChildren<Image>().sprite = sprites[0];
        interactebles[3].GetComponent<InputField>().text = "";
        interactebles[3].GetComponent<InputField>().interactable = false;
    }
    /// <summary>
    /// Функция которая выполняется после нажатия на переключатель со значением "Да"
    /// </summary>
    [SerializeField]
    private void RadioClick()
    {
        interactebles[1].GetComponentInChildren<Image>().sprite = sprites[1];
        interactebles[2].GetComponentInChildren<Image>().sprite = sprites[0];
        interactebles[3].GetComponent<InputField>().interactable = true;
    }
    /// <summary>
    /// Функция которая выполняется после нажатия на переключатель со значением "Нет"
    /// </summary>
    [SerializeField]
    private void RadioClick2()
    {
        interactebles[2].GetComponentInChildren<Image>().sprite = sprites[1];
        interactebles[1].GetComponentInChildren<Image>().sprite = sprites[0];
        interactebles[3].GetComponent<InputField>().interactable = false;
    }
    /// <summary>
    /// Функция которая выполняется после того как пользователь ввёл абсолютный путь в строке для ввода
    /// </summary>
    /// <param name="input">Строка которую ввёл пользователь</param>
    [SerializeField]
    private void Field(string input)
    {
        if (File.Exists(input))
            Main.savePath = input;
        else if (Directory.Exists(input))
            Main.savePath = input + @"/info.xml";
        else
        {
            interactebles[3].GetComponent<InputField>().text = "";
            interactebles[3].GetComponent<InputField>().placeholder.GetComponent<Text>().color = Color.red;
            interactebles[3].GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Указан не существующий абсолютный путь!";
            return;
        }
        Main.savePath = Main.savePath.Replace("//", "/");
        if (!File.Exists(Application.dataPath + @"/Path to the save file.txt"))
            _ = new FileInfo(Application.dataPath + @"/Path to the save file.txt")
            {
                IsReadOnly = false
            };
        using (StreamWriter sw = new(Application.dataPath + @"/Path to the save file.txt", false))
        {
            sw.Write(Main.savePath);
        }
        _ = new FileInfo(Application.dataPath + @"/Path to the save file.txt")
        {
            IsReadOnly = true
        };
    }
    /// <summary>
    /// Функция которая выполняется после выбора стиля приложения в списке стилей
    /// </summary>
    /// <param name="value">Стиль в числовом значении</param>
    [SerializeField]
    static internal void ChangeTheme(int value)
    {
        Color a, b, c, d, e;
        if (value == 0)
        {
            FileBrowser.Skin = Resources.Load<UISkin>("Skins/LightSkin");
            a = Color.white;
            b = Color.black;
            Resources.Load<GameObject>("Prefabs/errorWindow").GetComponent<Image>().color = a;
            Resources.Load<GameObject>("Prefabs/errorWindow").GetComponent<Outline>().effectColor = b;
            Resources.Load<GameObject>("Prefabs/errorWindow").transform.GetChild(0).GetComponent<Text>().color = b;
            c = new(50 / 255f, 50 / 255f, 50 / 255f);// 50 50 50
            d = new(100 / 255f, 100 / 255f, 100 / 255f);// 100 100 100
            e = new(85 / 255f, 1, 85 / 255f, 100 / 255f);// 85 85 85
        }
        else
        {
            FileBrowser.Skin = Resources.Load<UISkin>("Skins/DarkSkin");
            a = Color.black;
            b = Color.white;
            Resources.Load<GameObject>("Prefabs/errorWindow").GetComponent<Image>().color = a;
            Resources.Load<GameObject>("Prefabs/errorWindow").GetComponent<Outline>().effectColor = b;
            Resources.Load<GameObject>("Prefabs/errorWindow").transform.GetChild(0).GetComponent<Text>().color = b;
            c = new(245 / 255f, 245 / 255f, 245 / 255f);//245 245 245
            d = new(200 / 255f, 200 / 255f, 200 / 255f);//200 200 200
            e = new(200 / 255f, 200 / 255f, 200 / 255f);//200 200 200
        }
        getAll(Camera.main.transform.GetChild(0), a, b, c, d, e);
        getAll(Resources.Load<GameObject>("Prefabs/checkbox").transform, a, b, c, d, e);
        getAll(Resources.Load<GameObject>("Prefabs/pc").transform, a, b, c, d, e);
        getAll(Resources.Load<GameObject>("Prefabs/clientPrefab").transform, a, b, c, d, e);
    }
    /// <summary>
    /// Функция которая меняет стиль для всех элиментов приложеня
    /// </summary>
    /// <param name="a">Объект для изменения</param>
    /// <param name="img">Цвет на который нужно изменить задний фон окн</param>
    /// <param name="txt">Цвет на который нужно изменить текст</param>
    /// <param name="hig">Цвет на который нужно изменить выделение в строке ввода</param>
    /// <param name="press">Цвет на который нужно изменить выделение самой строки ввода</param>
    /// <param name="pl">Цвет на который нужно изменить текст подсказки ввода в строке ввода</param>
    static private void getAll(Transform a, Color img, Color txt, Color hig, Color press, Color pl)
    {
        Outline d = null;
        Image b = null;
        Text c = null;
        InputField f = null;
        foreach (Transform t in a.transform)
        {
            if (t.childCount != 0)
                getAll(t, img, txt, hig, press, pl);
            if (t.TryGetComponent<InputField>(out f))
            {
                f.textComponent.color = img;
                f.placeholder.color = pl;
                ColorBlock y = f.colors;
                y.normalColor = txt;
                y.selectedColor = txt;
                y.pressedColor = press;
                y.highlightedColor = hig;
                f.colors = y;
            }
            if (t.TryGetComponent<Outline>(out d))
                d.effectColor = txt;
            if (t.TryGetComponent<Image>(out b) && b.sprite == null && !t.CompareTag("selected"))
                b.color = img;
            else if (t.TryGetComponent<Text>(out c))
                c.color = txt;
        }
    }
    /// <summary>
    /// Функция которая отвечает за процесс сохранения данных
    /// </summary>
    internal void SaveSettingsData()
    {
        if (!interactebles[3].GetComponent<InputField>().interactable)
            return;
        if (File.Exists(Application.dataPath + @"/Path to the save file.txt"))
            _ = new FileInfo(Application.dataPath + @"/Path to the save file.txt")
            {
                IsReadOnly = false
            };
        using (StreamWriter sw = new(Application.dataPath + @"/Path to the save file.txt", false))
        {
            sw.Write(Main.savePath);
        }
        _ = new FileInfo(Application.dataPath + @"/Path to the save file.txt")
        {
            IsReadOnly = true
        };
        XDocument doc;
        try
        {
            if (!File.Exists(Main.savePath))
                throw new Exception();
            _ = new FileInfo(Main.savePath) { IsReadOnly = false };
            doc = XDocument.Load(Main.savePath);
            XElement elem = doc.Root ?? new XElement("data");
            XAttribute att = elem.Attribute("theme") ?? new XAttribute("theme", interactebles[0].GetComponent<Dropdown>().value.ToString());
            att.Value = interactebles[0].GetComponent<Dropdown>().value.ToString();
            elem = elem.Element("IPsData") ?? new XElement("IPsData");
            att.Value = NetworkClass.self.ToString();
            elem.RemoveNodes();
        }
        catch
        {
            doc = new XDocument(new XElement("data",
                new XAttribute("theme", interactebles[0].GetComponent<Dropdown>().value.ToString()),
                new XElement("IPsData")));
        }
        XElement cl = doc.Root.Element("IPsData");
        foreach (var a in NetworkClass.clients.Select(x => x.ToString()))
            cl.Add(new XElement("client", a));
        doc.Save(Main.savePath);
        _ = new FileInfo(Main.savePath)
        {
            IsReadOnly = true
        };
    }
}