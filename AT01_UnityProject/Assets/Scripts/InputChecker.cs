using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        listInputs.Add("u", upKeys);
        listInputs.Add("d", downKeys);
        listInputs.Add("l", leftKeys);
        listInputs.Add("r", rightKeys);
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
}
