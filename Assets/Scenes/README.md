
# Easy Unity Modular Template

This set of **components** and **scripts** allows for an easy to use modular approach to a collaborative Unity project. I created this to be able to get a team to a collaborative point quickly. 

It is designed so that each member can work on a seperate scene, while still enjoying the benefits of any global/modular code that other team members may have created.

Any thoughts on how to improve, please reach out and let me know! william.j.bechard@gmail.components

### Design
#### Folder Structure/Naming
The folders are set up into two primary categories, **Global** and **Menu**.

#### Global Elements
Able to be added to any scene. **Note:** Init is the only prefab object that needs to be brought into a new scene. It contains a property Mandatory Scenes to Load which handles pulling in other global elements into the scene after start.

#### Menu Elements
These **scenes**/**objects** are used to easily get a menu system up and running.

# Getting Started
Start a new blank project, or use a current one. From Unity's top menu bar select **Assets** -> **Import Package** -> **Custom Package**. 

Then simply select the **UnityModularTemplate** custom package, and allow all contents to be installed into the project.

# Menu
Open the MainMenu Scene under the folders: **Scenes** -> **Menu**

Look in the **Object Hierarchy** and you will see the **Init** Prefab.

    -Init Prefab: 
        Loads Mandatory Scenes
        Loads Extra Scenes
    
Notice that the Init Object in this scene has no Extra Scenes to load. However if you click play you will notice several scenes added to the Hierarchy. This is by design, as the Init script will automatically load Mandatory Scenes (SceneLoader and AudioManager) if they are not detected already in the Hierarchy. These scenes are mandatory as any scene should need both a way to move to a different scene, and audio that could be played.

An example of needing to use Extra Scenes to load would be any scenes that require an external scene to be loaded in (Like the Game Scene needing the Game Manager scene)

    -MusicPlayerSetup Prefab:
        Allows for Background Music
        Allows for Ambient persistent noise

The MusicPlayerSetup Object is not required. However, having it in this scene allows for background music to play when the scene starts. If the Music Name propery is set to No Music then any previously played music will continue to play (alternatively if this is the desired intent you could also just not have this prefab in the scene at all). If the Background SFX Name is blank then any playing background persistent noise is stopped.

### How to hook up a scene change with UI Button click

Add a **UI Button Game Object**

Then add the **Load Scene** script to the **Button Game Object**. Next in the Inspector select + to add an event to the **On Click() event handler**. Drag the **Load Scene script** into the **target field**. Select **LoadScene** -> **loadScene()**.  

To add a sound to the click event do the following as well. Add the **Play Sound One Shot script** to the **Button Game Object**. On the **On Click() event handler** in the Inspector, click + to add another event. Drag the **Play Sound One Shot script** to the **target field**. Select **Play Sound One Shot** -> **playSound()** 


# Audio Manager

The Audio Manager is responsible for 3 Audio Sources, one for each applicable Audio area (Music, Ambient Sound, and Sound Effects). Additionally it contains the Lists that contains references to the Audio files for each section. 

To add new sound files, you simply drag the file to their respective lists (depending on which audio source the file would use).

The Audio Manager is designed to handle all Audio of the game. Once the audio clip is uploaded to the correct List then there are two scripts to assist in playing a sound (mainly tailored for menu interation). Play Audio One Shot, and Play Sound on Hover.

Once these scripts are added to the applicable UI Button etc, you can simply select the clip you want to play. 

This works because the Audio Manager updates a text file with a list of names for the clips. These other scripts read from that file and create the drop down for you.

# Level Manager

Responsible for loading levels, the transition effect, and keeping track of the previously loaded level name (could be used for a Back button implementation).
