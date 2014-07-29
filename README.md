Fire-SMSTrigger
===============

Applicaiton files for georpocessing near real time fire data. 

This application glues together several components
a) FrontlineSMS http triggers
b) Esri Arcpy for geoprocessing point events on the server
c) .Net application (C#) to manage the logic of taking http triggers and processing events.

This was created in 2012 as a sample application and could probably use some updates. 


Assumes you have a database of Fire locations (or some other point database) in postgres. Ideally the FireETL is running and cooking near real time MODIS derived Fire locations: https://github.com/SERVIR/ReferenceNode_ETL/tree/master/sources/fire 
