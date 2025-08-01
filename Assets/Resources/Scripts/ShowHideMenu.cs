using UnityEngine;
using UnityEngine.EventSystems;
/// <summary>
/// ����� ��� ��������� ������� � �������� �� ������� ����
/// </summary>
public class ShowHideMenu: MonoBehaviour, ISelectHandler, IDeselectHandler
{
    /// <summary>
    /// ������ � ������� �������� ����� ����
    /// </summary>
    [SerializeField]
    private GameObject target;
    /// <summary>
    /// ������� ������������� �� ���������� ISelectHandler ������������ ��� ������ ������ ����
    /// </summary>
    /// <param name="eventData">������ ���������� ��� ���������� �������</param>
    public void OnSelect(BaseEventData eventData) => target.SetActive(true);
    /// <summary>
    /// ������� ������������� �� ���������� IDeselectHandler ������������ ��� ������ ������ ����
    /// </summary>
    /// <param name="eventData">������ ���������� ��� ���������� �������</param>
    public void OnDeselect(BaseEventData eventData) => target.SetActive(false);
}