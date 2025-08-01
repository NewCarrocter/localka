using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����� ��� ��������� ������� ����� ������ ������ � ������ ����
/// </summary>
public class SelectOptionMenuItem : MonoBehaviour
{
    /// <summary>
    /// ���� ������� ����� ��������
    /// </summary>
    [SerializeField]
    private GameObject target;
    /// <summary>
    /// ������� ����������� ����� ������� �� ����� � ������ ����
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