## Description
**SoundLayer** is a config node that is used to store audio data and parameters.


| Properties | Description | 
| :------------- | :----------: |
| **name** | Name of this SoundLayer, Must be unique. |
| **audioClip** | Path of your audio file eg: *ModName/Sounds/MySound* |
| **loop** | If this SoundLayer is a loop: *True / False (Default)* |
| **loopAtRandom** | Start the loop at a random location on the sample: *True / False (Default)* |
| **channel** | *ShipInternal, ShipExternal, ShipBoth (Default)*. See [Channel](#channel) |
| **spread** | (0.0-1.0) from 0 Degrees to 360 Degrees, see [AudioSource.Spread (Unity - Scripting API)](https://docs.unity3d.com/ScriptReference/AudioSource-spread.html). |
| **data** | Used to store which type of control is used for this SoundLayer |
| **volume** | *FXCurve* |
| **pitch** | *FXCurve* |
| **massToVolume** | Attenuate the Volume based on Ship or Part Mass, [**ShipEffects and Collisions**](https://github.com/ensou04/RocketSoundEnhancement/wiki/ShipEffects-and-Collisions) Only, *FXCurve* |
| **massToPitch** | Attenuate the Pitch based on Ship or Part Mass, [**ShipEffects and Collisions**](https://github.com/ensou04/RocketSoundEnhancement/wiki/ShipEffects-and-Collisions) Only, *FXCurve* |

## Channel
| Values | Description | 
| :------------- | :----------: |
| **ShipInternal** |  Assigns the SoundLayer to only be played  on the Internal Camera (IVA). This Channel Bypasses any Sound Effects including Listener Effects, see [AudioSource (Unity - Scripting API)](https://docs.unity3d.com/ScriptReference/AudioSource.html) |
| **ShipExternal** |  Assigns the SoundLayer to only be played on the External Camera |
| **ShipBoth** |  Default. Play this SoundLayer regardless of Active Camera |

