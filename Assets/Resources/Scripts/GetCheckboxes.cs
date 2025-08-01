using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Класс для создания флажков, которые используются для выбора клиентов
/// </summary>
public class GetCheckboxes : MonoBehaviour
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
            Instantiate(item, box);
        }
    }
}