#if !SANDBOX
using Microsoft.Office.RecordsManagement.RecordsRepository;
#endif

using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Schaeflein.Community.ContentOrganizerLink
{
	[ToolboxItemAttribute(false)]
	[Guid("a0831bcc-7336-4927-bb15-1a3dadd1332f")]
	public class ContentOrganizerLinkWebPart : System.Web.UI.WebControls.WebParts.WebPart, System.Web.UI.WebControls.WebParts.IWebEditable
	{
		[WebBrowsable(false)]
		[Personalizable(PersonalizationScope.Shared)]
		public string LinkText { get; set; }

		[WebBrowsable(false)]
		[Personalizable(PersonalizationScope.Shared)]
		public string ImageLink { get; set; }

#if SANDBOX
		[WebBrowsable(false)]
		[Personalizable(PersonalizationScope.Shared)]
		public string DropOffLibraryLink { get; set; }
#endif

		public ContentOrganizerLinkWebPart()
		{
			if (this.Title.Length == 0)
			{
				this.Title = "Content Organizer Link";
			}
			if (this.Description.Length == 0)
			{
				this.Description = "Renders a link to the Drop Off library in the current site, if configured. Created by Schaeflein Consulting. http://www.schaeflein.net/";
			}
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			string uploadUrl = String.Empty;

			if (System.AppDomain.CurrentDomain.FriendlyName.Contains("Sandbox"))
			{
				uploadUrl = "grrrrr";
			}
			else
			{
				SPWeb web = SPContext.Current.Web;
				EcmDocumentRoutingWeb ecmWeb = new EcmDocumentRoutingWeb(web);

				if (ecmWeb.IsRoutingEnabled)
				{
					SPList dropOffLibrary = ecmWeb.DropOffZone;

					if (dropOffLibrary != null)
					{
						uploadUrl = SPUtility.ConcatUrls(web.Url, dropOffLibrary.Forms[(int)Microsoft.SharePoint.PAGETYPE.PAGE_NEWFORM].Url);
					}
				}
			}

			if (!String.IsNullOrEmpty(uploadUrl))
			{


				string clickScript = String.Format("Schaeflein.Community.ContentOrganizerLink.LaunchUpload('{0}','{1}');return false;",
																					 uploadUrl,
																					 this.LinkText);


				HyperLink contentOrganizerNewForm = new HyperLink();
				contentOrganizerNewForm.ID = "contentOrganizerNewForm";
				contentOrganizerNewForm.NavigateUrl = "#";
				contentOrganizerNewForm.ImageUrl = "/_layouts/images/accesssetting.gif";
				contentOrganizerNewForm.Text = "This is the text";
				contentOrganizerNewForm.Attributes.Add("onclick", clickScript);

				Controls.Add(contentOrganizerNewForm);
			}
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.AddAttribute(HtmlTextWriterAttribute.Src, String.Format("{0}/Style Library/mavention/js/mv-sod.js", SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/')));
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.RenderEndTag(); // script         

			base.RenderContents(writer);

			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.WriteLine(String.Format("SP.SOD.registerSod('itp.js', '{0}/_layouts/{1}/itp.js');",
																																			SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/'),
																																			SPContext.Current.Web.Language));
			writer.WriteLine(String.Format("SP.SOD.registerSod('schaeflein-community-contentorganizerlink.js', '{0}/Style Library/Schaeflein/js/ContentOrganizerLink.js');",
																																					SPContext.Current.Site.ServerRelativeUrl.TrimEnd('/')));
			writer.WriteLine("SP.SOD.registerSodDep('schaeflein-community-contentorganizerlink.js','itp.js');");

			writer.RenderEndTag(); // script
		}

		EditorPartCollection IWebEditable.CreateEditorParts()
		{
			List<EditorPart> editors = new List<EditorPart>();
			ContentOrganizerLinkEditorPart editorPart = new ContentOrganizerLinkEditorPart();
			editorPart.ID = this.ID + "_editorPart";
			editors.Add(editorPart);
			return new EditorPartCollection(editors);
		}

		object IWebEditable.WebBrowsableObject
		{
			get { return this; }
		}
		//public override ContentOrganizerLinkEditorPart[] GetToolParts()
		//{
		//		WebPartToolPart webPartToolPart = new WebPartToolPart();
		//		webPartToolPart.Hide(WebPartToolPart.Properties.Direction);
		//		if ((base.WebPartManager == null || !base.WebPartManager.CustomizationMode) && !Utility.CheckForCustomToolpane(this.Page) && !base.WebPartManager.ForWebPartRender && SPWebPartManager.GetSPWebPartType(this) != WebPartType.OnlyForMe)
		//		{
		//				MessageToolPart messageToolPart = new MessageToolPart(WebPartPageResource.GetString("ImageToolPartCaption"), WebPartPageResource.GetString("ImageToolPartNoRights"));
		//				webPartToolPart.Expand(WebPartToolPart.Categories.Appearance);
		//				ToolPart[] toolPartArray = new ToolPart[] { messageToolPart, webPartToolPart };
		//				return toolPartArray;
		//		}
		//		else
		//		{
		//				ImageToolpart imageToolpart = new ImageToolpart();
		//				ToolPart[] toolPartArray1 = new ToolPart[] { imageToolpart, webPartToolPart };
		//				return toolPartArray1;
		//		}
		//}

		//protected override void RenderWebPart(HtmlTextWriter writer)
		//{
		//		if (!this.ShouldRenderDefaultPreview)
		//		{
		//				if (!this.IsConnected)
		//				{
		//						if (this._imageLink == null)
		//						{
		//								if (!base.DesignMode)
		//								{
		//										if (FrameworkClassesToolPart.ShouldCreateDefaultMessage(this))
		//										{
		//												writer.WriteLine(SPHttpUtility.NoEncode(FrameworkClassesToolPart.CreateDefaultMessage(this, "ImagePartDefaultMessage")));
		//										}
		//										return;
		//								}
		//								else
		//								{
		//										writer.WriteLine(SPHttpUtility.HtmlEncode(WebPartPageResource.GetString("ImagePartDefaultMessageDesignMode")));
		//										return;
		//								}
		//						}
		//				}
		//				else
		//				{
		//						if (!this._cellReadyError)
		//						{
		//								if (!this._cellReadyFired || this._imageLink == null)
		//								{
		//										object[] objArray = new object[] { this._providerPartTitle };
		//										writer.WriteLine(string.Concat("<p class=\"UserGeneric\">", SPHttpUtility.NoEncode(WebPartPageResource.GetString("ImagePartConnectedInitialMessage", objArray)), "</p>"));
		//										return;
		//								}
		//						}
		//						else
		//						{
		//								object[] objArray1 = new object[] { this._providerPartTitle };
		//								writer.WriteLine(string.Concat("<p class=\"UserGeneric\">", SPHttpUtility.NoEncode(WebPartPageResource.GetString("ImagePartCellReadyErrorMessage", objArray1)), "</p>"));
		//								return;
		//						}
		//				}
		//				writer.AddAttribute(HtmlTextWriterAttribute.Cellpadding, "0", false);
		//				writer.AddAttribute(HtmlTextWriterAttribute.Cellspacing, "0", false);
		//				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0", false);
		//				writer.AddStyleAttribute("width", "100%");
		//				if (base.Height.Length > 0)
		//				{
		//						writer.AddStyleAttribute("height", SPHttpUtility.HtmlEncode(base.Height));
		//				}
		//				writer.RenderBeginTag(HtmlTextWriterTag.Table);
		//				writer.RenderBeginTag(HtmlTextWriterTag.Tr);
		//				writer.AddAttribute(HtmlTextWriterAttribute.Align, ContentOrganizerLinkWebPart.HorizontalAlignmentStrings[(int)this.HorizontalAlignment], false);
		//				writer.AddAttribute(HtmlTextWriterAttribute.Valign, ContentOrganizerLinkWebPart.VerticalAlignmentStrings[(int)this.VerticalAlignment], false);
		//				if (this.BackgroundColor.Length > 0)
		//				{
		//						writer.AddAttribute(HtmlTextWriterAttribute.Style, string.Concat("background-color:", this.BackgroundColor), false);
		//				}
		//				writer.RenderBeginTag(HtmlTextWriterTag.Td);
		//				writer.AddAttribute(HtmlTextWriterAttribute.Border, "0", false);
		//				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ImageStorageKey, false);
		//				if (!this._inDesign)
		//				{
		//						string str = base.ReplaceTokens(this.ImageLink.Trim());
		//						if (Utility.BrowserIsNetscape(this.Context.Request.Browser))
		//						{
		//								if (str.Length <= 1 || str[0] != '\\' || str[1] != '\\')
		//								{
		//										if (str.Length > 2 && str[1] == ':' && str[2] == '\\')
		//										{
		//												str = string.Concat("file://", str);
		//										}
		//								}
		//								else
		//								{
		//										str = string.Concat("file:///", str);
		//								}
		//						}
		//						writer.AddAttribute("src", SPHttpUtility.HtmlUrlAttributeEncode(str), false);
		//				}
		//				else
		//				{
		//						writer.AddAttribute("src", SPHttpUtility.HtmlUrlAttributeEncode(this._imageLink), false);
		//				}
		//				string empty = string.Empty;
		//				if (this.AlternativeText.Trim().Length > 0)
		//				{
		//						empty = this.AlternativeText;
		//				}
		//				writer.AddAttribute(HtmlTextWriterAttribute.Alt, SPHttpUtility.HtmlEncode(empty), false);
		//				writer.RenderBeginTag(HtmlTextWriterTag.Img);
		//				writer.RenderEndTag();
		//				writer.RenderEndTag();
		//				writer.RenderEndTag();
		//				writer.RenderEndTag();
		//				return;
		//		}
		//		else
		//		{
		//				string[] strArrays = new string[] { "<img alt=\"", SPHttpUtility.HtmlEncode(WebPartPageResource.GetString("PreviewImageLiteral")), "\" src=\"", SPHttpUtility.NoEncode(Utility.MakeLayoutsRootServerRelative("images/ipvw.gif")), "\"/>" };
		//				writer.WriteLine(string.Concat(strArrays));
		//				return;
		//		}
		//}




		public enum ImageVerticalAlignment
		{
			/// <summary>Vertically align the image with the top of the frame.</summary>
			Top,
			/// <summary>Vertically align the image in the center of the frame.</summary>
			Middle,
			/// <summary>Vertically align the image with the bottom of the frame.</summary>
			Bottom
		}
		public enum ImageHorizontalAlignment
		{
			/// <summary>Left-align the image.</summary>
			Left,
			/// <summary>Center the image.</summary>
			Center,
			/// <summary>Right-align the image.</summary>
			Right
		}

	}
}

