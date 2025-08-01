using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����� ��� �������� �������, ������� ������������ ��� ������ ��������
/// </summary>
public class GetCheckboxes : MonoBehaviour
{
    /// <summary>
    /// ���������� ��� �������� �������, � ������� ����� ������� ����������
    /// </summary>
    [SerializeField]
    protected private Transform box;
    /// <summary>
    /// ������� ������� ������ ������, ��� ������ ��������
    /// </summary>
    void OnEnable()
    {
        box.DetachChildren();
        GameObject item = Resources.Load<GameObject>("Prefabs/checkbox");
        foreach (string ep in NetworkClass.clients.AsParallel().Select(ep => ep.Address.ToString() + ":" + ep.Port.ToString()))
        {
            item.GetComponentInChildren<Text>().text = ep;
            Instantiate(item, box);
        }
    }
}