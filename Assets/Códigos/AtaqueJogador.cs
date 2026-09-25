using UnityEngine;

public class AtaqueJogador : MonoBehaviour
{
    public int dano = 10;

    private bool atacando = false;

    public void ComecarAtaque()
    {
        atacando = true;
    }

    public void TerminarAtaque()
    {
        atacando = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!atacando)
            return;

        VidaInimigo inimigo = other.GetComponent<VidaInimigo>();

        if (inimigo != null)
        {
            inimigo.TomarDano(dano);
        }
    }
}
