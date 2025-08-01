using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using WMIForUnity;
/// <summary>
/// Класс для окна "Получение информации"
/// </summary>
public class GetInfoWindow : SaveFile
{
    /// <summary>
    /// Объект в котором хранятся флажки
    /// </summary>
    [SerializeField] private Transform box;
    /// <summary>
    /// Функция для вывода информации о выбранном(ых) копьютере(ах) на экран
    /// </summary>
    /// <exception cref="Exception">Пользователь не выбрал компьютер(ы)</exception>
    [SerializeField]
    private async void InfoOnScreen()
    {
        string[] ips = GetEndPoints();
        if (ips.Length == 0) throw new Exception("Вы не выбрали компьютер(ы)!");
        Debug.Log("getting data...");
        string path;
        foreach (string ip in ips)
        {
            path = Application.temporaryCachePath + ip + " info " + DateTime.Now.ToString("dd.MM.yyyy ss mm HH") + ".html";
            ListenerProgramm.ip = ip;
            await ListenerProgramm.GetPcInfoHtml(path);
            Application.OpenURL(path);
        }
    }
    /// <summary>
    /// Функция сохранении информации о выбранном(ых) копьютере(ах) в файл
    /// </summary>
    /// <exception cref="Exception">Пользователь не выбрал компьютер(ы)</exception>
    [SerializeField]
    private void SaveInfoInFile()
    {
        string[] ips = GetEndPoints();
        if (ips.Length == 0) throw new Exception("Вы не выбрали компьютер(ы)!");
        Debug.Log("getting data...");
        foreach (string ip in ips)
        {
            ListenerProgramm.ip = ip;
            tempaleteName = ip + " info " + DateTime.Now.ToString("dd.MM.yyyy ss mm HH") + ".html"; 
            SaveFileBtn();
        }
        
    }
    /// <summary>
    /// Функция которая выполняется после выбора пути, в окне выбора файла
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    protected private override async void OnFilesSelected(string path)
    {
        switch (path[path.LastIndexOf('.')..])
        {
            case ".html":
                Debug.Log("html selected!");
                await ListenerProgramm.GetPcInfoHtml(path);
            break;
            case ".xml":
                Debug.Log("xml selected!");
                XDocument temp = await Task.Run(ListenerProgramm.GetPcInfoXml);
                temp.Save(path);
            break;
        }
        Debug.Log("file data created!");
    }
    /// <summary>
    /// Функция для получения IP-адресов с конечной точкой, которые выбрал пользователь
    /// </summary>
    /// <returns>Массив IP-адресов с конечной точкой, которые выбрал пользователь</returns>
    private string[] GetEndPoints()
    {
        List<string> ipInText = new();
        foreach (Transform child in box)
            if(child.GetComponent<Toggle>().isOn)
                ipInText.Add(child.GetComponentInChildren<Text>().text);
        return ipInText.AsParallel().Select(str => str.Split(':')[0]).ToArray();
    }
}