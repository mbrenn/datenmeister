CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser build_burnsystems_webserver build_burnsystems_flexbg build_dejs \
	bin/DatenMeister.dll bin/DatenMeister.Tests.dll bin/DatenmeisterServer.exe build_web

.PHONY: build_burnsystems
build_burnsystems:
	make -C packages/burnsystems

.PHONY: build_burnsystems_parser
build_burnsystems_parser:
	make -C packages/burnsystems.parser

.PHONY: build_burnsystems_webserver
build_burnsystems_webserver:
	make -C packages/burnsystems.webserver

.PHONY: build_burnsystems_flexbg
build_burnsystems_flexbg:
	make -C packages/burnsystems.flexbg

.PHONY: build_dejs
build_dejs:
	make -C packages/dejs

copy_packagefiles:
	mkdir -p packages/bin
	cp -r packages/burnsystems.flexbg/bin/* packages/bin/
	cp packages/burnsystems.webserver/bin/*.dll packages/bin/
	cp packages/burnsystems.webserver/bin/*.mdb packages/bin/
	cp packages/burnsystems.parser/bin/* packages/bin/
	cp packages/burnsystems/bin/* packages/bin/

bin/DatenMeister.dll: copy_packagefiles
	xbuild src/DatenMeister/DatenMeister.csproj
	mkdir -p bin
	cp src/DatenMeister/bin/Debug/* bin/

bin/DatenMeister.Tests.dll: copy_packagefiles
	xbuild src/DatenMeister.Tests/DatenMeister.Tests.csproj
	mkdir -p bin
	cp -r src/DatenMeister.Tests/bin/Debug/* bin/

bin/DatenmeisterServer.exe: copy_packagefiles build_web
	xbuild src/DatenmeisterServer/DatenmeisterServer.csproj
	mkdir -p bin
	cp -r src/DatenmeisterServer/bin/Debug/* bin/

.PHONY: get_external_packages
get_external_packages: 
	./get-external-packages.sh

.PHONY: copy_typescript_definitions
copy_typescript_definitions: get_external_packages
	mkdir -p bin/web/js/dejs/
	mkdir -p bin/web/js/requirejs/
	mkdir -p bin/web/js/underscore/
	mkdir -p bin/web/js/jquery/
	mkdir -p bin/web/js/backbone/
	mkdir -p bin/web/js/datenmeister/
	cp ext-packages/DefinitelyTyped/underscore/*.d.ts bin/web/js/underscore/
	cp ext-packages/DefinitelyTyped/jquery/*.d.ts bin/web/js/jquery/
	cp ext-packages/DefinitelyTyped/requirejs/*.d.ts bin/web/js/requirejs/
	cp ext-packages/DefinitelyTyped/backbone/*.d.ts bin/web/js/backbone/
	cp -r packages/dejs/bin/js/dejs/* bin/web/js/dejs/

bin/web/js/datenmeister/init.js: src/DatenMeisterWeb/js/datenmeister/init.ts \
	bin/web/js/datenmeister/datenmeister.js

bin/web/js/datenmeister/datenmeister.js: src/DatenMeisterWeb/js/datenmeister/datenmeister.ts \
	bin/web/js/datenmeister/datenmeister.datatable.js \
	bin/web/js/datenmeister/datenmeister.navigation.js \
	bin/web/js/datenmeister/datenmeister.objects.js \
	bin/web/js/datenmeister/datenmeister.serverapi.js \
	bin/web/js/datenmeister/datenmeister.views.js

bin/web/js/datenmeister/datenmeister.datatable.js: src/DatenMeisterWeb/js/datenmeister/datenmeister.datatable.ts \
	bin/web/js/datenmeister/datenmeister.objects.js \
	bin/web/js/datenmeister/datenmeister.serverapi.js

bin/web/js/datenmeister/datenmeister.navigation.js: src/DatenMeisterWeb/js/datenmeister/datenmeister.navigation.ts
bin/web/js/datenmeister/datenmeister.objects.js: src/DatenMeisterWeb/js/datenmeister/datenmeister.objects.ts
bin/web/js/datenmeister/datenmeister.serverapi.js: src/DatenMeisterWeb/js/datenmeister/datenmeister.serverapi.ts \
	bin/web/js/datenmeister/datenmeister.navigation.js

bin/web/js/datenmeister/datenmeister.views.js: src/DatenMeisterWeb/js/datenmeister/datenmeister.views.ts \
	bin/web/js/datenmeister/datenmeister.dataform.js
	
bin/web/js/datenmeister/datenmeister.fieldinfo.js: src/DatenMeisterWeb/js/datenmeister/datenmeister.fieldinfo.ts \
	bin/web/js/datenmeister/datenmeister.objects.js
	

.PHONY: compile_typescript
compile_typescript: bin/web/js/datenmeister/init.js \
	bin/web/js/datenmeister/datenmeister.js \
	bin/web/js/datenmeister/datenmeister.dataform.js \
	bin/web/js/datenmeister/datenmeister.datatable.js \
	bin/web/js/datenmeister/datenmeister.fieldinfo.js \
	bin/web/js/datenmeister/datenmeister.navigation.js \
	bin/web/js/datenmeister/datenmeister.objects.js \
	bin/web/js/datenmeister/datenmeister.serverapi.js \
	bin/web/js/datenmeister/datenmeister.views.js

.PHONY: build_web
build_web: copy_typescript_definitions compile_typescript
	mkdir -p bin/web/
	mkdir -p bin/web/js/bootstrap/
	mkdir -p bin/web/css
	mkdir -p bin/web/img
	mkdir -p bin/web/fonts
	mkdir -p src/DatenMeisterWeb/bin/Debug/

	cp -r src/DatenMeisterWeb/* bin/web/

	cp ext-packages/bootstrap/dist/js/*.js bin/web/js/bootstrap/
	cp ext-packages/bootstrap/dist/css/*.css bin/web/css/
	cp ext-packages/bootstrap/dist/fonts/* bin/web/fonts/

	cp ext-packages/backbone/backbone-min.js bin/web/js/backbone/
	cp ext-packages/backbone/backbone.js bin/web/js/backbone/

	cp ext-packages/underscore/underscore-min.js bin/web/js/underscore/
	cp ext-packages/underscore/underscore.js bin/web/js/underscore/

	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/Require/*.js bin/web/js/requirejs/
	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/Require/*.ts bin/web/js/requirejs/
	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/JQuery/*.js bin/web/js/jquery/

	cp -r bin/web src/DatenMeisterWeb/bin/Debug/

# Rule to transfer Typescript files to JavaScript files
bin/web/js/datenmeister/%.js : src/DatenMeisterWeb/js/datenmeister/%.ts 
	mkdir -p bin/web/js/datenmeister
	cp $< bin/web/js/datenmeister
	tsc bin/web/js/datenmeister/$(<F) --module amd --declaration --sourcemap

.PHONY: clean
clean:
	make clean -C packages/burnsystems
	make clean -C packages/burnsystems.parser
	make clean -C packages/burnsystems.webserver
	make clean -C packages/burnsystems.flexbg
	make clean -C packages/dejs
	rm -rf packages/bin
	rm -rf bin

.PHONY: clean_web
clean_web:
	rm -rf bin/web

.PHONY: clean-all
clean-all:
	rm -rf ext-packages/


.PHONY: client-run
client-run:
	firefox http://127.0.0.1:8080/ &
	xsp2 --root $(abspath bin/web/) 


.PHONY: server-run
server-run: 
	cd bin; mono DatenmeisterServer.exe

