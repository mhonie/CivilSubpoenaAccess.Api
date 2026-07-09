# CivilSubpoenaAccess.Api Installation  
  
(1) The EFiling database credentials are stored in environment variables.  

There's an install script that sets them on the host machine.  

(2) There's a script to create an event source called ** CivilSubpoenaAccess.Api** in Event Viewer.

Startup errors go to the Event Viewer. All other errors go to the Serilog logger. 
    
