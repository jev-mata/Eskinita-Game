using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image[] hearts;             // Assign your 3 heart image slots in Inspector
    public Sprite fullHeart;           // Full heart sprite
    public Sprite emptyHeart;          // Empty heart sprite

    public void UpdateHearts(int currentLife)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentLife)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
                hearts[i].enabled = true;
            }
        }
    }
}
