#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.DataLogger;
using FTOptix.HMIProject;
using FTOptix.EventLogger;
using FTOptix.NativeUI;
using FTOptix.CoreBase;
using FTOptix.SQLiteStore;
using FTOptix.Store;
using FTOptix.OPCUAServer;
using FTOptix.Retentivity;
using FTOptix.NetLogic;
using FTOptix.Alarm;
using FTOptix.Core;
#endregion

[CustomBehavior]
public class BombaBehavior : BaseNetBehavior
{
    public override void Start()
    {
        // Insert code to be executed when the user-defined behavior is started
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined behavior is stopped
    }
[ExportMethod]
public void EncenderMotor
    {
        Node.
    }
#region Auto-generated code, do not edit!
    protected new Bomba Node => (Bomba)base.Node;
#endregion
}
