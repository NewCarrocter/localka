using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс для обработки события после выбора пункта в строке меню
/// </summary>
public class SelectOptionMenuItem : MonoBehaviour
{
    /// <summary>
    /// Окно которое нужно изменить
    /// </summary>
    [SerializeField]
    private GameObject target;
    /// <summary>
    /// Функция выполняется после нажатия на пункт в строке меню
    /// </summary>
    [SerializeField]
    private void ActionClick()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        target.transform.position = Vector3.zero;
        target.GetComponent<RectTransform>().sizeDelta = new Vector2(target.GetComponent<LayoutElement>().minWidth,target.GetComponent<LayoutElement>().minHeight);
        target.SetActive(true);
    }
}