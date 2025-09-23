# WebXR Setup - gjtiquia

## Setup Instructions

### Pre-requisites

You must first install the necessary dependencies for the project

First install `WebXR Export` and `WebXR Interactions` and everything else following the docs here
- https://github.com/De-Panther/unity-webxr-export/blob/master/Documentation/Using-XR-Interaction-Toolkit.md

Basically what you should be doing is 
- Install packages `WebXR Export` and `WebXR Interactions`
- Install packages `XR Interaction Toolkit`
- Import sample `Starter Assets` from `XR Interaction Toolkit`
- Import sample `XR Interaction Toolkit Sample` from `WebXR Interactions`
- Fix problems in `Project Settigs > XR Plug-in Management > Project Validation` 
- This should install `XR Hands` and related samples

### XR Interaction WebXR Hands Setup

Now you need to recreate `XR Interaction WebXR Hands Setup` 
- because the one in the `WebXR Interactions` sample is outdated
- (hard to include in the .unitypackage due to the complex dependencies)
- first copy and paste the existing `XR Interaction WebXR Hands Setup` prefab from the sample into your own /Prefabs
- open the copied prefab
- delete the `XR Origin (XR Rig)` game object
- drag `XR Origin Hands (XR Rig)` from `XR Interaction Toolkit`'s `Hands Interaction Demo` sample prefabs

Now you can drag this prefab into any scene to turn it into a VR scene
- remember to delete any existing cameras
- remember to also give it a floor to stand on

### DesktopController Setup

You can import the .unitypackage that contains the `DesktopController` prefab.

Make sure you have completed the Pre-requisites and downloaded the necessary dependencies

Assets > Import Package > Custom Package...

Select the .unitypackage you want to import

This should import into /Plugins/gjtiquia/WebXRSetup

In /DesktopController drag the `DesktopControllerManager` prefab into a scene with `XR Interaction WebXR Hands Setup`

Set the corresponding External References (which should be components in `XR Interaction WebXR Hands Setup`)

## Maintainer Notes

### Export as Package instructions

Assets > Export Package...

Click None

Then click the gjtiquia/WebXRSetup folder

Include dependencies and Include all scripts

Click Export...

Choose a parent folder (eg. /Builds/Plugins, cuz Builds is git ignored)

Create a folder and enter it (eg. WebXRSetup_v0.1)

Create a name (eg. WebXRSetup_v0.1)

Click Save

You should see a WebXRSetup_v0.1.unitypackage and a /WebXRSetup_v0.1 generated



