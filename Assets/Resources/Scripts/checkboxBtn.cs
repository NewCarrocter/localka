using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс для визуализации изменения значения элемента управления флажок
/// </summary>
public class checkboxBtn : MonoBehaviour
{
    /// <summary>
    /// Массив для хранения каринок показывающие значиение флажка
    /// </summary>
    public Sprite[] sprites;
    /// <summary>
    /// Функция для изменения значиения флажка
    /// </summary>
    public void OnAction()
    {
        if (GetComponentInChildren<Image>().sprite == sprites[0])
            GetComponentInChildren<Image>().sprite = sprites[1];
        else
            GetComponentInChildren<Image>().sprite = sprites[0];
    }
}