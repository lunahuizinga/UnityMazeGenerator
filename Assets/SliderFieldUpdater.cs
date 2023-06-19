using TMPro;
using UnityEngine;
public class SliderFieldUpdater : MonoBehaviour{
    
    public TextMeshProUGUI TextMeshProInstance;

    private void Reset(){
        if (TextMeshProInstance == null) TextMeshProInstance = GetComponent<TextMeshProUGUI>();
    }

    public void OnValueChanged(float newValue){
        TextMeshProInstance.text = newValue.ToString();
    }
}