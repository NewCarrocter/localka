using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс для обработки IP-адресов с конечной точкой для сервера и клиентов введённые пользователем 
/// </summary>
public class IpInput : MonoBehaviour
{
    /// <summary>
    /// Переменная для хранения IP-адреса с конечной точкой после проверки
    /// </summary>
    internal IPEndPoint ip;
    /// <summary>
    /// Функция которая выполняется после того как пользователь ввёл IP-адрес с конечной точкой
    /// </summary>
    /// <param name="input">IP-адрес с конечной точкой в виде строки</param>
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
            GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Введён не IP адрес!";
        }
    }
    /// <summary>
    /// Функция для проверки на корректность ввода IP-адреса с конечной точкой
    /// </summary>
    /// <param name="input">IP-адрес с конечной точкой в виде строки который нужно проверить на корректность ввода</param>
    /// <param name="result">Переменная в которую будет записанно значение если пользователь ввёл IP-адреса с конечной точкой корректно</param>
    /// <returns>Истино если пользователь ввёл IP-адреса с конечной точкой корректно, ложно если пользователь ввёл IP-адреса с конечной точкой не корректно</returns>
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
    /// Функция для проверки IP-адреса с конечной точкой на корректное значение для клиента
    /// </summary>
    private void ForClient()
    {
        if (ip == NetworkClass.self)
            return;
        if (transform.parent.parent.childCount - 1 == transform.parent.GetSiblingIndex())
            Instantiate(Resources.Load<GameObject>("Prefabs/clientPrefab"), transform.parent.parent);
    }
}