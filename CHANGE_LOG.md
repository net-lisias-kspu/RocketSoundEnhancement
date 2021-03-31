# Rocket Sound Enhancement :: Change Log

* 2020-1123: 0.5.3 (ensou04) for KSP 1.5.1
	+ Fixed Confusing Naming Conventions
	+ Added Tooltips for Lowpass Filter Settings
	+ Updated Configs
* 2020-1121: 0.5.2 (ensou04) for KSP 1.5.1
	+ Added Option to Muffle Chatterer Sounds (off by default)
	+ Changed ShipEffects Thrust controls to TotalThrust / Mass instead of just Total Thrust Power
	+ Updated ShipEffects.cfg to reflect new change
	+ Reduced Pitching for Medium Rattles layer in ShipEffects.cfg
* 2020-1120: 0.5.1 (ensou04) for KSP 1.5.1
	+ Download the Config/Library from v0.5.0 for the patches and sounds
	+ Work on the limiter is still being done and might be replaced in the future.
	+ Plugin
			- Added Whitelist for Chatterer
			- Reduced sound cracking from Audio Limiter by moving the Volume Reduction code from LateUpdate() to OnAudioFilterRead()
	+ Configs
			- Fixed Collision Module getting applied to parts that does not have crashTolerance
* 2020-1119: 0.5.0 (ensou04) for KSP 1.5.1
	+ Rewrite from the ground up.
	+ IMPORTANT:
	+ Remove the old v0.4.0 RSE entirely, nuke it. not a single code was carried over to v0.5.0 not deleting will result in an unexpected outcome.
	+ Changelog:
			- EVERYTHING
			- ShipEffects is integrated and rewritten from scratch
			- Old RSE plugin now deprecated. delete the old v0.4.0 folder entirely
			- All new sounds recorded and created.
			- Wheel Sounds
			- Collision Sounds
			- Decouplers, Docking Ports and Launch Clamps
			- Stock Rocket Engine Patches
			- Wheel Patches
			- Restock Support
	+ Dependencies (Default Sound Library and Configs):
			- Module Manager
* 2019-0102: 0.4.0 (ensou04) for KSP 1.5.1
	+ added one shot samples to most lower stage engines.
* 2018-1201: 0.3.1 (ensou04) for KSP 1.5.1
	+ No changelog provided
* 2018-1128: 0.3.0 (ensou04) for KSP 1.5.1
	+ Ammonialox, Cryogenic-UpperLower patches
	+ Thump! Featured on most plumes
	+ Redone lowpass curves
	+ Volume are now based on Engine Thrust
	+ New SRB Crackle sound layer for Solid-Lower
	+ Reverted Engage, Disengage and Flameout sounds to stock.
* 2018-1118: 0.2.0 (ensou04) for KSP 1.5.1
	+ Plugin Update
	+ Engines now uses a custom Audio handler via RSEAudio plugin
	+ Fixed patch errors: Hydrogen-NTR and Hydrogen-NTR-HightTemp were referencing an incorrect plume config
	+ Engines now have a realistic lowpass-based volume rolloff attenuation
	+ Rebalanced and improved sound assets
* 2018-1116: 0.1.1 (ensou04) for KSP 1.5.1
	+ Fix Small Engine Volumes
		- Hypergolic-OMS White and Red
		- Kerolox-Vernier
		- Removed sound_lqd_tiny
* 2018-1114: 0.1.0 (ensou04) for KSP 1.5.1 PRE-RELEASE
	+ New Solid Rocket Booster Sounds
	+ Running Sound Patches:
			- Hydrogen-NTR
			- Hydrogen-NTR-HighTemp
			- Hydrolox-Lower
			- Hydrolox-Upper
			- Hypergolic-Lower
			- Hypergolic-OMS-Red
			- Hypergolic-OMS-White
			- Hypergolic-Upper
			- Kerolox-Lower
			- Kerolox-Lower-F1
			- Kerolox-Upper
			- Kerolox-Vernier
			- Solid-Lower
			- Solid-Sepmotor
			- Solid-Upper
			- Solid-Vacuum
