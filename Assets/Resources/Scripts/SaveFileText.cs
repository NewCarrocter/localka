using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileText : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private void SaveFileBtn() => StartCoroutine(ShowSaveDialogCoroutine());
    private IEnumerator ShowSaveDialogCoroutine()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("��� ����", ".log"), new FileBrowser.Filter("��������� ����", ".txt"));
        FileBrowser.SetDefaultFilter(".log");
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, initialFilename: "Other PC.log", title: "��������� ���", saveButtonText: "���������");
        if (FileBrowser.Success)
            OnFilesSelected(FileBrowser.Result[0]);
    }
    private void OnFilesSelected(string path)
    {
        using StreamWriter sw = new(path, false);
            sw.WriteLine(text.text);
    }
}