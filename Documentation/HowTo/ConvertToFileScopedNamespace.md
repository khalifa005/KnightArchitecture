#### VS 2022 - Convert to file-scoped namespace in all files

   After you have configured the .editorconfig, you can configure a 'Code Cleanup' setting to automatically convert all files to use file-scoped namespace. Go to Tools -> Options -> Text Editor -> Code Cleanup -> Configure Code Cleanup. Then add the 'Apply namespace preferences'. Then go to Analyze -> Code Cleanup (or just search for 'Code cleanup') and run the Code Cleanup to automatically change the namespaces to file-scoped.


   Best answer in my opinion is here: https://www.ilkayilknur.com/how-to-convert-block-scoped-namespacees-to-file-scoped-namespaces

It says that you can change the code-style preference (and enable the display of the option to apply this preference in a document / project / solution) by going to Tools => Options => Text Editor => C#=> Code Style and then changing the related preference.

![image](https://github.com/user-attachments/assets/fd3e99bd-facc-4e49-87d5-cd83f3ce5a0c)


https://blog.joaograssi.com/series/authorization-in-asp.net-core/ 




---
## Running sql server on docker
Ensure SQL Server Container is Running:

Use docker ps to ensure your sqlserver container is up and running.
Find the IP Address or Hostname:

Use the hostname localhost or the machine's IP address if accessing from the same system.
The SQL Server is mapped to port 1434 on your host machine as per the ports section in your Docker Compose file.

Connect to the SQL Server:

Server Type: Select Database Engine.
Server Name: Enter localhost,1434. The 1434 is the port your container is exposing. //we can use container name-ip
Authentication: Choose SQL Server Authentication.
Login: Enter sa (the default SQL Server system administrator).
Password: Enter the password StrongP@ssw0rd1. //example password

![image](https://github.com/user-attachments/assets/3bb526b1-d402-42ac-9ac6-4c8ca0d59f56)

https://chatgpt.com/c/675c1f14-670c-800a-9ed7-11d1ef90a203
