#### VS 2022 - Convert to file-scoped namespace in all files

   After you have configured the .editorconfig, you can configure a 'Code Cleanup' setting to automatically convert all files to use file-scoped namespace. Go to Tools -> Options -> Text Editor -> Code Cleanup -> Configure Code Cleanup. Then add the 'Apply namespace preferences'. Then go to Analyze -> Code Cleanup (or just search for 'Code cleanup') and run the Code Cleanup to automatically change the namespaces to file-scoped.


   Best answer in my opinion is here: https://www.ilkayilknur.com/how-to-convert-block-scoped-namespacees-to-file-scoped-namespaces

It says that you can change the code-style preference (and enable the display of the option to apply this preference in a document / project / solution) by going to Tools => Options => Text Editor => C#=> Code Style and then changing the related preference.

![image](https://github.com/user-attachments/assets/fd3e99bd-facc-4e49-87d5-cd83f3ce5a0c)


https://blog.joaograssi.com/series/authorization-in-asp.net-core/ 



---
