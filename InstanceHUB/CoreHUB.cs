using FirInstanceCell;
using UnityEngine;

public static class CoreHUB
{
    public static InstanceCell<MonoBehaviour> LogicCore = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> NoteManager = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> DiagramOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> MicrophonOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> NoteBeltOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> ResultOperator = new InstanceCell<MonoBehaviour>();
}