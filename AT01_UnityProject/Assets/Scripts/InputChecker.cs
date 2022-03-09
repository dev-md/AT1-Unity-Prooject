using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour
{
    #region
    private static InputChecker _instance;
    private static InputChecker Instance;
    #endregion

    //
    public delegate string InputConfrim(string dirInput);
    public static event InputConfrim ConfrimDirInput;

    public List<KeyCode> upKeys;
    public List<KeyCode> downKeys;
    public List<KeyCode> leftKeys;
    public List<KeyCode> rightKeys;
    Dictionary<string, List<KeyCode>> listInputs = new Dictionary<string, List<KeyCode>>();

    public Button uButton;
    public Button dButton;
    public Button lButton;
    public Button rButton;

    private void Awake()
    {
        listInputs.Add("u", upKeys);
        listInputs.Add("d", downKeys);
        listInputs.Add("l", leftKeys);
        listInputs.Add("r", rightKeys);

        uButton.GetComponent<Button>().onClick.AddListener(UpButtonClick);
        dButton.GetComponent<Button>().onClick.AddListener(DownButtonClick);
        lButton.GetComponent<Button>().onClick.AddListener(LeftButtonClick);
        rButton.GetComponent<Button>().onClick.AddListener(RightButtonClick);

    }

    private void Update()
    {
        foreach(KeyValuePair<string, List<KeyCode>> x in listInputs)
        {
            foreach(KeyCode y in x.Value)
            {
                if (Input.GetKeyDown(y))
                {
                    if (ConfrimDirInput != null) ConfrimDirInput.Invoke(x.Key);
                }
            }
        }
    }

    void UpButtonClick()
    {
        if (ConfrimDirInput != null) ConfrimDirInput.Invoke("u");
    }
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
