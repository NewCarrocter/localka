using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Сласс для проверки и добавления строк для ввода IP-адресов с конечной точкой пользователем требующиеся для установки соединения
/// </summary>
public class ClientPrefab : MonoBehaviour
{
    /// <summary>
    /// Функция для проверки строки на корректность IP-адресов с конечной точкой для правельного подключения
    /// </summary>
    /// <param name="input">Строка для проверки</param>
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
            GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Введён не IP адрес!";
        }
    }
    /// <summary>
    /// Функция для переобразования строки в тип IPEndPoint, после преобразования возвращает значение истино или ложно взависимости от результата преобразования
    /// </summary>
    /// <param name="input">Строка для преобразования</param>
    /// <param name="result">Переменная IPEndPoint в которую нужно записать IP-адрес к конечной точкой</param>
    /// <returns>Значение истино или ложно взависимости от результата преобразования</returns>
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
    /// Функция для проверки IP-адреса к конечной точкой предназначенного для получения информации с других устройств
    /// </summary>
    /// <param name="ip">IP-адрес к конечной точкой предназначенный для получения информации с других устройств</param>
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
                GetComponent<InputField>().placeholder.GetComponent<Text>().text = "Для этого IP адреса маршрутизация запрещена!";
                return;
        }
    }
    /// <summary>
    /// Функция для создания новой строки ввода в которой вводится IP-адрес к конечной точкой предназначенный для отправки информации на сервер
    /// </summary>
    /// <param name="ip">IP-адрес к конечной точкой предназначенный для отправки информации на сервер</param>
    private void ForClient(IPEndPoint ip)
    {
        Instantiate(Resources.Load<GameObject>("Prefabs/clientPrefab"), transform.parent.parent);
        Destroy(GetComponent<ClientPrefab>());
    }
}