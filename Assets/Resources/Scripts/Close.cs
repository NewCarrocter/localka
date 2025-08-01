using UnityEngine;
/// <summary>
/// ����� ��� �������� ��� �������� ���� � ��������� ���� ����������
/// </summary>
public class Close : MonoBehaviour
{
    /// <summary>
    /// �������� ��� �������� ������� �������� ����� ������� ��� �������
    /// </summary>
    public GameObject target;
    /// <summary>
    /// ������� ��� �������� ��� �������� ������� ������� ��������� ���������� target
    /// </summary>
    public void DoAction() => target.SetActive(!target.activeSelf);
}