using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControllerManager : MonoBehaviour {

    public InputDevice[] m_InputDevices = new InputDevice[4];

    private InputDevice m_P1, m_P2, m_P3, m_P4;
    
    public int m_DevicesAssigned = -1;

    void Awake()
    {
        // Assign null devices to the array
        m_InputDevices[0] = m_P1;
        m_InputDevices[1] = m_P2;
        m_InputDevices[2] = m_P3;
        m_InputDevices[3] = m_P4;
        Setup();
    }

    void Setup()
    {
        for (int i = 0; i < InputManager.Devices.Count; i++)
        {
            if (InputManager.Devices[i].IsAttached) // Get all connected devices and add them to the list.
                AddDeviceToArray(InputManager.Devices[i]);
        }
    }

    public bool AddDeviceToArray(InputDevice _newDevice)
    {
        if (m_InputDevices[0] == null) // Check if array slot has been assigned
        {
            m_InputDevices[0] = _newDevice;
            m_DevicesAssigned++;
            return true;
        }
        else if (m_InputDevices[1] == null)
        {
            m_InputDevices[1] = _newDevice;
            m_DevicesAssigned++;
            return true;
        }
        else if (m_InputDevices[2] == null)
        {
            m_InputDevices[2] = _newDevice;
            m_DevicesAssigned++;
            return true;
        }
        else if (m_InputDevices[3] == null)
        {
            m_InputDevices[3] = _newDevice;
            m_DevicesAssigned++;
            return true;
        }

        return false;
    }

    public InputDevice GiveInput(GameObject _player)
    {
        InputDevice _device;
        for (int i = 0; i < m_InputDevices.Length; i++)
        {
            if (m_InputDevices[i] != null)
            {
                _device = m_InputDevices[i]; // Grab a device
                m_InputDevices[i] = null; // Remove it from the array
                return _device;
            }
        }
        return null;
    }

}
