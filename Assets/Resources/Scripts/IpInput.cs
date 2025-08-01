using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����� ��� ��������� IP-������� � �������� ������ ��� ������� � �������� �������� ������������� 
/// </summary>
public class IpInput : MonoBehaviour
{
    /// <summary>
    /// ���������� ��� �������� IP-������ � �������� ������ ����� ��������
    /// </summary>
    internal IPEndPoint ip;
    /// <summary>
    /// ������� ������� ����������� ����� ���� ��� ������������ ��� IP-����� � �������� ������
    /// </summary>
    /// <param name="input">IP-����� � �������� ������ � ���� ������</param>
    [SerializeField]
    private void CheckData(string input)
    {
        if (CheckForValidIPEndPoint(input, out ip))
        {
            ForClient();
        }
        else
        {
            GetComponent<InputField>().text = "";
            GetComponent<InputField>().placeholder.color = Color.red;
            GetComponent<InputField>().placeholder.GetComponent<Text>().text = "����� �� IP �����!";
        }
    }
    /// <summary>
    /// ������� ��� �������� �� ������������ ����� IP-������ � �������� ������
    /// </summary>
    /// <param name="input">IP-����� � �������� ������ � ���� ������ ������� ����� ��������� �� ������������ �����</param>
    /// <param name="result">���������� � ������� ����� ��������� �������� ���� ������������ ��� IP-������ � �������� ������ ���������</param>
    /// <returns>������ ���� ������������ ��� IP-������ � �������� ������ ���������, ����� ���� ������������ ��� IP-������ � �������� ������ �� ���������</returns>
    private bool CheckForValidIPEndPoint(string input, out IPEndPoint result)
    {
        string[] a = input.Split(':');
        if (a.Length != 2)
        {
            result = null;
            return false;
        }
        if (!IPAddress.TryParse(a[0], out IPAddress ip))
        {
            result = null;
            return false;
        }
        if (!int.TryParse(a[1], out int port))
        {
            result = null;
            return false;
        }
        else if (port < 0 || port > 65535)
        {
            result = null;
            return false;
        }
        result = new IPEndPoint(ip, port);
        return true;
    }
    /// <summary>
    /// ������� ��� �������� IP-������ � �������� ������ �� ���������� �������� ��� �������
    /// </summary>
    private void ForClient()
    {
        if (ip == NetworkClass.self)
            return;
        if (transform.parent.parent.childCount - 1 == transform.parent.GetSiblingIndex())
            Instantiate(Resources.Load<GameObject>("Prefabs/clientPrefab"), transform.parent.parent);
    }
}