using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class UIAnimHandler : MonoBehaviour
{
    [SerializeField] private Sprite arrowRight;
    [SerializeField] private Sprite arrowLeft;
    [SerializeField] private Animator anim;
    [SerializeField] private Image arrowTarget;
    [SerializeField] private Button targetButton;
    [SerializeField] private float lockDurationSeconds = 0.42f;

    private bool isLocked;

    public void OnClickButton()
    {
        if (isLocked) { return; }
        if (arrowRight == null || arrowLeft == null || anim == null || arrowTarget == null) { return; }

        if (arrowRight == arrowTarget.sprite)
        {
            anim.SetTrigger("Enter");
            arrowTarget.sprite = arrowLeft;
        }
        else
        {
            anim.SetTrigger("Exit");
            arrowTarget.sprite = arrowRight;
        }

        StartCoroutine(LockButtonCoroutine());
    }

    private IEnumerator LockButtonCoroutine()
    {
        isLocked = true;
        if (targetButton != null)
        {
            targetButton.interactable = false;
        }

        yield return new WaitForSeconds(lockDurationSeconds);

        if (targetButton != null)
        {
            targetButton.interactable = true;
        }
        isLocked = false;
    }
    public void CloseApp()
    {
        Application.Quit();
    }
}
