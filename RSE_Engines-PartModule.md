PartModule for Engines.

| Parameters | Description | 
| :------------- | :----------: |
| **volume** | Master Volume for this PartModule |

| SoundLayer Group | Description | 
| :------------- | :----------: |
| *EngineId{}* | Replace "EngineId" with the Engine's. Default is "**Engine**" unless specified under [ModuleEngines](https://wiki.kerbalspaceprogram.com/wiki/Module#ModuleEngines) or [ModuleEnginesFX](https://wiki.kerbalspaceprogram.com/wiki/Module#ModuleEnginesFX) PartModule. |
| **Engage{}** | Engage Group |
| **Disengage{}** | Disengage Group |
| **Flameout{}** | Flameout Group |

Example Module Manager Config from Rocket Sound Enhancement Stock Configs for [S3 KS-25 "Vector" Liquid Fuel Engine](https://wiki.kerbalspaceprogram.com/wiki/S3_KS-25_%22Vector%22_Liquid_Fuel_Engine)

	@PART[SSME]:FOR[RocketSoundEnhancement]
	{
		// Remove Stock Audio.
		@EFFECTS{@running_closed{!AUDIO{}}}
		@EFFECTS{@engage{!AUDIO{}}}
		@EFFECTS{@flameout{!AUDIO{}}}
		
		MODULE
		{
			name = RSE_Engines
			volume = 1.0 // Master Volume for this PartModule
			
			// Vector's Engine ID
			// Crossfade between two SoundLayers based on Thrust Power
			KS25
			{
				SOUNDLAYER
				{
					name = ThrustHigh
					audioClip = RocketSoundEnhancement/Sounds/Engines/Liquid_Heavy-High
					loop = true
					spread = 0.5
					channel = ShipBoth
					volume = 0.0  0.0
					volume = 0.33  0.0
					volume = 0.66  1.0
					volume = 1.0  1.0
					pitch = 1.0
				}
				SOUNDLAYER
				{
					name = ThrustLow
					audioClip = RocketSoundEnhancement/Sounds/Engines/Liquid_Heavy-Low
					loop = true
					spread = 0.5
					channel = ShipBoth
					volume = 0.0  0.0
					volume = 0.33  1.0
					volume = 0.66  0.0
					volume = 1.0  0.0
					pitch = 1.0
				}
			}
			Disengage
			{
				SOUNDLAYER
				{
					name = Disengage
					audioClip = sound_vent_soft
					channel = ShipBoth
					volume = 0.2
					pitch = 2.0
				}
			}
			Flameout
			{
				SOUNDLAYER
				{
					name = FlameOut
					audioClip = sound_explosion_low
					channel = ShipBoth
					volume = 1.0
					pitch = 2.0
				}
			}
		}
	}