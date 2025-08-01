using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Класс для обработки событий с вязанных со строкой меню
/// </summary>
public class ShowHideMenu: MonoBehaviour, ISelectHandler, IDeselectHandler
{
    /// <summary>
    /// Объект в котором хранятся пункт меню
    /// </summary>
    [SerializeField]
    private GameObject target;
    /// <summary>
    /// Функция насдедованная от интерфейса ISelectHandler вызывающаяся при выборе пункта меню
    /// </summary>
    /// <param name="eventData">Данные получаемые при выполнении функции</param>
    public void OnSelect(BaseEventData eventData) => target.SetActive(true);
    /// <summary>
    /// Функция насдедованная от интерфейса IDeselectHandler вызывающаяся при выборе пункта меню
    /// </summary>
    /// <param name="eventData">Данные получаемые при выполнении функции</param>
    public void OnDeselect(BaseEventData eventData) => target.SetActive(false);
}