using System;
using System.IO;
using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

#region Input Action Enum
[Serializable]
public enum InputAction
{
    MoveForward = 0,
    MoveBackward = 1,
    MoveLeft = 2,
    MoveRight = 3,
    Run = 4,
    LookBehind = 5,
    PauseOrCancel = 6,
    Confirm = 7,
    Interact = 8,
    UseItem = 9,
    Jump = 10,
    Slot0 = 11,
    Slot1 = 12,
    Slot2 = 13,
    Slot3 = 14,
    Slot4 = 15,
    Slot5 = 16,
    Slot6 = 17,
    Slot7 = 18,
    Slot8 = 19
}
#endregion

public class InputManager : Singleton<InputManager>
{
    #region Input Mapping Data
    public Dictionary<InputAction, KeyCode> KeyboardMapping = new Dictionary<InputAction, KeyCode>()
    {
        { InputAction.MoveForward, KeyCode.W },
        { InputAction.MoveBackward, KeyCode.S },
        { InputAction.MoveLeft, KeyCode.A },
        { InputAction.MoveRight, KeyCode.D },
        { InputAction.Run, KeyCode.LeftShift },
        { InputAction.LookBehind, KeyCode.Space },
        { InputAction.PauseOrCancel, KeyCode.Escape },
        { InputAction.Confirm, KeyCode.Return },
        { InputAction.Interact, KeyCode.E },
        { InputAction.UseItem, KeyCode.Q },
        { InputAction.Jump, KeyCode.Mouse0 },
        { InputAction.Slot0, KeyCode.Alpha1 },
        { InputAction.Slot1, KeyCode.Alpha2 },
        { InputAction.Slot2, KeyCode.Alpha3 },
        { InputAction.Slot3, KeyCode.Alpha4 },
        { InputAction.Slot4, KeyCode.Alpha5 },
        { InputAction.Slot5, KeyCode.Alpha6 },
        { InputAction.Slot6, KeyCode.Alpha7 },
        { InputAction.Slot7, KeyCode.Alpha8 },
        { InputAction.Slot8, KeyCode.Alpha9 }
    };

    private Dictionary<InputAction, bool> keyStates = new Dictionary<InputAction, bool>();
    #endregion

    #region Input Queries
    public bool GetActionKey(InputAction action)
    {
        if (KeyboardMapping.ContainsKey(action))
        {
            bool isKeyPressed = Input.GetKey(KeyboardMapping[action]) || (KeyboardMapping[action] == KeyCode.Mouse0 && Input.GetMouseButton(0)) || (KeyboardMapping[action] == KeyCode.Mouse1 && Input.GetMouseButton(1));

            if (action == InputAction.Run || action == InputAction.LookBehind ||
                action == InputAction.MoveForward || action == InputAction.MoveBackward ||
                action == InputAction.MoveRight || action == InputAction.MoveLeft ||
                action == InputAction.Interact)
            {
                return isKeyPressed;
            }

            if (isKeyPressed && !keyStates.ContainsKey(action))
            {
                keyStates[action] = true;
                return true;
            }

            if (!isKeyPressed && keyStates.ContainsKey(action))
            {
                keyStates.Remove(action);
            }

            return false;
        }

        return false;
    }

    public bool GetActionKeyDown(InputAction action)
    {
        if (KeyboardMapping.ContainsKey(action))
        {
            bool isKeyPressed = Input.GetKeyDown(KeyboardMapping[action]) || (KeyboardMapping[action] == KeyCode.Mouse0 && Input.GetMouseButtonDown(0)) || (KeyboardMapping[action] == KeyCode.Mouse1 && Input.GetMouseButtonDown(1));

            if (isKeyPressed)
            {
                keyStates[action] = true;
                return true;
            }
        }

        return false;
    }

    public bool GetActionKeyUp(InputAction action)
    {
        if (KeyboardMapping.ContainsKey(action))
        {
            bool isKeyPressed = Input.GetKeyUp(KeyboardMapping[action]) || (KeyboardMapping[action] == KeyCode.Mouse0 && Input.GetMouseButtonUp(0)) || (KeyboardMapping[action] == KeyCode.Mouse1 && Input.GetMouseButtonUp(1));

            if (isKeyPressed)
            {
                keyStates[action] = false;
                return true;
            }
        }

        return false;
    }
    #endregion

    #region Key Binding Utilities
    public void Modifiy(InputAction type, KeyCode newer) => KeyboardMapping[type] = newer;

    public void SetDefaults()
    {
        Modifiy(InputAction.MoveForward, KeyCode.W);
        Modifiy(InputAction.MoveBackward, KeyCode.S);
        Modifiy(InputAction.MoveLeft, KeyCode.A);
        Modifiy(InputAction.MoveRight, KeyCode.D);
        Modifiy(InputAction.Run, KeyCode.LeftShift);
        Modifiy(InputAction.LookBehind, KeyCode.Space);
        Modifiy(InputAction.PauseOrCancel, KeyCode.Escape);
        Modifiy(InputAction.Confirm, KeyCode.Return);
        Modifiy(InputAction.Interact, KeyCode.E);
        Modifiy(InputAction.UseItem, KeyCode.Q);
        Modifiy(InputAction.Jump, KeyCode.Mouse0);
        Modifiy(InputAction.Slot0, KeyCode.Alpha1);
        Modifiy(InputAction.Slot1, KeyCode.Alpha2);
        Modifiy(InputAction.Slot2, KeyCode.Alpha3);
        Modifiy(InputAction.Slot3, KeyCode.Alpha4);
        Modifiy(InputAction.Slot4, KeyCode.Alpha5);
        Modifiy(InputAction.Slot5, KeyCode.Alpha6);
        Modifiy(InputAction.Slot6, KeyCode.Alpha7);
        Modifiy(InputAction.Slot7, KeyCode.Alpha8);
        Modifiy(InputAction.Slot8, KeyCode.Alpha9);
        Debug.Log("Settings reset.");
    }
	#endregion
	
    #region Save & Load Logic
	public void Save(string fileName)
	{
		string filePath = Application.persistentDataPath + "/basicsshowControls_" + fileName + ".xml";
		XmlSerializer serializer = new XmlSerializer(typeof(List<InputMapping>));

		List<InputMapping> inputMappings = new List<InputMapping>();
		foreach (var kvp in KeyboardMapping)
		{
			inputMappings.Add(new InputMapping { Action = kvp.Key, KeyCode = kvp.Value });
		}

		using (StreamWriter writer = new StreamWriter(filePath))
		{
			serializer.Serialize(writer, inputMappings);
		}
	}

    public void Load(string fileName)
    {
        string filePath = Application.persistentDataPath + "/basicsshowControls_" + fileName + ".xml";
        string oldPath = Application.persistentDataPath + "/basicsshowControls_" + fileName + ".dat";
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        if (File.Exists(oldPath))
        {
            using (FileStream fileStream = File.OpenRead(oldPath))
            {
                KeyboardMapping = (Dictionary<InputAction, KeyCode>)binaryFormatter.Deserialize(fileStream);
            }
            File.Delete(oldPath);
            Save(fileName);
            return;
        }

        if (File.Exists(filePath))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<InputMapping>));
            List<InputMapping> inputMappings;
            using (StreamReader reader = new StreamReader(filePath))
            {
                inputMappings = (List<InputMapping>)serializer.Deserialize(reader);
            }

            KeyboardMapping = new Dictionary<InputAction, KeyCode>();
            foreach (var mapping in inputMappings)
            {
                KeyboardMapping[mapping.Action] = mapping.KeyCode;
            }
        }
        else
        {
            SetDefaults();
            Save(fileName);
            Debug.Log("Controls_" + fileName + " doesn't exist. Loading defaults...");
        }
    }
    #endregion
}

#region Input Mapping
public class InputMapping
{
    public InputAction Action { get; set; }
    public KeyCode KeyCode { get; set; }
}
#endregion