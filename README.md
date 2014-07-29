Fire-SMSTrigger
===============

Applicaiton files for georpocessing near real time fire data. 

This application glues together several components
a) FrontlineSMS http triggers
b) Esri Arcpy for geoprocessing point events on the server
c) .Net application (C#) to manage the logic of taking http triggers and processing events.

This was created in 2012 as a sample application and could probably use some updates. 


Assumes you have a database of Fire locations (or some other point database) in postgres. Ideally the FireETL is running and cooking near real time MODIS derived Fire locations: https://github.com/SERVIR/ReferenceNode_ETL/tree/master/sources/fire

The outline is as follows:
==========================
a.	Scheduled Python job pulls latest data from the FTP from “/allData/1/MOD14T/Recent/,/allData/1/MYD14T/Recent/",

b.	As part of this the associate semi-structured MET file is also downloaded and parsed

c.	The job pulls any files it is missing based on comparing filenames in the database table to filenames in the FTP server for the last 90 days (or other defined time period). 

d.	If no new data is available the job stops here. 

e.	The txt data is converted to a shapefile based on the latitude and longitude field and geoprocessed against the GAUL0,1,2 Admin boundaries to obtain a locational attribute

f.	The Fire table is appended with the new data

g.	The map services refresh 

Accessing Services
==================
The Rapid Fire data is available as both WMS-T images service and ArcGIS REST Map and Identify requests.  Using the standard out-of-the-box functionality of ArcGIS Server and standard OGC map services there is already a high degree of data interactivity. 

Getting Latest Fire Date
========================
The date range for the rapid fire data is updated as soon as new data enters the database. When the ArcGIS Server map service refreshes itself it updates the max time of the range. This value can be obtained by querying the map service REST endpoint (similar to a getcapabilities request) and parsing the output json with a simple javascript function:
function getTimeExtent() {
	   dojo.xhrPost({
           url: "http://SERVER IP/ArcGIS/rest/services/GLOBAL RAPID FIRE/MapServer?f=json",
           contentType: "text/plain;charset=utf-8",
           handleAs: "json",
           load: completeGetTimeExtent,
           error: onError
	});
}

function completeGetTimeExtent(items) {
	min_date = epochToDateString(items.timeInfo.timeExtent[0]);
	max_date = epochToDateString(items.timeInfo.timeExtent[1]);
	intialdate = writeTimes(mindate, max_date);
	addNewWMSLayer(intial_date);
}

Because ArcGIS uses epoch dates a simple function is required to reverse parse the dates into a readable date string:
function epochToDateString(epochSeconds) {
	var d = new Date(epochSeconds);
	var t = d.toGMTString();
	var hours = d.getUTCHours();
	var year = d.getUTCFullYear();
	var month = d.getUTCMonth() + 1;
	var day = d.getUTCDate();
	var _time = month + "-" + day + "-" + year + " " + hours + ":00:00";
	return _time;
}


