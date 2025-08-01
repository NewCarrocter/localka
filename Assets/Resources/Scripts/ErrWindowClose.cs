using UnityEngine;
/// <summary>
/// Класс для уничтожения окна ошибки
/// </summary>
public class ErrWindowClose : MonoBehaviour
{
    /// <summary>
    /// Функция для уничтожения окна ошибки
    /// </summary>
    [SerializeField]
    private void KillWindow() => Destroy(transform.parent.gameObject);
}