# HoloSLAM

Record HoloLens poses: rotation in Euler angles and position in meter.

## Usage

1. Change the IP address according to your computer's IP address in Unity, build the visual studio solution and deploy to the HoloLens.
2. Start the sharing service.
3. Launch HoloPose on HoloLens, press play button in the Unity project. In the sharing service terminal, if you can see two users join, then go to the next step.
4. Use the HoloLensclicker to click and record the holoLens poses, in your unity, you should see the number of poses recorded.
5. Collect the pose data in in HoloLens `\Pictures\Camera Roll` folder. The file name should be in the format of `HoloData_XXX.json`.

## Dataset Collected

Please see `Holo_C-arm_Calibration` for details.
