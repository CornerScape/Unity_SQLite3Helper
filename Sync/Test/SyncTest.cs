#if UNIT_TEST
using System;
using UnityEngine;

public class SyncTest : MonoBehaviour
{
    private SyncPlayer player;

    private Action<string, short, short> shortAction;
    private Action<string, int, int> intAction;
    private Action<string, long, long> longAction;
    private Action<string, bool, bool> boolAction;
    private Action<string, char, char> charAction;
    private Action<string, float, float> floatAction;
    private Action<string, double, double> doubleAction;
    private Action<string, string, string> stringAction;
    // Use this for initialization
    void Start()
    {
        player = new SyncPlayer(0, 1, 2, true, 'a', 0.1f, 0.2, "abc" );

        shortAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue, InCurrentValue));
        };
        player.RegisterPropertyChanged((int) SyncPlayerEnum.ShortValue, shortAction);

        intAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue,
                InCurrentValue));
        };
        player.RegisterPropertyChanged((int)SyncPlayerEnum.IntValue, intAction);

        longAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue,
                InCurrentValue));
        };
        player.RegisterPropertyChanged((int)SyncPlayerEnum.LongValue, longAction);

        boolAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue,
                InCurrentValue));
        };
        player.RegisterPropertyChanged((int)SyncPlayerEnum.BoolValue, boolAction);

        charAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue,
                InCurrentValue));
        };
        player.RegisterPropertyChanged((int)SyncPlayerEnum.CharValue, charAction);

        floatAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue,
                InCurrentValue));
        };
        player.RegisterPropertyChanged((int)SyncPlayerEnum.FloatValue, floatAction);

        doubleAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue,
                InCurrentValue));
        };
        player.RegisterPropertyChanged((int)SyncPlayerEnum.DoubleValue, doubleAction);

        stringAction = (InPropertyName, InOldValue, InCurrentValue) =>
        {
            Debug.LogError(string.Format("On property {0} from {1} to {2}.", InPropertyName, InOldValue,
                InCurrentValue));
        };
        player.RegisterPropertyChanged((int)SyncPlayerEnum.StringValue, stringAction);
    }

    // Update is called once per frame
    void OnGUI()
    {
        GUI.skin.button.fontSize = 32;
        if (GUILayout.Button("Short"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.ShortValue, 100);

            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.ShortValue, shortAction);
        }

        if (GUILayout.Button("Int"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.IntValue, 101);
            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.IntValue, intAction);
        }
        
        if (GUILayout.Button("Long"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.LongValue, 102);
            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.LongValue, longAction);
        }
        
        if (GUILayout.Button("bool"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.BoolValue, false);
            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.BoolValue, boolAction);
        }

        if (GUILayout.Button("char"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.CharValue, 'z');
            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.CharValue, charAction);
        }

        if (GUILayout.Button("float"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.FloatValue, 0.01f);
            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.FloatValue, floatAction);
        }

        if (GUILayout.Button("double"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.DoubleValue, 0.02);
            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.DoubleValue, doubleAction);
        }

        if (GUILayout.Button("string"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.StringValue, "zyx");
            player.UnRegisterPropertyChanged((int)SyncPlayerEnum.StringValue, stringAction);
        }

        if (GUILayout.Button(">Short<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.ShortValue, 200);
        }

        if (GUILayout.Button(">Int<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.IntValue, 201);
        }

        if (GUILayout.Button(">Long<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.LongValue, 202);
        }

        if (GUILayout.Button(">bool<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.BoolValue, true);
        }

        if (GUILayout.Button(">char<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.CharValue, 'm');
        }

        if (GUILayout.Button(">float<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.FloatValue, 0.001f);
        }

        if (GUILayout.Button(">double<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.DoubleValue, 0.002);
        }

        if (GUILayout.Button(">string<"))
        {
            player.OnSyncOne((int)SyncPlayerEnum.StringValue, "mno");
        }

        if (GUILayout.Button("Log"))
        {
            Debug.LogError(player);
        }
    }
}
#endif