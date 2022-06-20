using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour
{
    #region
    private static InputChecker _instance; // Getting the instance of the input checker
    private static InputChecker Instance;
    public static InputChecker inputInstance { get; private set; }
    #endregion

    //
    public delegate string InputConfrim(string dirInput); //Input event along with a string
    public static event InputConfrim ConfrimDirInput; // confriming event

    #region
    [SerializeField] private Button uButton; //The UI Buttons that will be used
    [SerializeField] private Button dButton;
    [SerializeField] private Button lButton;
    [SerializeField] private Button rButton;

    public Button publicUButton { get; private set; }
    public Button publicDButton { get; private set; }
    public Button publicLButton { get; private set; }
    public Button publicRButton { get; private set; }
    #endregion

    private float dirupdown;
    private float dirrightleft;

    private void Awake()
    {
        inputInstance = this;

        #region
        publicUButton = uButton;
        publicDButton = dButton;
        publicLButton = lButton;
        publicRButton = rButton;


        // A Super lazy way of checking to see if the Buttons are being pressed
        uButton.GetComponent<Button>().onClick.AddListener(UpButtonClick);
        dButton.GetComponent<Button>().onClick.AddListener(DownButtonClick);
        lButton.GetComponent<Button>().onClick.AddListener(LeftButtonClick);
        rButton.GetComponent<Button>().onClick.AddListener(RightButtonClick);
        #endregion

    }

    private void Update()
    {
        dirupdown = Input.GetAxis("Vertical");
        dirrightleft = Input.GetAxis("Horizontal");

        if (ConfrimDirInput != null)
        {
            //ConfrimDirInput.Invoke(Key);
            if (dirupdown > 0)
            {
                ConfrimDirInput.Invoke("u");
            }
            else if (dirupdown < 0)
            {
                ConfrimDirInput.Invoke("d");
            }
            else if (dirrightleft < 0)
            {
                ConfrimDirInput.Invoke("l");
            }
            else if (dirrightleft > 0)
            {
                ConfrimDirInput.Invoke("r");
            }
        }
        
    }

    //A super duper lazy way of creating a direction method,
    void UpButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("u");
        // Calls Event with the direction that is stated.
    }

    //This is the same for the rest of the direction.
    void DownButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("d");
    }
    void LeftButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("l");
    }
    void RightButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("r");
    }

}
