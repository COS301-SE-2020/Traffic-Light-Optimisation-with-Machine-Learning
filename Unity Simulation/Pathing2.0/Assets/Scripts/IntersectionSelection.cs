/**
	@file IntersectionSelection.cs
*/

using UnityEngine;
using System.Collections;
using TMPro;

/**
	This class is to implement the feature of hovering statistics
*/
public class IntersectionSelection : MonoBehaviour
{
    public TextMeshProUGUI UI_intersection;
    public TextMeshProUGUI UI_intersectionData;

    public GameObject IntersectionDataPanel;

    //public GameObject Intersection;
    private Color color;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        IntersectionDataPanel.SetActive(false);
    }

    private void OnMouseEnter() {
        UI_intersection.text = this.name;
        color = GetComponent<Renderer>().material.color;
        color.a = .50f;
        GetComponent<Renderer>().material.color = color;

        IntersectionDataPanel.SetActive(true);

        StartCoroutine(CalcIntersectionData());
    }

    /// <summary>
    /// Called when the mouse is not any longer over the GUIElement or Collider.
    /// </summary>
    void OnMouseExit()
    {
        UI_intersection.text = "";
        color = GetComponent<Renderer>().material.color;
        color.a = .05f;
        GetComponent<Renderer>().material.color = color;

        IntersectionDataPanel.SetActive(false);
    }

    private IEnumerator CalcIntersectionData() {
        while(true){
            TrafficIntersection IntersectionData = GetComponentInParent<IntersectionParent>().getIntersection(); //hard coded atm
        
            float totalCars = IntersectionData.movingX + IntersectionData.movingY + IntersectionData.stationaryX + IntersectionData.stationaryY;
            float stationaryCars = IntersectionData.stationaryX + IntersectionData.stationaryY;
            float movingCars = IntersectionData.movingX + IntersectionData.movingY;

            UI_intersectionData.text = "Number of cars: " + totalCars.ToString() +"\n"
                                        + "stationary cars: " + stationaryCars.ToString() + "\n"
                                        + "moving cars: " + movingCars.ToString() + "\n";

            yield return new WaitForSeconds(0.1f);
        }
    }
}
