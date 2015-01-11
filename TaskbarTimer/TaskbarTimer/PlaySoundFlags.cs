using System;

namespace TaskbarTimer {
    /// <summary>
    /// Flags for calling unmanaged PlaySound function 
    /// </summary>
    [Flags]
    public enum PlaySoundFlags : int {
        SND_SYNC = 0x0,     // play synchronously (default)
        SND_ASYNC = 0x1,    // play asynchronously
        SND_NODEFAULT = 0x2,    // silence (!default) if sound not found
        SND_MEMORY = 0x4,       // pszSound points to a memory file
        SND_LOOP = 0x8,     // loop the sound until next sndPlaySound
        SND_NOSTOP = 0x10,      // don't stop any currently playing sound
        SND_NOWAIT = 0x2000,    // don't wait if the driver is busy
        SND_ALIAS = 0x10000,    // name is a registry alias
        SND_ALIAS_ID = 0x110000,// alias is a predefined ID
        SND_FILENAME = 0x20000, // name is file name
        SND_RESOURCE = 0x40004, // name is resource name or atom
    }
}