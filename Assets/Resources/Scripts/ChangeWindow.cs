using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWindow : GetCheckboxes
{
    public Dropdown dropdown;
    [SerializeField]
    private void ChangePc()
    {
        IPEndPoint[] points = GetEndPoints();
        if (points is null)
            throw new Exception("Не выбраны клиенты!");
        else
            NetworkClass.SendCommand(NetworkClass.Command.changeState, points, dropdown.value.ToString());
    }
    /// <summary>
    /// Функция для получения IP-адресов с конечной точкой, которые выбрал пользователь
    /// </summary>
    /// <returns>Массив IP-адресов с конечной точкой</returns>
    private IPEndPoint[] GetEndPoints()
    {
        List<string> ipInText = new();
        foreach (Transform child in box)
            if (child.GetComponent<Toggle>().isOn)
                ipInText.Add(child.GetComponentInChildren<Text>().text);
        return ipInText.AsParallel().Select(str => str.Split(':')).Select(str => new IPEndPoint(IPAddress.Parse(str[0]), int.Parse(str[1]))).ToArray();
    }
}