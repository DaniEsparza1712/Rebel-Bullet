using UnityEngine;
using UnityEngine.UI;

public class Update : MonoBehaviour
{
    [SerializeField] public Slider statSlider;
    public Color green = new Color(37, 130, 7);
    public Color blue = new Color(40, 44, 55);
    public Color red = new Color(130, 7, 7);

    private float currentStatValue;

    void OnEnable()
    {
        currentStatValue = statSlider.value;
        UpdateUI(2);
    }

    public void UpdateUI(float newValue)
    {
        statSlider.value = newValue;

        if (currentStatValue < newValue)
        {
            statSlider.fillRect.GetComponent<Image>().color = green;
        }
        else if (currentStatValue > newValue)
        {
            statSlider.fillRect.GetComponent<Image>().color = red;
        }
        else
        {
            statSlider.fillRect.GetComponent<Image>().color = blue;
        }

        currentStatValue = statSlider.value;
    }
}
