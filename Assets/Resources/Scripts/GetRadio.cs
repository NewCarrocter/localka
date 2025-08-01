using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class GetRadio : MonoBehaviour
{
    /// <summary>
    /// Переменная для хранения объекта, в котором нужно создать переменные
    /// </summary>
    [SerializeField]
    protected private Transform box;
    /// <summary>
    /// Функция которая создаёт флажки, для выбора клиентов
    /// </summary>
    void OnEnable()
    {
        box.DetachChildren();
        GameObject item = Resources.Load<GameObject>("Prefabs/checkbox");
        foreach (string ep in NetworkClass.clients.AsParallel().Select(ep => ep.Address.ToString() + ":" + ep.Port.ToString()))
        {
            item.GetComponentInChildren<Text>().text = ep;
            item.GetComponent<Toggle>().onValueChanged.AddListener(DeselectOther);
            Instantiate(item, box);
        }
    }
    private void DeselectOther(bool state)
    {
        foreach (Transform c in transform.parent)
        {
            c.GetComponent<Toggle>().isOn = false;
        }
        transform.GetComponent<Toggle>().isOn = state;
    }
}
