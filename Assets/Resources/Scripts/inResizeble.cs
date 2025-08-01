using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ����� ��� ������ � ������
/// </summary>
public class inResizeble : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    /// <summary>
    /// ��������� � ������� �������� ����������� ������ ����
    /// </summary>
    [SerializeField]
    private Vector2 minSize;
    /// <summary>
    /// ��������� � ������� �������� ������������ ������ ����
    /// </summary>
    [SerializeField]
    private Vector2 maxSize;
    /// <summary>
    /// ���������� ��� �������� �������� �������� ����
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// ���������� ��� �������� ������� ������� ���������
    /// </summary>
    private Vector2 currentPointerPosition;
    /// <summary>
    /// ���������� ��� �������� ���������� ������� ���������
    /// </summary>
    private Vector2 previousPointerPosition;
    /// <summary>
    /// ����� ���������� �� ����� �������� ���������� ��������
    /// </summary>
    void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
    }
    /// <summary>
    /// ������� ������������ ��� ����������� ������� � ��������� ����������
    /// </summary>
    /// <param name="data">"������� � ��������� ����������</param>
    public void OnPointerDown(PointerEventData data)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out previousPointerPosition);
    }
    /// <summary>
    /// ��� �������������� ��� ������� ����� ���������� ��� ������ ����������� �������
    /// </summary>
    /// <param name="data">������� � ����������</param>
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
