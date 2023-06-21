using TMPro;
using UnityEngine;
public class SliderFieldUpdater : MonoBehaviour{
    
    // Our instance of TextMeshProUGUI, representing a UI label
    public TextMeshProUGUI TextMeshProInstance;
    
    // If TextMeshProInstance isn't set, get the local instance
    private void Reset(){
        if (TextMeshProInstance == null) TextMeshProInstance = GetComponent<TextMeshProUGUI>();
    }
    
    // This method simple assigns the passed in float newValue to the text field.
    // We set the slider in the inspector so that we call this method whenever the slider is changed.
    public void OnValueChanged(float newValue){
        TextMeshProInstance.text = newValue.ToString();
    }
}