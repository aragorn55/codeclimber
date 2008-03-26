///////////////////////////////////////////
// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.
//////////////////////////////////////////



// JScript source code

//contains calls to silverlight.js, example below loads Page.xaml
function createSilverlight()
{
	Silverlight.createObjectEx({
		source: "Page.xaml",
		parentElement: document.getElementById("SilverlightControlHost"),
		id: "SilverlightControl",
		properties: {
			width: "100%",
			height: "100%",
			version: "1.1",
			enableHtmlAccess: "true"
		},
		events: {}
	});
	   
	// Give the keyboard focus to the Silverlight control by default
    document.body.onload = function() {
      var silverlightControl = document.getElementById('SilverlightControl');
      if (silverlightControl)
      {
        silverlightControl.focus();
      }
    }
    
    window.onresize = function(){
        resize();
    }
}

function resize()
{
    var host = document.getElementById('SilverlightControlHost');
    if(!document.all)
        host.style.height = (document.documentElement.clientHeight - 20) +"px";
}


function ZoomIn()
{
    var silverlightControl = document.getElementById('SilverlightControl');
    silverlightControl.Content.basic.ZoomIn();
}

function ZoomOut()
{
    var silverlightControl = document.getElementById('SilverlightControl');
    silverlightControl.Content.basic.ZoomOut();
}