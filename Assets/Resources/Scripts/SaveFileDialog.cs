using SimpleFileBrowser;
using System.Collections;
using UnityEngine;
/// <summary>
/// ����� ��� ������ ����������� ���� � ����������� ������ �����
/// </summary>
public class SaveFile : MonoBehaviour
{
    private protected string tempaleteName = "Pc info.html";
    /// <summary>
    /// ������� ��� ������ ����������� ����, ������� ����� ������������
    /// </summary>
    public virtual void SaveFileBtn() => StartCoroutine(ShowSaveDialogCoroutine());
    /// <summary>
    /// �������� ������� �������� �� ���������� ���������� ����������� ����
    /// </summary>
    /// <returns>��������� ������ ����</returns>
    private IEnumerator ShowSaveDialogCoroutine()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("��� ��������", ".html"), new FileBrowser.Filter("Xml ����", ".xml"));
        FileBrowser.SetDefaultFilter(".html");
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, initialFilename: tempaleteName, title:"��������� ���");
        if (FileBrowser.Success)
            OnFilesSelected(FileBrowser.Result[0]);
    }
    /// <summary>
    /// ������� ��� ���������� �������� � ��������� ����, ������� ����� ������������
    /// </summary>
    /// <param name="path">���� � �����</param>
    private protected virtual void OnFilesSelected(string path) => Debug.Log(path);
}