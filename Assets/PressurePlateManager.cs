using UnityEngine;


enum PressurePlateColor
{
    Red, Green, Blue, Yellow, None
}

public class PressurePlateManager : MonoBehaviour
{
    [SerializeField] private GameObject leverReward;

    [SerializeField] private Material neutralState;
    [SerializeField] private Material correctState;
    [SerializeField] private Material wrongState;

    [SerializeField] private MeshRenderer[] circlesFeedBack;


    [SerializeField] private PressurePlate[] pressurePlates;

    private PressurePlateColor[] pressurePlateCorrectOrder = new PressurePlateColor[4]{PressurePlateColor.Blue,PressurePlateColor.Yellow,PressurePlateColor.Green,PressurePlateColor.Red};

    private PressurePlateColor[] pressedPlateOrder = new PressurePlateColor[4];

    int activePlateCounter = 0;

    private void Start()
    {
        leverReward.SetActive(false);
    }

    private void UpdateOrderFeedback()
    {        
        if (pressedPlateOrder[activePlateCounter] == pressurePlateCorrectOrder[activePlateCounter])
        {
            circlesFeedBack[activePlateCounter].material = correctState;
            Debug.Log("Correct press");

        }
        else
        {
            circlesFeedBack[activePlateCounter].material = wrongState;
            Debug.Log("Wrong press");

        }

        activePlateCounter++;

        if (activePlateCounter >= 4)
        {
            if (CompareOrder())
            {
                //Drop lever
                leverReward.SetActive(true);
            }
            else
            {                
                ResetAll();
            }

        }
    }

    /// <summary>
    /// compare the pressed order and the correct order
    /// </summary>
    /// <returns>
    /// True = order is correct, player won the mini game
    /// False = order isn't correct, player hasn't found the correct order. Reset game
    /// </returns>
    private bool CompareOrder()
    {
        bool isCorrect = true;

        for (int i = 0; i < pressurePlateCorrectOrder.Length; i++)
        { 
            if (pressedPlateOrder[i] != pressurePlateCorrectOrder[i])
            {
                isCorrect = false; 
                break;
            }
        }

        return isCorrect;
    }

    private void ResetAll()
    {
        activePlateCounter = 0;      

        foreach (MeshRenderer circleFB in circlesFeedBack)
        { 
            circleFB.material = neutralState;
        }
        pressedPlateOrder = new PressurePlateColor[4];

        foreach (PressurePlate plate in pressurePlates)
        {
            plate.state = false;
        }        
    }

    public void RedActivate()
    { 
        pressedPlateOrder[activePlateCounter] = PressurePlateColor.Red;
        UpdateOrderFeedback();
    }

    public void BlueActivate()
    {
        pressedPlateOrder[activePlateCounter] = PressurePlateColor.Blue; 
        UpdateOrderFeedback();
    }

    public void GreenActivate()
    {
        pressedPlateOrder[activePlateCounter] = PressurePlateColor.Green;
        UpdateOrderFeedback();
    }

    public void YellowActivate()
    {
        pressedPlateOrder[activePlateCounter] = PressurePlateColor.Yellow;
        UpdateOrderFeedback();
    }
}
