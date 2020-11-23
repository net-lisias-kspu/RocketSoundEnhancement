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
| **vessel** | Only play sounds while on Vessel/Part |
| none | Play sounds at any situation (Default) |