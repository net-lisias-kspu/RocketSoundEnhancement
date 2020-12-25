PartModule for Wheels.

| Parameters | Description | 
| :------------- | :----------: |
| **volume** | Master Volume for this PartModule |

| SoundLayer Group | Description | 
| :------------- | :----------: |
| **Torque{}** | Motor Torque, 0.0 - 1.0 (0-100%) |
| **Speed{}** | Motor Enabled Wheel Speed (m/s) |
| **Ground{}** | Wheel's Speed on the Ground (m/s) |
| **Slip{}** | Wheel Slip (m/s) |

SoundLayer "**data**" values for **Ground** and **Slip** Groups
| Values | Description | 
| :------------- | :----------: |
| **dirt** | Only play sounds while on Ground. eg: *Planet Surfaces* |
| **concrete** | Only play sounds while on Concrete: *KSC, Runway* |
| **vessel** | Only play sounds while colliding with a Vessel/Part |
| none | Play sounds at any situation (Default) |

Example Patch for TR-2L Ruggedized Vehicular Wheel

	@PART[wheelMed]:FOR[RocketSoundEnhancement]
	{
		MODULE
		{
			name = RSE_Wheels
			volume = 1.0

			Speed
			{
				SOUNDLAYER
				{
					name = Motor_Base
					audioClip = RocketSoundEnhancement/Sounds/Wheels/Motor_ElectricCar-Base
					loop = true
					loopAtRandom = true
					channel = ShipBoth
					spool = true
					spoolSpeed = 60
					spread = 0.12
					volume = 0.0  0.0
					volume = 1.0  0.5
					volume = 60.0  0.8
					pitch = 0.0  0.25
					pitch = 20.0  1.0
					pitch = 60.0  1.25
				}
				SOUNDLAYER
				{
					name = Motor_Whine
					audioClip = RocketSoundEnhancement/Sounds/Wheels/Motor_ElectricCar-Whine
					loop = true
					loopAtRandom = true
					channel = ShipBoth
					spool = true
					spoolSpeed = 60
					spread = 0.12
					volume = 0.0  0.0
					volume = 1.0  0.8
					volume = 20.0  0.7
					volume = 60.0  0.3
					pitch = 0.0  0.1
					pitch = 20.0  0.75
					pitch = 60.0  1.25
				}
			}
			Ground
			{
				SOUNDLAYER
				{
					name = Tire_Concrete
					audioClip = RocketSoundEnhancement/Sounds/Wheels/Tire_Concrete
					data = concrete
					loop = true
					loopAtRandom = true
					channel = ShipBoth
					spread = 0.12
					volume = 0.0  0.0
					volume = 1.0  0.1
					volume = 60.0  1.0
					pitch = 0.0  0.8
					pitch = 60.0  1.2
				}
				SOUNDLAYER
				{
					name = Tire_Ground-Dirt
					audioClip = RocketSoundEnhancement/Sounds/Wheels/Tire_Ground
					data = dirt
					loop = true
					loopAtRandom = true
					channel = ShipBoth
					spread = 0.12
					volume = 0.0  0.0
					volume = 1.0  0.1
					volume = 60.0  1.0
					pitch = 0.0  0.8
					pitch = 60.0  1.2
				}
			}
			Slip
			{
				SOUNDLAYER
				{
					name = Tire_Screech-Low-Concrete
					audioClip = RocketSoundEnhancement/Sounds/Wheels/Tire_Screech-Low
					data = concrete
					loop = true
					loopAtRandom = true
					channel = ShipBoth
					spool = true
					spoolSpeed = 2
					spread = 0.12
					volume = 0.0  0.0
					volume = 0.1  0.5
					volume = 1.0  1.0
					pitch = 0.0  1.2
					pitch = 1.0  1.0
				}
				SOUNDLAYER
				{
					name = Tire_Slip-Dirt
					audioClip = RocketSoundEnhancement/Sounds/Wheels/Tire_Ground
					data = dirt
					loop = true
					loopAtRandom = true
					channel = ShipBoth
					spool = true
					spoolSpeed = 2
					spread = 0.12
					volume = 0.0  0.0
					volume = 0.1  0.5
					volume = 1.0  1.0
					pitch = 0.0  2.0
					pitch = 1.0  1.8
				}
			}
		}
	}