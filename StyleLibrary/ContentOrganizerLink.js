/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\SP.js" />
/// <reference path="C:\Program Files\Common Files\Microsoft Shared\Web Server Extensions\14\TEMPLATE\LAYOUTS\1033\itp.js" />

"use strict";

var Schaeflein = window.Schaeflein || {};

Schaeflein.Community = function () {
	var contentOrganizerLink = function () {
		var linkControlId = null,

		launchUpload = function (pageUrl, title) {
			var options = {
				url: pageUrl,
				title: title,
				allowMaximize: false,
				showClose: true,
				dialogReturnValueCallback: refreshPageEventDialogCallback
			};
			var did = SP.UI.ModalDialog.showModalDialog(options);

			function refreshPageEventDialogCallback(dialogResult, returnValue) {
				SP.UI.ModalDialog.RefreshPage(SP.UI.DialogResult.OK);
			}
		},

		testLink = function () {
			var errorMessage = "Image Link is not a valid URL, UNC or local path. Please check the spelling and syntax, and then type a valid path.";
			var validTypes = 15;

			var url = document.getElementById(this.linkControlId).value;

			MsoFrameworkToolpartPreview(url, errorMessage, validTypes);
		},

		init = function (linkId, textboxId) {
			this.linkControlId = textboxId;

			var testLink = document.getElementById(linkId);
			if (testLink) {
				Sys.UI.DomEvent.addHandler(testLink, 'click', function(e) {
					MaventionExecuteSodFunction(function() { Schaeflein.Community.ContentOrganizerLink.TestLink(); }, 'schaeflein-community-contentorganizerlink.js');
					e.preventDefault();    
				});
			}
		}

		return {
			LaunchUpload: launchUpload,
			TestLink: testLink,
			init: init
		}

	}();

	return {
		ContentOrganizerLink: contentOrganizerLink
	}
}();

SP.SOD.notifyScriptLoadedAndExecuteWaitingJobs("schaeflein-community-contentorganizerlink.js");