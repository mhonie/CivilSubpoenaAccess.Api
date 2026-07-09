# CivilSubpoenaAccess.Api Release Pipeline  
  
(1) The release pipeline uses appSettings.Development.json to run unit tests, etc.  
  
(2) The environment variables (e.g., database credentials) are stored in the pipeline variables.   

(3) We have to disable SSL verification (because OIT injects self-signed certificates, etc.)  

So, there's a deployment scripts that sets **NODE_TLS_REJECT_UNAUTHORIZED** to 0.  
  
(4) The test script contains a reference to SkipCopyConfig. This is a flag to disable the post build event.  

Defining that flag requires you to manually update the csproj file, unfortunately.