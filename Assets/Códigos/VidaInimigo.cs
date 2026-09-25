using UnityEngine;

public class VidaInimigo : MonoBehaviour
{
    public int vida = 30;

    public void TomarDano(int dano)
    {
        vida -= dano;

        if (vida <= 0)
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Destroy(gameObject);
    }
}