cd dependencies\DeJS\
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild /m
cd ..\..\

copy dependencies\DeJS\src\js\*.js src\DatenMeisterWeb\js\lib\*.js
copy dependencies\DeJS\src\js\*.d.ts src\DatenMeisterWeb\js\lib\
copy dependencies\BootstrapGit\dist\js\*.js src\DatenMeisterWeb\js\lib\*.js
copy dependencies\BootstrapGit\dist\css\*.css src\DatenMeisterWeb\css\*.css
copy dependencies\BootstrapGit\dist\img\*.* src\DatenMeisterWeb\img\*.*

copy dependencies\BackboneJS\backbone-min.js src\DatenMeisterWeb\js\lib\
copy dependencies\BackboneJS\backbone.js src\DatenMeisterWeb\js\lib\

copy dependencies\UnderscoreJS\underscore-min.js src\DatenMeisterWeb\js\lib\
copy dependencies\UnderscoreJS\underscore.js src\DatenMeisterWeb\js\lib\

copy dependencies\DefinitelyTyped\underscore\*.d.ts src\DatenMeisterWeb\js\lib\
REM copy dependencies\DefinitelyTyped\backbone\*.d.ts src\DatenMeisterWeb\js\lib\
copy dependencies\DefinitelyTyped\jquery\*.d.ts src\DatenMeisterWeb\js\lib\

copy dependencies\BurnSystems.WebServer\src\BurnSystems.WebServer\Resources\Require\*.js src\DatenMeisterWeb\js\lib\*.js
copy dependencies\BurnSystems.WebServer\src\BurnSystems.WebServer\Resources\Require\*.ts src\DatenMeisterWeb\js\lib\*.ts
copy dependencies\BurnSystems.WebServer\src\BurnSystems.WebServer\Resources\JQuery\*.js src\DatenMeisterWeb\js\lib\*.js

cd src
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild /m
cd ..
