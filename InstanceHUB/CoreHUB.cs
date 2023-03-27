using FirInstanceCell;
using UnityEngine;

public static class CoreHUB
{
    public static MonoBehaviour logicCore => LogicCore.GetValue();
    public static MonoBehaviour diagramOperator => DiagramOperator.GetValue();
    public static MonoBehaviour microphonOperator => MicrophonOperator.GetValue();
    public static MonoBehaviour audioOperator => AudioOperator.GetValue();
    public static MonoBehaviour noteBeltOperator => NoteBeltOperator.GetValue();
    public static MonoBehaviour resultOperator => ResultOperator.GetValue();
    public static MonoBehaviour playModeOperator => PlayModeOperator.GetValue();

    public static InstanceCell<MonoBehaviour> LogicCore = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> DiagramOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> MicrophonOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> AudioOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> NoteBeltOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> PlayModeOperator = new InstanceCell<MonoBehaviour>();
    public static InstanceCell<MonoBehaviour> ResultOperator = new InstanceCell<MonoBehaviour>();
}