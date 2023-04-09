using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class PoseTransform : MonoBehaviour
{
    public float gyro_x;
    public float gyro_y;
    public float gyro_z;
    public float accel_x;
    public float accel_y;
    public float accel_z;
    public static PoseTransform Instance { set; get; }
    private bool runToggle = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            RegisterEvents();
        }
    }

    public bool isLegUp()
    {
        return accel_y < 1;
    }
    public bool isLegDown()
    {
        return accel_y >= 1;
    }
    public bool toggleSquat()
    {
        return accel_y < -1;
    }

    #region Server
    private void RegisterEvents()
    {
        NetUtility.S_GYRO += Getgyro;
        NetUtility.S_ACCEL += Getaccel;
    }
    private void Getgyro(NetworkMessage msg, NetworkConnection cnn)
    {
        NetGyro netGyro = msg as NetGyro;
        this.gyro_x = netGyro.gyro_x;
        this.gyro_y = netGyro.gyro_y;
        this.gyro_z = netGyro.gyro_z;
    }
    private void Getaccel(NetworkMessage msg, NetworkConnection cnn)
    {
        NetAcceler netAcceler = msg as NetAcceler;
        this.accel_x = netAcceler.accel_x;
        this.accel_y = netAcceler.accel_y;
        this.accel_z = netAcceler.accel_z;
    }
    #endregion
}
