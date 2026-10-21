using UnityEngine;
using System.Collections;


public class DetectorEstaca : MonoBehaviour
{
    public float velocidadeQueda = 1.5f;
    public float atrasoMinimo = 0.3f;
    public float atrasoMaximo = 2f;


    private bool ativada = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (ativada)
            return;


        if (other.CompareTag("Player"))
        {
            ativada = true;
            StartCoroutine(IniciarQueda());
        }
    }


    private IEnumerator IniciarQueda()
    {
        float atraso = Random.Range(atrasoMinimo, atrasoMaximo);
        yield return new WaitForSeconds(atraso);


        Rigidbody2D rb =
            GetComponentInParent<Rigidbody2D>();


        if (rb != null)
        {
            rb.gravityScale = velocidadeQueda;
        }
    }
}