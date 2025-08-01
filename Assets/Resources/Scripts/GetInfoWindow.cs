using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using WMIForUnity;
/// <summary>
/// ����� ��� ���� "��������� ����������"
/// </summary>
public class GetInfoWindow : SaveFile
{
    /// <summary>
    /// ������ � ������� �������� ������
    /// </summary>
    [SerializeField] private Transform box;
    /// <summary>
    /// ������� ��� ������ ���������� � ���������(��) ���������(��) �� �����
    /// </summary>
    /// <exception cref="Exception">������������ �� ������ ���������(�)</exception>
    [SerializeField]
    private async void InfoOnScreen()
    {
        string[] ips = GetEndPoints();
        if (ips.Length == 0) throw new Exception("�� �� ������� ���������(�)!");
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
    /// ������� ���������� ���������� � ���������(��) ���������(��) � ����
    /// </summary>
    /// <exception cref="Exception">������������ �� ������ ���������(�)</exception>
    [SerializeField]
    private void SaveInfoInFile()
    {
        string[] ips = GetEndPoints();
        if (ips.Length == 0) throw new Exception("�� �� ������� ���������(�)!");
        Debug.Log("getting data...");
        foreach (string ip in ips)
        {
            ListenerProgramm.ip = ip;
            tempaleteName = ip + " info " + DateTime.Now.ToString("dd.MM.yyyy ss mm HH") + ".html"; 
            SaveFileBtn();
        }
        
    }
    /// <summary>
    /// ������� ������� ����������� ����� ������ ����, � ���� ������ �����
    /// </summary>
    /// <param name="path">���� � �����</param>
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
    /// ������� ��� ��������� IP-������� � �������� ������, ������� ������ ������������
    /// </summary>
    /// <returns>������ IP-������� � �������� ������, ������� ������ ������������</returns>
    private string[] GetEndPoints()
    {
        List<string> ipInText = new();
        foreach (Transform child in box)
            if(child.GetComponent<Toggle>().isOn)
                ipInText.Add(child.GetComponentInChildren<Text>().text);
        return ipInText.AsParallel().Select(str => str.Split(':')[0]).ToArray();
    }
}