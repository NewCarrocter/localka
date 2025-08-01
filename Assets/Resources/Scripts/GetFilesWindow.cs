using SimpleFileBrowser;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Класс для окна выбора файла
/// </summary>
public class GetFilesWindow : GetCheckboxes
{
    private string[] files;
    private string messege = "";
    /// <summary>
    /// Функция которая выполняется после нажатия на кнопку "Отправить файлы"
    /// </summary>
    [SerializeField]
    private void Send()
    {
        IPEndPoint[] clientsSelected = GetEndPoints();
        if (clientsSelected.Length == 0)
            throw new Exception("PCs not selected!");
        else if (messege.Length == 0)
            throw new Exception("File(s) not selected!");
        Debug.Log("sending");
        NetworkClass.SendCommand(NetworkClass.Command.sendFile, clientsSelected, messege);
        messege = "";
    }
    /// <summary>
    /// Функция для запуска диалогового окна
    /// </summary>
    [SerializeField]
    private void LoadFileBtn() => StartCoroutine(ShowLoadDialogCoroutine());
    /// <summary>
    /// Корутина для корректной обработки выбора файла
    /// </summary>
    /// <returns>Путь к файлу или файлам</returns>
    private IEnumerator ShowLoadDialogCoroutine()
    {
        FileBrowser.SetFilters(true);
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, title: "Выбрете файл или файлы", loadButtonText:"Выбрать");
        if (FileBrowser.Success)
            OnFilesSelected(FileBrowser.Result);
    }
    /// <summary>
    /// Функция выполняемая после удачного выбора файла или файлов
    /// </summary>
    /// <param name="path">Массив пути к файлу или файлам</param>
    private void OnFilesSelected(string[] path)
    {
        foreach (string file in path)
        {
            if (new System.IO.FileInfo(file).Length == 0)
            {
                throw new Exception("Empty file!");
            }
            else
            {
                messege += "|" + file;
            }
        }
    }
    /// <summary>
    /// Функция для получения IP-адресов с конечной точкой, которые выбрал пользователь
    /// </summary>
    /// <returns>Массив IP-адресов с конечной точкой</returns>
    private IPEndPoint[] GetEndPoints()
    {
        List<string> ipInText = new();
        foreach (Transform child in box)
            if (child.GetComponent<Toggle>().isOn)
                ipInText.Add(child.GetComponentInChildren<Text>().text);
        return ipInText.AsParallel().Select(str => str.Split(':')).Select(str => new IPEndPoint(IPAddress.Parse(str[0]), int.Parse(str[1]))).ToArray();
    }
}