copy dependencies\DeJS\src\js\*.js web\js\lib\*.js
copy dependencies\Bootstrap\js\*.js web\js\lib\*.js
copy dependencies\Bootstrap\css\*.css web\css\*.css
copy dependencies\Bootstrap\img\*.* web\img\*.*
copy dependencies\BurnSystems.WebServer\src\BurnSystems.WebServer\Resources\Require\*.js web\js\lib\*.js
copy dependencies\BurnSystems.WebServer\src\BurnSystems.WebServer\Resources\JQuery\*.js web\js\lib\*.js

cd src
msbuild
cd ..
