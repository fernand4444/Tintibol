
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalizarCutscene : MonoBehaviour
{
    public Animator animator;

    private bool jaTerminou = false;

    void Update()
    {
        if (jaTerminou) return;

        if (animator == null) return;

        AnimatorStateInfo estado =
            animator.GetCurrentAnimatorStateInfo(0);

        if (estado.IsName("custecene") &&
            estado.normalizedTime >= 1f)
        {
            jaTerminou = true;

            SceneManager.LoadScene("jogo");
        }
    }
}
