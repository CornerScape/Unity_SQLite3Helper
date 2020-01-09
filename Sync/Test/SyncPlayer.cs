#if UNIT_TEST
using Szn.Framework.Sync;

public enum SyncPlayerEnum
{
    ShortValue,
    IntValue,
    LongValue,

    BoolValue,
    CharValue,

    FloatValue,
    DoubleValue,

    StringValue
}

public class SyncPlayer : SyncBase
{
    [Sync((int)SyncPlayerEnum.ShortValue)]
    public short ShortValue { get; private set; }

    [Sync((int)SyncPlayerEnum.IntValue)]
    public int IntValue { get; private set; }

    [Sync((int)SyncPlayerEnum.LongValue)]
    public long LongValue { get; private set; }

    [Sync((int)SyncPlayerEnum.BoolValue)]
    public bool BoolValue { get; private set; }

    [Sync((int)SyncPlayerEnum.CharValue)]
    public char CharValue { get; private set; }

    [Sync((int)SyncPlayerEnum.FloatValue)]
    public float FloatValue { get; private set; }

    [Sync((int)SyncPlayerEnum.DoubleValue)]
    public double DoubleValue { get; private set; }

    [Sync((int)SyncPlayerEnum.StringValue)]
    public string StringValue { get; private set; }

    public SyncPlayer(short InShortValue, int InIntValue, long InLongValue, bool InBoolValue, char InCharValue, float InFloatValue, double InDoubleValue, string InStringValue)
    {
        ShortValue = InShortValue;
        IntValue = InIntValue;
        LongValue = InLongValue;
        BoolValue = InBoolValue;
        CharValue = InCharValue;
        FloatValue = InFloatValue;
        DoubleValue = InDoubleValue;
        StringValue = InStringValue;
    }
}


#endif