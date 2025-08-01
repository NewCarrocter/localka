using UnityEngine;
/// <summary>
/// Класс для открытия или закрытия окон и различных меню приложения
/// </summary>
public class Close : MonoBehaviour
{
    /// <summary>
    /// Перемная для хранения объекта которого нужно открыть или закрыть
    /// </summary>
    public GameObject target;
    /// <summary>
    /// Функция для открытия или закрытия объекта который ссылается переменная target
    /// </summary>
    public void DoAction() => target.SetActive(!target.activeSelf);
}