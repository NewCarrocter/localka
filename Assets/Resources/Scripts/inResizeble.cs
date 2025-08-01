using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// Класс для работы с окнами
/// </summary>
public class inResizeble : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    /// <summary>
    /// Перемнная в которой хранится минимальный размер окна
    /// </summary>
    [SerializeField]
    private Vector2 minSize;
    /// <summary>
    /// Перемнная в которой хранится максимальный размер окна
    /// </summary>
    [SerializeField]
    private Vector2 maxSize;
    /// <summary>
    /// Переменная для хранения текущего размероа окна
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// Переменная для хранения текущей позиции указателя
    /// </summary>
    private Vector2 currentPointerPosition;
    /// <summary>
    /// Переменная для хранения пердыдущей позиции указателя
    /// </summary>
    private Vector2 previousPointerPosition;
    /// <summary>
    /// Метод вызывается во время загрузки экземпляра сценария
    /// </summary>
    void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
    }
    /// <summary>
    /// Функция используется для обнаружения событий с опущенным указателем
    /// </summary>
    /// <param name="data">"Событие с опущенным указателем</param>
    public void OnPointerDown(PointerEventData data)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out previousPointerPosition);
    }
    /// <summary>
    /// При перетаскивании эта функция будет вызываться при каждом перемещении курсора
    /// </summary>
    /// <param name="data">Событие с указателем</param>
    public void OnDrag(PointerEventData data)
    {
        if (rectTransform == null)
            return;

        Vector2 sizeDelta = rectTransform.sizeDelta;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out currentPointerPosition);
        Vector2 resizeValue = currentPointerPosition - previousPointerPosition;

        sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
        sizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
            Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
            );

        rectTransform.sizeDelta = sizeDelta;

        previousPointerPosition = currentPointerPosition;
    }
}
