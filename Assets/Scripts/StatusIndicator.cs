using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {
    [SerializeField]
    private RectTransform healthBarRect;
    [SerializeField]
    private Text healthText;
    private void Start()
    {
        if(healthBarRect == null)
        {
            Debug.LogError("STATUS INDICATOR : No HealthBar reference!!");
        }
        if (healthText == null)
        {
            Debug.LogError("STATUS INDICATOR : No HealthText reference!!");
        }
    }
    public void SetHealth(int _cur, int _max)       //_argumentName denotes variables that are constraint to local environment only.
    {
        float _value =(float) _cur / _max;
        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = _cur + "/" + _max + " HP";

    }
    
}
