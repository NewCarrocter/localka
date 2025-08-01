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
/// ����� ��� ���� ������ �����
/// </summary>
public class GetFilesWindow : GetCheckboxes
{
    private string[] files;
    private string messege = "";
    /// <summary>
    /// ������� ������� ����������� ����� ������� �� ������ "��������� �����"
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
    /// ������� ��� ������� ����������� ����
    /// </summary>
    [SerializeField]
    private void LoadFileBtn() => StartCoroutine(ShowLoadDialogCoroutine());
    /// <summary>
    /// �������� ��� ���������� ��������� ������ �����
    /// </summary>
    /// <returns>���� � ����� ��� ������</returns>
    private IEnumerator ShowLoadDialogCoroutine()
    {
        FileBrowser.SetFilters(true);
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, true, title: "������� ���� ��� �����", loadButtonText:"�������");
        if (FileBrowser.Success)
            OnFilesSelected(FileBrowser.Result);
    }
    /// <summary>
    /// ������� ����������� ����� �������� ������ ����� ��� ������
    /// </summary>
    /// <param name="path">������ ���� � ����� ��� ������</param>
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
    /// ������� ��� ��������� IP-������� � �������� ������, ������� ������ ������������
    /// </summary>
    /// <returns>������ IP-������� � �������� ������</returns>
    private IPEndPoint[] GetEndPoints()
    {
        List<string> ipInText = new();
        foreach (Transform child in box)
            if (child.GetComponent<Toggle>().isOn)
                ipInText.Add(child.GetComponentInChildren<Text>().text);
        return ipInText.AsParallel().Select(str => str.Split(':')).Select(str => new IPEndPoint(IPAddress.Parse(str[0]), int.Parse(str[1]))).ToArray();
    }
}