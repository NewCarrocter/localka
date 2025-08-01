using SimpleFileBrowser;
using System.Collections;
using UnityEngine;
/// <summary>
/// Класс для вызова диалогового окна с назначением выбора файла
/// </summary>
public class SaveFile : MonoBehaviour
{
    private protected string tempaleteName = "Pc info.html";
    /// <summary>
    /// Функция для вызова диалогового окна, которую можно перезаписать
    /// </summary>
    public virtual void SaveFileBtn() => StartCoroutine(ShowSaveDialogCoroutine());
    /// <summary>
    /// Корутина каторая отвечает за функционал выполнения диалогового окна
    /// </summary>
    /// <returns>Результат вызова окна</returns>
    private IEnumerator ShowSaveDialogCoroutine()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Веб страница", ".html"), new FileBrowser.Filter("Xml файл", ".xml"));
        FileBrowser.SetDefaultFilter(".html");
        yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, initialFilename: tempaleteName, title:"Сохранить как");
        if (FileBrowser.Success)
            OnFilesSelected(FileBrowser.Result[0]);
    }
    /// <summary>
    /// Функция для выполнения действий с выбранным путём, которую можно перезаписать
    /// </summary>
    /// <param name="path">Путь к файлу</param>
    private protected virtual void OnFilesSelected(string path) => Debug.Log(path);
}