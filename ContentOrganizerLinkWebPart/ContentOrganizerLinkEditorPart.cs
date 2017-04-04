using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Schaeflein.Community.ContentOrganizerLink
{
	class ContentOrganizerLinkEditorPart : System.Web.UI.WebControls.WebParts.EditorPart
	{
		protected TextBox txtImageLink;
		protected HyperLink lnkTestLink;
		protected TextBox txtLinkText;

		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			this.Title = "Image Properties";

			txtImageLink = new TextBox() { ID = "imageUrl" };
			txtImageLink.CssClass = "UserInput";
			txtImageLink.Width = 176;

			Panel sectionHead = new Panel { CssClass = "UserSectionHead" };
			sectionHead.Controls.Add(new Label { Text = "Image Url", AssociatedControlID = txtImageLink.ID });
			Controls.Add(sectionHead);

			Panel sectionBody = new Panel { CssClass = "UserSectionBody" };
			sectionBody.Controls.Add(new Label { Text = "To link to an image file, type a URL or path." });

			Panel testLinkPanel = new Panel();
			string clickScript = String.Format("Schaeflein.Community.ContentOrganizerLink.TestLink('{0}');return false;",
																				 txtImageLink.ClientID);
			lnkTestLink = new HyperLink { ID = "testLink", NavigateUrl = "#", Text = "Test Link" };
			lnkTestLink.Attributes.Add("onclick", clickScript);
			testLinkPanel.Controls.Add(lnkTestLink);
			sectionBody.Controls.Add(testLinkPanel);

			Panel controlGroup = new Panel { CssClass = "UserControlGroup" };
			controlGroup.Controls.Add(txtImageLink);
			sectionBody.Controls.Add(controlGroup);
			Controls.Add(sectionBody);

			txtLinkText = new TextBox() { ID = "linkText" };
			txtLinkText.CssClass = "UserInput";
			txtLinkText.Width = 176;

			sectionHead = new Panel { CssClass = "UserSectionHead" };
			sectionHead.Controls.Add(new Label { Text = "Link Text", AssociatedControlID = txtLinkText.ID });
			sectionHead.Controls.Add(sectionHead);

			sectionBody = new Panel { CssClass = "UserSectionBody" };
			controlGroup = new Panel { CssClass = "UserControlGroup" };
			controlGroup.Controls.Add(txtLinkText);
			sectionBody.Controls.Add(controlGroup);
			Controls.Add(sectionBody);
			this.ChildControlsCreated = true;
		}

		protected override void RenderContents(HtmlTextWriter writer)
		{
			base.RenderContents(writer);

			string initScript = String.Format("MaventionExecuteSodFunction(function() {{ Schaeflein.Community.ContentOrganizerLink.init('{0}','{1}'); }},'schaeflein-community-contentorganizerlink.js');",
														lnkTestLink.ClientID, txtImageLink.ClientID);

			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.WriteLine(initScript);

			writer.RenderEndTag(); // script
		}

		public override void SyncChanges()
		{
			EnsureChildControls();
			ContentOrganizerLinkWebPart webPart = this.WebPartToEdit as ContentOrganizerLinkWebPart;
			if (webPart!=null)
			{
				txtImageLink.Text = webPart.ImageLink;
				txtLinkText.Text = webPart.LinkText;
			}
		}

		public override bool ApplyChanges()
		{
			EnsureChildControls();
			ContentOrganizerLinkWebPart webPart = this.WebPartToEdit as ContentOrganizerLinkWebPart;
			if (webPart != null)
			{
				webPart.ImageLink = txtImageLink.Text;
				webPart.LinkText = txtLinkText.Text;
			}
			return true;
		}
	}
}
