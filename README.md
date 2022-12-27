# ** PROJECT IS INACTIVE **
Liquid Interactive does not have any plans to continune to maintain this project at this time. We encourage the usage and exploration of existing and future Github Forks of this project to continue its development moving forward. 

Need a custom Sitecore module and/or Website to be professionally architected/designed/developed? Drop us your request at [hello@liquidint.com](mailto:hello@liquidint.com)!

# Sitecore Map Plugin

This is a Sitecore 9.2+ supported module which installs a Bing Map rendering to be used throughout your website. Easily manage pushpin locations directly through the Content Editor. There or no third-party requirements necessary &mdash; install and run.


# Installation

Download the latest release package for your Sitecore version and upload it via the Installation Wizard which can be found under the Development Tools option while viewing the Sitecore Desktop. 

## Setting up your first map

The rendering to be used on your layout can be found in `/sitecore/layout/Renderings/Modules/MapPlugin/Map Plugin` 

and its datasource template is located in 
`/sitecore/templates/Modules/MapPlugin/Map Plugin`

Create / add your Bing Maps Key by going to `/sitecore/system/Settings/Map Plugin`

Everyone's environment is different and we have considered that in the architecture of this module. The datasource can live where ever you see fit &mdash; on the item context itself; or located somewhere as a reusable component. 

The `Map Plugin` data template consumes PushPin locations using another template called 	Map Plugin Pin which is located in `/sitecore/templates/Modules/MapPlugin/Map Plugin Pin`

This template further defines the properties of each pushpin location that will be included on your map. 
**Each `Map Plugin Pin` item must get added to the corresponding Map Plugin's field `Map Plugin Pins`*

## Customizing your first map

The map plugin frontend view can be customized entirely by modifying `/Views/MapPlugin/Index.cshtml` in your webroot. The CSS for this project has been namespaced using the class name*.map-plugin*

Since the CSS is namespaced accordingly, overriding the look and feel is as easy as loading your custom stylesheet after ours.  

# Contributing

Help keep this project healthy by contributing bug fixes and new features. Below are a list of pull request guidelines and steps to get started developing locally.  

## Install Sitecore.

We will name the obvious - in order to contribute you must have a Sitecore installation to test with. This way you'll be able to test your code changes.

## Clone the repo.

After pulling down this repo locally and opening the solution be sure to "Restore NuGet Packages." In doing so, the project will be able to use the required Sitecore DLL references properly. Clean and Rebuild. 

## Serialize items from filesystem.

We've included Sitecore items that help manage the module from the Content Editor. In order to introduce these items to your environment you must update your database with serialized items from the filesystem.

**Configure Serialization**
In order to maintain the same data format across developers ensure "YAML" is configured in `\App_Config\Sitecore\CMS.Core\Sitecore.Serialization.config`on `Line 37`:

`<setting name="Serialization.SerializationType" value="YAML" />`

**Get Latest**
Copy and paste the serialized items from `src\Feature\MapPlugin\code\App_Data\serialization` and place them in your webroot. Navigate to the Content Editor and click the "Developer" tab in the ribbon.  *If you do not see this then right-click any other tab and enable the Developer option*. Click "Update database."

**Sync updates**
Go to the item or parent item (tree) and click "Update item," or "Update tree," respectively. This will move items to your `serialization` folder. Drag these items into the repo to be committed. 

We chose to manage items in this way to prevent the use of additional third-party plugins or modules.

## Introducing new items or files.

In order to maintain usability across various environments all Sitecore items introduced or modified should *only* be defined in the following locations: 
|                |                                                     |
|----------------|-----------------------------------------------------|
|Templates       |`/sitecore/templates/Modules/MapPlugin`              |     
|Renderings      |`/sitecore/layout/Renderings/Modules/MapPlugin`      | 
|Layouts         |`/sitecore/layout/Layouts/Modules/Map Plugin`        |
|CSS & JS        |`..\sitecore modules\Liquid\MapPlugin\`              |
|Views           |`..\Views\MapPlugin\`                                |
