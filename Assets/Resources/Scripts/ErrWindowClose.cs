using UnityEngine;
/// <summary>
/// ����� ��� ����������� ���� ������
/// </summary>
public class ErrWindowClose : MonoBehaviour
{
    /// <summary>
    /// ������� ��� ����������� ���� ������
    /// </summary>
    [SerializeField]
    private void KillWindow() => Destroy(transform.parent.gameObject);
}