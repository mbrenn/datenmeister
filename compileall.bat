cd dependencies\DeJS\
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild /m
cd ..\..\

copy dependencies\DeJS\src\js\*.js src\DatenMeisterWeb\js\lib\*.js
copy dependencies\DeJS\src\js\*.d.ts src\DatenMeisterWeb\js\lib\
copy dependencies\Bootstrap\js\*.js src\DatenMeisterWeb\js\lib\*.js
copy dependencies\Bootstrap\css\*.css src\DatenMeisterWeb\css\*.css
copy dependencies\Bootstrap\img\*.* src\DatenMeisterWeb\img\*.*
copy dependencies\BurnSystems.WebServer\src\BurnSystems.WebServer\Resources\Require\*.js src\DatenMeisterWeb\js\lib\*.js
copy dependencies\BurnSystems.WebServer\src\BurnSystems.WebServer\Resources\JQuery\*.js src\DatenMeisterWeb\js\lib\*.js

cd src
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild /m
cd ..
