using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace Schaeflein.Community.ContentOrganizerLink.Features.SiteComponents
{
	/// <summary>
	/// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
	/// </summary>
	/// <remarks>
	/// The GUID attached to this class may be used during packaging and should not be modified.
	/// </remarks>

	[Guid("f7e2176d-0fbc-4a20-af6d-369af939d205")]
	public class SiteComponentsEventReceiver : SPFeatureReceiver
	{
		// Uncomment the method below to handle the event raised after a feature has been activated.

		//public override void FeatureActivated(SPFeatureReceiverProperties properties)
		//{
		//}


		// Uncomment the method below to handle the event raised before a feature is deactivated.

		public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
		{
			SPSite site = properties.Feature.Parent as SPSite;
			SPWeb web = site.RootWeb;

			string[] filesToDelete = {
										"_catalogs/wp/ContentOrganizerLink.webpart",
										"Style Library/Schaeflein/js/ContentOrganizerLink.js",
										"Style Library/Schaeflein/images/logo16.png",
										"Style Library/Mavention/js/mv-sod.js",
								 };
			DeleteFeatureFiles(web, filesToDelete);

			string[] foldersToDelete = {
										"Style Library/Schaeflein",
										"Style Library/Mavention"
								};
		}


		// Uncomment the method below to handle the event raised after a feature has been installed.

		//public override void FeatureInstalled(SPFeatureReceiverProperties properties)
		//{
		//}


		// Uncomment the method below to handle the event raised before a feature is uninstalled.

		//public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
		//{
		//}

		// Uncomment the method below to handle the event raised when a feature is upgrading.

		//public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
		//{
		//}


		public void DeleteFeatureFiles(SPWeb currentWeb, string[] filesToDelete)
		{
			SPFile fileToDelete = null;

			if (currentWeb != null)
			{
				foreach (var file in filesToDelete)
				{
					fileToDelete = null;
					try
					{
						fileToDelete = currentWeb.GetFile(file);
					}
					catch
					{
						fileToDelete = null;
					}

					if (fileToDelete != null && fileToDelete.Exists)
					{
						try
						{
							if (fileToDelete.CheckOutType != SPFile.SPCheckOutType.None)
							{
								fileToDelete.CheckIn("Forced Check In by branding feature.");
							}

							try { fileToDelete.Versions.RecycleAll(); }
							catch (Exception aEx)
							{
								// log error on recycle 
							}

							try { fileToDelete.Recycle(); }
							catch (Exception bEx)
							{
								// log error on delete
							}
						}
						catch (Exception ex)
						{
							// log unexpected error
						}
					}
				}
			}
		}

		public void DeleteFeatureFolders(SPWeb currentWeb, string[] foldersToDelete)
		{
			SPFolder folderToDelete = null;

			if (currentWeb != null)
			{
				foreach (var folder in foldersToDelete)
				{
					folderToDelete = null;
					try
					{
						folderToDelete = currentWeb.GetFolder(folder);
					}
					catch
					{
						folderToDelete = null;
					}

					if (folderToDelete != null)
					{
						try { folderToDelete.Recycle(); }
						catch (Exception bEx)
						{
							// log error on delete
						}
					}
				}
			}
		}
	}
}
