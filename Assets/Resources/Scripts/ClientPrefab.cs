using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����� ��� �������� � ���������� ����� ��� ����� IP-������� � �������� ������ ������������� ����������� ��� ��������� ����������
/// </summary>
public class ClientPrefab : MonoBehaviour
{
    /// <summary>
    /// ������� ��� �������� ������ �� ������������ IP-������� � �������� ������ ��� ����������� �����������
    /// </summary>
    /// <param name="input">������ ��� ��������</param>
    public void CheckData(string input)
    {
        IPEndPoint ip;
        if (checkForValidIPEndPoint(input, out ip))
        {
            if (name == "server")
                ForServer(ip);
            else
                ForClient(ip);
        }
        else
        {
            GetComponent<InputField>().text = "";
            GetComponent<InputField>().placeholder.color = Color.red;
            GetComponent<InputField>().placeholder.GetComponent<Text>().text = "����� �� IP �����!";
        }
    }
    /// <summary>
    /// ������� ��� ��������������� ������ � ��� IPEndPoint, ����� �������������� ���������� �������� ������ ��� ����� ������������ �� ���������� ��������������
    /// </summary>
    /// <param name="input">������ ��� ��������������</param>
    /// <param name="result">���������� IPEndPoint � ������� ����� �������� IP-����� � �������� ������</param>
    /// <returns>�������� ������ ��� ����� ������������ �� ���������� ��������������</returns>
    private bool checkForValidIPEndPoint(string input, out IPEndPoint result)
    {
        string[] a = input.Split(':');
        IPAddress ip;
        if (!IPAddress.TryParse(a[0], out ip))
        {
            result = null;
            return false;
        }
        int port;
        if (!int.TryParse(a[1], out port))
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
    /// ������� ��� �������� IP-������ � �������� ������ ���������������� ��� ��������� ���������� � ������ ���������
    /// </summary>
    /// <param name="ip">IP-����� � �������� ������ ��������������� ��� ��������� ���������� � ������ ���������</param>
    private void ForServer(IPEndPoint ip)
    {
        Debug.Log(ip.Address.ToString().Split('.')[0]);
        switch (ip.Address.ToString().Split('.')[0])
        {
            case "0":
            case "127":
            case "255":
                GetComponent<InputField>().text = "";
                GetComponent<InputField>().placeholder.color = Color.red;
                GetComponent<InputField>().placeholder.GetComponent<Text>().text = "��� ����� IP ������ ������������� ���������!";
                return;
        }
    }
    /// <summary>
    /// ������� ��� �������� ����� ������ ����� � ������� �������� IP-����� � �������� ������ ��������������� ��� �������� ���������� �� ������
    /// </summary>
    /// <param name="ip">IP-����� � �������� ������ ��������������� ��� �������� ���������� �� ������</param>
    private void ForClient(IPEndPoint ip)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/clientPrefab"), transform.parent.parent);
        Destroy(GetComponent<ClientPrefab>());
    }
}