using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ����� ��� ������������ ��������� �������� �������� ���������� ������
/// </summary>
public class checkboxBtn : MonoBehaviour
{
    /// <summary>
    /// ������ ��� �������� ������� ������������ ��������� ������
    /// </summary>
    public Sprite[] sprites;
    /// <summary>
    /// ������� ��� ��������� ��������� ������
    /// </summary>
    public void OnAction()
    {
        if (GetComponentInChildren<Image>().sprite == sprites[0])
            GetComponentInChildren<Image>().sprite = sprites[1];
        else
            GetComponentInChildren<Image>().sprite = sprites[0];
    }
}