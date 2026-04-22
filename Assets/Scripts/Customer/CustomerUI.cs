using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private Customer customer;
    public void UpdateUI()
    {
        //Debug.Log("What VavleUI sees " + valve.unscrewProgress);
        transform.LookAt(Camera.main.transform);
        timer.fillAmount = Mathf.Clamp01(customer.getCurrentPaitence() / customer.maxPaitence);
    }
    public void ResetUI()
    {
        timer.fillAmount = 0;
    }
}

