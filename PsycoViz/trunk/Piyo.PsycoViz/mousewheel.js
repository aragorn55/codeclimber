﻿///////////////////////////////////////////
// Copyright © Piyosailing.com 2008.
// This source is subject to the Microsoft Public License (Ms-PL).
// See the included License.txt file.
// All other rights reserved.
//////////////////////////////////////////


function handle(delta) {
	if (delta > 0)
		ZoomIn();
	else
		ZoomOut();
}

function wheel(event){
	var delta = 0;
	if (!event) event = window.event;
	if (event.wheelDelta) {
		delta = event.wheelDelta/120; 
		if (window.opera) delta = -delta;
	} else if (event.detail) {
		delta = -event.detail/3;
	}
	if (delta)
		handle(delta);
        if (event.preventDefault)
                event.preventDefault();
        event.returnValue = false;
}

/* Initialization code. */
if (window.addEventListener)
	window.addEventListener('DOMMouseScroll', wheel, false);
window.onmousewheel = document.onmousewheel = wheel;