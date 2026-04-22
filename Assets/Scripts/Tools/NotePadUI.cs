using UnityEngine;
using UnityEngine.UI;

public class NotePadUI : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private Notepad notepad;
    public void UpdateUI()
    {
        //Debug.Log("What VavleUI sees " + valve.unscrewProgress);
        transform.LookAt(Camera.main.transform);
        timer.fillAmount = Mathf.Clamp01(notepad.currentCustomer.progress / notepad.maxProgress);
    }
    public void ResetUI()
    {
        timer.fillAmount = 0;
    }
}
