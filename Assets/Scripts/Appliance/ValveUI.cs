using UnityEngine;
using UnityEngine.UI;

public class ValveUI : MonoBehaviour
{
    [SerializeField] private Image timer;
    [SerializeField] private Valve valve;
    void LateUpdate()
    {
        //Debug.Log("What VavleUI sees " + valve.unscrewProgress);
        transform.LookAt(Camera.main.transform);
        timer.fillAmount = valve.unscrewProgress;
    }
}
