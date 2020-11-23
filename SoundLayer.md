## Description
**SoundLayer** is a config node that is used to store audio data and parameters.


| Properties | Description | 
| :------------- | :----------: |
| **name** | Name of this SoundLayer |
| **audioClip** | Path of your audio file eg: *ModName/Sounds/MySound* |
| **loop** | If this SoundLayer is a loop: *True / False* |
| **loopAtRandom** | Start the loop at a random location on the sample: *True / False* |
| **channel** | ShipInternal, ShipExternal, ShipBoth. See [Channels](#channels) |
| **spread** | (0.0-1.0) from 0 Degrees to 360 Degrees, see [AudioSource.Spread (Unity - Scripting API)](https://docs.unity3d.com/ScriptReference/AudioSource-spread.html) for more details |
| **data** | Used to store which type of control is used for this SoundLayer |
| **volume** | *FXCurve* |
| **pitch** | *FXCurve* |
| **massToVolume** | Attenuate the Volume based on Ship or Part Mass, *FXCurve* |
| **massToPitch** | Attenuate the Pitch based on Ship or Part Mass, *FXCurve* |

## Channels
| Properties | Description | 
| :------------- | :----------: |
| **ShipInternal** |  Assigns the SoundLayer to be played only on the Internal Camera (IVA). This Channel Bypasses any Sound Effects including Listener Effects, see [AudioSource (Unity - Scripting API)](https://docs.unity3d.com/ScriptReference/AudioSource.html) |
| **ShipExternal** |  Assigns the SoundLayer to be played only on the External Camera |
| **ShipBoth** |  Assigns the SoundLayer to be played both on External Camera and Internal Camera |

