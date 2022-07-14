using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Threading.Tasks;
using UnityEngine.Events;

public class ArduiConnection : MonoBehaviour
{
    public UnityEvent<string> OnSerialPortDataSent = new UnityEvent<string>();

    SerialPort port;

    private void Awake()
    {
        try
        {
            //port.WriteTimeout = 100;
            //port.ReadTimeout = 100;  
            port = new SerialPort("COM4", 9600);
        }
        catch (Exception ex)
        {            
            DisplayEditorPopup.DisplayDialog("Exception", ex.Message, "OK", "Cancel");
        }
    }

    private void OnEnable()
    {
        try
        {
            port.Open();
        }
        catch (Exception ex)
        {
            DisplayEditorPopup.DisplayDialog("Exception", ex.Message, "OK", "Cancel");
        }
    }

    private void OnDisable()
    {
        port.Close();
    }

    void Start()
    {

    }

    void Update()
    {
        if (port.IsOpen)
        {
            try //�n�Ounity�S����WArduino��Println ���ܡAReadLine�|LAG
            {
                string value = port.ReadLine();

                OnSerialPortDataSent?.Invoke(value);
            }
            catch (Exception ex)
            {
                OnSerialPortDataSent?.Invoke(null);

                Debug.Log(ex);
            }
        }
    }
}
