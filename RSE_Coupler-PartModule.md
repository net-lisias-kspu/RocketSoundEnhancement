PartModule for Decouplers, LaunchClamps and Docking Ports.

SoundLayers node only serves as a wrapper for this PartModule. Multiple SoundLayers does not work for the same Action and only the first one will be applied.

SoundLayer "**name**" values for Part Types:
| value | Part Type| 
| :------------- | :----------: |
| **decouple** | Decouplers |
| **activate** | LaunchClamps |
| **dock** | Docking Ports |
| **undock** | Docking Ports |

Example Module Manager Patch from Rocket Sound Enhancement Stock Configs

	@PART[radialDecoupler*]:FOR[RocketSoundEnhancement]
	{
		!sound_decoupler_fire = decouple //Remove Stock Audio
		MODULE
		{
			name = RSE_Coupler
			SOUNDLAYER
			{
				// name of Part Type
				name = decouple
				audioClip = RocketSoundEnhancement/Sounds/Decouplers/Decoupler_1
				spread = 0.25
				channel = ShipBoth
				volume = 0.5
				pitch = 2
			}
		}
	}