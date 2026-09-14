using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private bool isDead = false;

    [Header("Boshqaruvchilar")]
    // MenuManager skriptini inspector'da shu yerga bering
    [SerializeField] private MenuManager menuManager;

    void Start()
    {
        currentHealth = maxHealth;

        // Agar Inspector'da biriktirilmagan bo'lsa, avtomatik topish
        if (menuManager == null)
        {
            menuManager = FindFirstObjectByType<MenuManager>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("Player Dead");

        // MenuManager orqali YouDie panelini ochamiz va cursor/vaqtni boshqaramiz
        if (menuManager != null)
        {
            menuManager.ShowYouDie();
        }
        else
        {
            Debug.LogError("MenuManager topilmadi! PlayerHealth skriptiga MenuManager ulanmagan.");
        }
    }
}