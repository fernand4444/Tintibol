using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    private bool ativado = false;


    [Header("Cores do Checkpoint")]
    public SpriteRenderer imagem;
    public Color corInicial = Color.white;
    public Color corAtivada = new Color(1f, 0.4f, 0.7f, 1f);


    private void Start()
    {
        if (imagem == null)
        {
            imagem = GetComponent<SpriteRenderer>();
        }


        if (imagem != null)
        {
            imagem.color = corInicial;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ativado)
            return;


        if (other.CompareTag("Player"))
        {
            CheckpointManager.SalvarCheckpoint(transform.position);


            ativado = true;


            if (imagem != null)
            {
                imagem.color = corAtivada;
            }


            Debug.Log("Checkpoint ativado!");
        }
    }
}