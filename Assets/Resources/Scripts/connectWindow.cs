using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс который взаимодействует с окном для ввода информации для создания соединений с устройствами
/// </summary>
public class ConnectWindow : MonoBehaviour
{
    /// <summary>
    /// Массив для хранения элементов управления которые находятся в окне для создания соединений с устройствами
    /// </summary>
    [SerializeField]
    private GameObject[] radio;
    /// <summary>
    /// Массив картинок значений пререключателей которые находятся в окне для создания соединений с устройствами
    /// </summary>
    [SerializeField]
    private Sprite[] sprites;
    /// <summary>
    /// Функция для смены картинок пререключателей и отключения текстовых полей, для ввода IP-адресса с конечной точкой, клиента
    /// </summary>
    [SerializeField]
    private void ClientRadioBtn()
    {
        radio[2].GetComponent<Image>().sprite = sprites[1];
        radio[3].GetComponent<Image>().sprite = sprites[0];
        foreach (Transform a in radio[5].transform)
            a.GetComponentInChildren<InputField>().interactable = false;
    }
    /// <summary>
    /// Функция для смены картинок пререключателей и включения текстовых полей, для ввода IP-адресса с конечной точкой, клиента
    /// </summary>
    [SerializeField]
    private void ClientRadioBtn1()
    {
        radio[2].GetComponent<Image>().sprite = sprites[0];
        radio[3].GetComponent<Image>().sprite = sprites[1];
        foreach (Transform a in radio[5].transform)
            a.GetComponentInChildren<InputField>().interactable = true;
    }
    /// <summary>
    /// Функция для установки соединения между клиентами и сервером
    /// </summary>
    [SerializeField]
    private void Connect()
    {
        if (radio[5].GetComponentsInChildren<InputField>()[0].interactable)
        {
            List<IPEndPoint> a = radio[5].GetComponentsInChildren<IpInput>().AsParallel().Select(scr => scr.ip).ToList();
            a.RemoveRange(a.Count - 1,1);
            if (a.Any(ip => ip == null))
                a.RemoveAll(ip => ip == null);
            NetworkClass.clients = a.ToArray();
        }
        else
            NetworkClass.clients = NetworkClass.SetClients();
        //NetworkClass.clients.AsParallel().ForAll(el => Debug.Log(el.ToString()));
        NetworkClass.SendCommand(NetworkClass.Command.startSend, NetworkClass.clients);
    }
}
