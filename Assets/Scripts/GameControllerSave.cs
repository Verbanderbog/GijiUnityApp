using System;
using System.Collections.Generic;

public class GameControllerSave
{
    public Dictionary<string, int> flags;
    public TimeSpan gametime;

    public GameControllerSave(Dictionary<string, int> flags, TimeSpan gametime)
    {
        this.flags = flags;
        this.gametime = gametime;
    }
}