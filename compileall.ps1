# Defines the configuration

$targetJS = "src/DatenMeisterWeb/js"
$targetWeb = "src/DatenMeisterWeb"

mkdir -Force $targetJS

#
# Gets information from external packages
# 

mkdir -Force ext-packages 

cd ext-packages

	#
	# Request information from git
	#

	# Loading DefinitelyTyped
	if (!(Test-Path -path DefinitelyTyped))
	{
		git clone https://github.com/borisyankov/DefinitelyTyped
	}


	cd DefinitelyTyped
	git checkout ea3efa8377050a0464a21a88bd3251c6f34938b8
	cd ..

	# Loading bootstrap
	if (!(Test-Path -path  bootstrap))
	{ 
		git clone https://github.com/twbs/bootstrap.git
	}
	cd bootstrap
		git checkout v3.0.3
	cd ..

	# Loading underscore
	if (!(Test-Path -path underscore))
	{
		git clone https://github.com/jashkenas/underscore.git
	}

	cd underscore
		git checkout 1.5.2
	cd ..

	# Loading backbone
	if (!(Test-Path -path backbone))
	{
		git clone https://github.com/jashkenas/backbone.git
	}

	cd backbone
		git checkout 1.1.0
	cd ..

cd ..

#
# Copy all the typescript definition files to bin
#

mkdir -Force $targetJS/dejs/ 
mkdir -Force $targetJS/requirejs/ 
mkdir -Force $targetJS/underscore/ 
mkdir -Force $targetJS/jquery/ 
mkdir -Force $targetJS/backbone/ 
mkdir -Force $targetJS/datenmeister/ 
cp -Recurse -Force ext-packages/DefinitelyTyped/underscore/*.d.ts $targetJS/underscore/
cp -Recurse -Force ext-packages/DefinitelyTyped/jquery/*.d.ts $targetJS/jquery/
cp -Recurse -Force ext-packages/DefinitelyTyped/requirejs/*.d.ts $targetJS/requirejs/
cp -Recurse -Force ext-packages/DefinitelyTyped/backbone/*.d.ts $targetJS/backbone/
cp -Recurse -Force ../dejs/src/js/dejs/* $targetJS/dejs/

#
# Copies the rest
#

mkdir -Force $targetJS/
mkdir -Force $targetJS/bootstrap/
mkdir -Force $targetJS/backbone/
mkdir -Force $targetJS/underscore/
mkdir -Force $targetJS/requirejs/
mkdir -Force $targetJS/jquery/
mkdir -Force $targetWeb/css
mkdir -Force $targetWeb/img
mkdir -Force $targetWeb/fonts


cp -Recurse -Force ext-packages/bootstrap/dist/js/*.js $targetJS/bootstrap/
cp -Recurse -Force ext-packages/bootstrap/dist/css/*.css $targetWeb/css/
cp -Recurse -Force ext-packages/bootstrap/dist/fonts/* $targetWeb/fonts/

cp -Recurse -Force ext-packages/backbone/backbone-min.js $targetJS/backbone/
cp -Recurse -Force ext-packages/backbone/backbone.js $targetJS/backbone/

cp -Recurse -Force ext-packages/underscore/underscore-min.js $targetJS/underscore/
cp -Recurse -Force ext-packages/underscore/underscore.js $targetJS/underscore/

cp ../burnsystems.webserver/src/BurnSystems.WebServer/Resources/Require/*.js $targetJS/requirejs/
cp ../burnsystems.webserver/src/BurnSystems.WebServer/Resources/Require/*.ts $targetJS/requirejs/
cp ../burnsystems.webserver/src/BurnSystems.WebServer/Resources/JQuery/*.js $targetJS/jquery/