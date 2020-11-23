## Config - SHIPEFFECTS_SOUNDLAYERS
Physics based Sound Effects Engine. This Config is Global and gets applied to each vessel in-game.

Controls are fed on [**SoundLayer's**](https://github.com/ensou04/RocketSoundEnhancement/wiki/SoundLayer) **data** Parameter.

| Physics Controls | Description | 
| :------------- | :----------: |
| **Acceleration** | Change in Velocity (m/s) |
| **Jerk** | Change in Acceleration (m/s) |
| **AirSpeed** | Velocity in Atmosphere (m/s) |
| **GroundSpeed** | Surface Speed on Ground (m/s) |
| **Thrust** | Acceleration (m/s) from Total Ship Engine Thrust |

    SHIPEFFECTS_SOUNDLAYERS
    {
	    //	Play Sound Effects when the Ship Changes in Acceleration (Jerk)
	    SOUNDLAYER
	    {
		    name = Small_Rattles
		    audioClip = RocketSoundEnhancement/Sounds/Physics/Hull_RattleLayer-Medium
		    loop = true
		    loopAtRandom = true
		    spread = 0.5
		    channel = ShipInternal
		    
		    //	Control this SoundLayer with Jerk
		    data = Jerk
		    
		    //	Control the Volume based on the Amount of Jerk Dealt
		    //	Jerk : Volume
		    volume = 0.0  0.0
		    volume = 0.5  0.0
		    volume = 25  1.0
		    
		    //	Attenuate Pitch Based on Mass
		    //	Mass : Pitch
		    massToPitch = 0.0  1.2
		    massToPitch = 50  1.2
		    massToPitch = 100  1
		    massToPitch = 1000  0.8
		}
	}


## PartModule - ShipEffectsCollisions
Part Module to add Collision Sound Effects to Parts
| Collision Types | Description | 
| :------------- | :----------: |
| **CollisionEnter{}** | Impact |
| **CollisionStay{}** | Scraping, Sliding |
| **CollisionExit{}** | End of Collision |

Types of Collision Controls for SoundLayer's data.

| Data | Description | 
| :------------- | :----------: |
| **dirt** | Play sounds when this part collides with the ground eg: *Planet Surfaces* |
| **concrete** | Play sounds when this part collides with Static Objects eg: *KSC, Runway* |
| **vessel** | Play sounds when this part collides with another Vessel/Part |
| empty | Play sounds at any situation |

    MODULE
    {
	    name = ShipEffectsCollisions

		CollisionStay
		{
			SOUNDLAYER
			{
				name = Ground_Impact-High
				
				//	Only play this SoundLayer when we collide with concrete or dirt. not ships
				data = concrete-dirt
				
				// Multiple audioClips for Randomization so we don't get the Machine Gun Effect.
				audioClip = RocketSoundEnhancement/Sounds/Physics/Impacts/Ground_Impact-High-1
				audioClip = RocketSoundEnhancement/Sounds/Physics/Impacts/Ground_Impact-High-2
				audioClip = RocketSoundEnhancement/Sounds/Physics/Impacts/Ground_Impact-High-3
								    
				loop = false
				spread = 0.25
				channel = ShipBoth
				
				//	Control the Volume based on the Relative Velocity on Impact
				volume = 0.0 0.0
				volume = 15 0.0
				volume = 30 1.0
				
				//	Control the Pitch based on the Relative Velocity on Impact
				pitch = 0.0 0.8
				pitch = 15 0.8
				pitch = 30 1.2
			}
		}
	}

