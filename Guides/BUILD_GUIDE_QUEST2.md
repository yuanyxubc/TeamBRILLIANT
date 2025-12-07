# BUILD GUIDE: Meta Quest 2 Deployment

Complete step-by-step guide to build and deploy "Harmony in Diversity" to Meta Quest 2.

**Target Device:** Meta Quest 2
**Platform:** Android
**XR Runtime:** OpenXR
**Build Time:** ~10-30 minutes (first build)

---

## TABLE OF CONTENTS

1. [Prerequisites](#1-prerequisites)
2. [Install Android Build Support](#2-install-android-build-support)
3. [Configure Project Settings](#3-configure-project-settings)
4. [Configure XR Plugin Management](#4-configure-xr-plugin-management)
5. [Configure Build Settings](#5-configure-build-settings)
6. [Configure Player Settings](#6-configure-player-settings)
7. [Optimize Quality Settings](#7-optimize-quality-settings)
8. [Prepare Quest 2 Device](#8-prepare-quest-2-device)
9. [Build and Deploy](#9-build-and-deploy)
10. [Troubleshooting](#10-troubleshooting)

---

## 1. PREREQUISITES

### Required Software

- ✅ **Unity 6000.2.4f1** (already installed)
- ✅ **Android SDK, NDK, JDK** (installed via Unity Hub)
- ⬜ **Meta Quest Developer Account** (free - create at developer.oculus.com)
- ⬜ **Meta Quest Mobile App** (on your phone)
- ⬜ **USB-C Cable** (to connect Quest 2 to PC)

### Optional but Recommended

- **SideQuest** (alternative deployment tool - sidequestvr.com)
- **ADB (Android Debug Bridge)** (comes with Android SDK)

---

## 2. INSTALL ANDROID BUILD SUPPORT

### Step 2.1: Check if Android Build Support is Installed

1. Open **Unity Hub**
2. Click **Installs** tab
3. Find your Unity version: **6000.2.4f1**
4. Click the **⚙ (gear icon)** next to it
5. Select **Add Modules**

### Step 2.2: Install Required Modules

Check that these modules are installed (if not, check them and click Install):

- ✅ **Android Build Support**
  - ✅ **Android SDK & NDK Tools**
  - ✅ **OpenJDK**

**Installation time:** 5-15 minutes depending on internet speed

### Step 2.3: Verify Installation

1. In Unity, go to **Edit > Preferences** (Windows) or **Unity > Preferences** (Mac)
2. Click **External Tools**
3. Verify these paths are set (should be automatic):
   - **Android SDK Tools**
   - **Android NDK**
   - **JDK**

If any are missing, click **Download** or manually browse to the installation location.

---

## 3. CONFIGURE PROJECT SETTINGS

### Step 3.1: Switch to Android Platform

1. In Unity, go to **File > Build Settings**
2. In the **Platform** list, select **Android**
3. Click **Switch Platform** button (bottom right)
   - ⏳ This will take 5-15 minutes as Unity reimports all assets for Android
   - Unity logo will appear in the corner showing progress
   - Wait until it completes

4. Verify the Unity icon appears next to **Android** in the platform list

### Step 3.2: Set Texture Compression

Still in **Build Settings** window:

1. Find **Texture Compression** dropdown
2. Select: **ASTC**
   - This is the best compression for Quest 2

---

## 4. CONFIGURE XR PLUGIN MANAGEMENT

### Step 4.1: Open XR Settings

1. Go to **Edit > Project Settings**
2. In the left sidebar, find **XR Plug-in Management**
3. If prompted to install XR Management, click **Install**

### Step 4.2: Configure Android XR Settings

1. In **XR Plug-in Management**, click the **Android tab** (Android robot icon)
2. Enable these checkboxes:
   - ✅ **OpenXR**
   - ✅ **Oculus** (if available - provides better Quest integration)

**IMPORTANT:** Make sure you're on the **Android tab**, not the PC/Windows tab!

### Step 4.3: Configure OpenXR Settings

1. In left sidebar, find **XR Plug-in Management > OpenXR**
2. Click the **Android tab**
3. Check for any warning icons (⚠) and click **Fix** if they appear

4. **Interaction Profiles:**
   - Click **+** button
   - Add: **Oculus Touch Controller Profile**
   - This ensures controller input works on Quest 2

5. **Features:**
   - Expand **OpenXR Feature Groups**
   - Enable: ✅ **Meta Quest Support**
   - If you don't see this, enable: ✅ **Oculus** under available features

### Step 4.4: Verify No Errors

At the bottom of OpenXR settings, check **Validation**:
- Should show: ✅ "No issues found"
- If you see errors, click the error and then click **Fix** button

---

## 5. CONFIGURE BUILD SETTINGS

### Step 5.1: Add Scenes to Build

1. Go to **File > Build Settings**
2. In **Scenes In Build** section:
   - Click **Add Open Scenes** (if your scene is currently open)
   - OR click **Add Scenes** and browse to: `Assets/Scenes/HarmonyInDiversity.unity`

3. Verify your scene appears with ✅ checkbox

4. **Remove any other scenes** from the build (uncheck them or click ✖)

### Step 5.2: Configure Build Settings

Still in Build Settings window:

**Development Build** (RECOMMENDED for testing):
- ✅ Check **Development Build**
- ✅ Check **Script Debugging** (optional - for debugging)
- ✅ Check **Wait for Managed Debugger** (optional - only if debugging)

**For final release:**
- ⬜ Uncheck **Development Build**

**Compression Method:**
- Select: **LZ4** (faster build) or **LZ4HC** (smaller size)

**Run Device:**
- Will show your Quest 2 once connected (later step)

---

## 6. CONFIGURE PLAYER SETTINGS

### Step 6.1: Open Player Settings

1. In **Build Settings** window, click **Player Settings...** (bottom left)
2. Player Settings panel opens in Inspector

### Step 6.2: Company and Product Name

In **Player Settings > Company Name**:
- Enter your name or team name (e.g., "Team BRILLIANT")

In **Product Name**:
- Enter: `Harmony in Diversity`

### Step 6.3: Icon Settings (Optional)

**Default Icon:**
- Drag a 512x512 PNG image (optional)

**Adaptive Icon:**
- Foreground/Background icons (optional for now)

### Step 6.4: Resolution and Presentation

Scroll to **Resolution and Presentation**:

**Default Orientation:**
- Set to: **Landscape Left** (doesn't matter much for VR)

**Resolution Scaling Mode:**
- Set to: **Fixed DPI**

### Step 6.5: Other Settings (CRITICAL)

Expand **Other Settings** section:

**Rendering:**
- Color Space: **Linear** (should already be set)
- Graphics API: **OpenGLES3** or **Vulkan**
  - Remove OpenGLES2 if present (click - button)
  - Recommended order: Vulkan, OpenGLES3

**Identification:**
- Package Name: Change to something unique
  - Format: `com.YourCompany.HarmonyInDiversity`
  - Example: `com.teambrilliant.harmonyindiversity`
  - **Must be all lowercase, no spaces**
  - This uniquely identifies your app

- Version: `0.1.0` (or leave as 1.0)
- Bundle Version Code: `1`

**Minimum API Level:**
- Set to: **Android 10.0 (API level 29)** or higher
- Quest 2 requires at least API 29

**Target API Level:**
- Set to: **Automatic (highest installed)**
- Or manually set to **API level 32+**

**Scripting Backend:**
- Set to: **IL2CPP** (REQUIRED for Quest 2)
- Do NOT use Mono (won't work on Quest)

**API Compatibility Level:**
- Set to: **.NET Standard 2.1**

**Target Architectures:**
- ✅ Check **ARM64** (MUST be checked)
- ⬜ Uncheck ARMv7 (Quest 2 doesn't need it)

### Step 6.6: Configuration

Expand **Configuration** section:

**Scripting Define Symbols:**
- Leave empty (or add custom defines if needed)

**Active Input Handling:**
- Set to: **Input System Package (New)** or **Both**
- Do NOT use "Input Manager (Old)" only

### Step 6.7: XR Settings

Scroll to **XR Settings** at the bottom:

- **Stereo Rendering Mode:**
  - Set to: **Multi-view** (best performance for Quest 2)
  - This is critical for good VR performance

---

## 7. OPTIMIZE QUALITY SETTINGS

### Step 7.1: Open Quality Settings

1. Go to **Edit > Project Settings**
2. Select **Quality** in left sidebar

### Step 7.2: Configure for Android

1. Under **Levels**, find the **Android (robot icon)** column
2. Click on the quality level row to select it (usually **Medium** or **High**)
3. This sets the default quality for Android builds

### Step 7.3: Adjust Settings for Quest 2 Performance

With the Android quality level selected:

**Rendering:**
- Pixel Light Count: `2-4`
- Texture Quality: **Full Res** or **Half Res** (if performance issues)
- Anisotropic Textures: **Per Texture**
- Anti Aliasing: **4x Multi Sampling** (good balance)

**Shadows:**
- Shadows: **Hard and Soft Shadows**
- Shadow Resolution: **Medium** or **High**
- Shadow Distance: `20-50` meters

**VSync:**
- VSync Count: **Don't Sync** (XR handles this automatically)

**LOD Bias:**
- LOD Bias: `1.5` or `2`

---

## 8. PREPARE QUEST 2 DEVICE

### Step 8.1: Enable Developer Mode

1. **Create Meta Developer Account:**
   - Go to: https://developer.oculus.com
   - Sign in with Facebook/Meta account
   - Accept developer terms

2. **Create Organization:**
   - In Developer Dashboard, create an organization (can be just your name)
   - This is required to enable Developer Mode

3. **Enable Developer Mode on Quest 2:**
   - Install **Meta Quest mobile app** on your phone
   - Sign in with same Meta account
   - In the app: **Menu > Devices > Your Quest 2**
   - Tap **Developer Mode**
   - Toggle it **ON**

### Step 8.2: Connect Quest 2 to PC

1. **Turn on Quest 2** and put it on
2. **Connect USB-C cable** from Quest 2 to PC
3. In the headset, you'll see a prompt: **"Allow USB debugging?"**
4. Check: ✅ **Always allow from this computer**
5. Click **OK**

### Step 8.3: Verify Connection

**Method 1: Unity Build Settings**
1. In Unity, go to **File > Build Settings**
2. With Android platform selected
3. Click **Refresh** button next to "Run Device"
4. Your Quest 2 should appear in the dropdown (might show as "Oculus Quest 2" or device serial number)

**Method 2: ADB Command (if Method 1 doesn't work)**
1. Open **Command Prompt** (Windows) or **Terminal** (Mac)
2. Navigate to Android SDK platform-tools:
   ```
   cd "C:\Program Files\Unity\Hub\Editor\6000.2.4f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools"
   ```
3. Run:
   ```
   adb devices
   ```
4. You should see your Quest 2 listed:
   ```
   List of devices attached
   1WMHH815T1234  device
   ```

If you see "unauthorized", go back to the headset and accept the USB debugging prompt.

---

## 9. BUILD AND DEPLOY

### Step 9.1: Final Pre-Build Checklist

- ✅ Scene added to Build Settings
- ✅ Android platform selected (Unity icon next to Android)
- ✅ OpenXR configured with Meta Quest Support
- ✅ Player Settings: IL2CPP, ARM64, correct Package Name
- ✅ Quest 2 connected and showing in Run Device
- ✅ Developer Mode enabled on Quest 2

### Step 9.2: Build and Run (Recommended)

This builds and automatically installs to Quest 2:

1. **Put on Quest 2 headset** (to see installation progress)
2. In Unity: **File > Build Settings**
3. Select your Quest 2 from **Run Device** dropdown
4. Click **Build And Run** button (bottom right)

5. **Choose save location** for APK:
   - Create folder: `Builds/Android/`
   - Save as: `HarmonyInDiversity.apk`

6. **Wait for build:**
   - Progress bar will show in Unity (bottom right)
   - Build time: 10-30 minutes for first build
   - Subsequent builds: 2-10 minutes

7. **Automatic installation:**
   - After build completes, Unity installs to Quest 2
   - In headset, you'll see installation progress
   - App launches automatically when done

### Step 9.3: Build Only (Alternative)

If you just want to create the APK without installing:

1. Click **Build** button instead of "Build And Run"
2. Save APK to `Builds/Android/HarmonyInDiversity.apk`
3. Manually install using SideQuest or ADB (see Step 9.4)

### Step 9.4: Manual Installation via ADB (if needed)

If automatic installation fails:

1. Open Command Prompt / Terminal
2. Navigate to platform-tools folder (see Step 8.3)
3. Run:
   ```
   adb install "D:\IMTC 505 Projects\Final Project\TeamBRILLIANT\Builds\Android\HarmonyInDiversity.apk"
   ```
4. Wait for "Success" message

### Step 9.5: Launch the App on Quest 2

1. In Quest 2 headset, press **Oculus button** to open menu
2. Go to **App Library**
3. Click filter dropdown (top right)
4. Select: **Unknown Sources**
5. Find **Harmony in Diversity**
6. Click to launch!

---

## 10. TROUBLESHOOTING

### Build Error: "IL2CPP error for method..."

**Solution:**
1. Edit > Project Settings > Player > Other Settings
2. Scripting Backend: Ensure **IL2CPP** is selected
3. Target Architectures: Ensure **ARM64** is checked

### Build Error: "Unable to list target platforms"

**Solution:**
1. Unity Hub > Installs > Click ⚙ on your Unity version
2. Add Modules > Ensure Android Build Support (with SDK/NDK) is installed

### Quest 2 Not Showing in Run Device

**Solution:**
1. Check USB cable is connected properly
2. In Quest 2 headset, accept USB debugging prompt
3. Try different USB port on PC (USB 3.0 preferred)
4. Run `adb devices` to verify connection
5. Click "Refresh" button in Build Settings

### App Crashes on Launch

**Solution:**
1. Check Console for errors before building
2. Verify all scripts compile without errors
3. Build with Development Build enabled to see crash logs
4. Run `adb logcat` to see detailed error logs

### Black Screen on Quest 2

**Solution:**
1. Verify XR Plugin Management is set up correctly
2. Check that scene has XR Origin/Rig
3. Check Main Camera is tagged as "MainCamera"
4. Verify OpenXR is enabled (not Oculus plugin only)

### Performance Issues / Low FPS

**Solution:**
1. Quality Settings > Reduce Shadow Quality
2. Reduce particle system counts
3. Enable GPU Instancing on materials
4. Use XR > Stereo Rendering Mode: Multi-view
5. Profile with Unity Profiler (Window > Analysis > Profiler)
6. Build with Release configuration (uncheck Development Build)

### "Package Name is Invalid"

**Solution:**
1. Player Settings > Other Settings > Package Name
2. Must be format: `com.company.appname`
3. All lowercase, no spaces
4. Example: `com.teambrilliant.harmonyindiversity`

### Input Not Working (Controllers)

**Solution:**
1. XR Plug-in Management > OpenXR > Android tab
2. Verify "Oculus Touch Controller Profile" is added
3. Check Input Actions asset is configured
4. Verify XR Interaction Manager exists in scene

### App Not in App Library

**Solution:**
1. Check "Unknown Sources" filter in App Library
2. Apps from Unity appear in Unknown Sources, not main library
3. Or search for "Harmony in Diversity" in search bar

---

## 11. POST-BUILD TESTING

### In-Headset Testing Checklist

- [ ] Scene 1 starts automatically
- [ ] Sparkles visible at spawn points
- [ ] Narration text readable
- [ ] Scene 2: Orbs spawn correctly
- [ ] Orbs have correct colors
- [ ] Controllers appear and track correctly
- [ ] Can point ray at orbs
- [ ] Can pull threads (trigger button)
- [ ] Threads create connections
- [ ] Connection counter updates
- [ ] All 5 scenes complete successfully
- [ ] End screen appears
- [ ] Restart button works
- [ ] Performance is smooth (72 FPS)

### Check Frame Rate

While in headset:
1. Open **Settings** (hold Oculus button)
2. Go to **Developer** section
3. Enable **Performance Overlay**
4. Check FPS (should be 72 consistently)

---

## 12. ITERATION WORKFLOW

### Quick Rebuild for Testing

After making changes:

1. **File > Build Settings**
2. **Build And Run** (Unity remembers your settings)
3. Wait for incremental build (~2-5 minutes)
4. App automatically updates on Quest 2

### Faster Iteration with Unity Link (Advanced)

For fastest iteration, use **Oculus Link** or **Air Link**:
1. Connect Quest 2 to PC (USB or WiFi)
2. Enable Oculus Link in headset
3. Play directly from Unity Editor
4. Changes apply instantly

---

## 13. BUILD SETTINGS SUMMARY

**Quick Reference for Build Configuration:**

```
Platform: Android
Texture Compression: ASTC
Development Build: ✅ (for testing)

XR Plug-in Management (Android):
  - OpenXR: ✅
  - Oculus: ✅ (if available)

OpenXR (Android):
  - Meta Quest Support: ✅
  - Oculus Touch Controller Profile: Added

Player Settings > Other Settings:
  - Package Name: com.teambrilliant.harmonyindiversity
  - Minimum API Level: Android 10.0 (API 29)
  - Scripting Backend: IL2CPP
  - Target Architectures: ARM64 only
  - Graphics API: Vulkan, OpenGLES3

Player Settings > XR Settings:
  - Stereo Rendering Mode: Multi-view

Quality Settings (Android):
  - Anti Aliasing: 4x MSAA
  - VSync: Don't Sync
```

---

## COMPLETE! 🎉

Your app should now be running on Quest 2!

**Next Steps:**
- Test all 5 scenes thoroughly
- Check performance (should be 72 FPS)
- Iterate and improve based on testing
- Share with others for feedback!

**Build time decreases:**
- First build: 10-30 minutes
- Subsequent builds: 2-10 minutes
- Iterative builds (small changes): 1-3 minutes

---

**Questions or issues? Check the Troubleshooting section or the Console logs in Unity!**

