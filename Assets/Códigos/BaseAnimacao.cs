using UnityEngine;


public class BaseAnimacao : MonoBehaviour
{
    public Animator animator;


    void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }


    public void Atacar()
    {
        if (animator != null)
        {
            animator.SetBool("vilaoatacando", true);
        }
    }


    public void PararAtaque()
    {
        if (animator != null)
        {
            animator.SetBool("vilaoatacando", false);
        }
    }
}