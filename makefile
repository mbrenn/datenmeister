CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser build_burnsystems_webserver build_burnsystems_flexbg build_dejs datenmeister \
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

.PHONY: datenmeister
prepare_datenmeister: 
	./get-external-packages.sh

bin/DatenMeister.dll: prepare_datenmeister copy_packagefiles
	xbuild src/DatenMeister/DatenMeister.csproj
	mkdir -p bin
	cp src/DatenMeister/bin/Debug/* bin/

bin/DatenMeister.Tests.dll: prepare_datenmeister copy_packagefiles
	xbuild src/DatenMeister.Tests/DatenMeister.Tests.csproj
	mkdir -p bin
	cp -r src/DatenMeister.Tests/bin/Debug/* bin/

bin/DatenmeisterServer.exe: prepare_datenmeister copy_packagefiles
	xbuild src/DatenmeisterServer/DatenmeisterServer.csproj
	mkdir -p bin
	cp -r src/DatenmeisterServer/bin/Debug/* bin/

.PHONY:
build_web: datenmeister
	mkdir -p bin/web/
	mkdir -p bin/web/js/lib/
	mkdir -p bin/web/css
	mkdir -p bin/web/img
	mkdir -p bin/web/fonts

	cp -r src/DatenMeisterWeb/* bin/web/

	cp packages/dejs/src/js/*.js bin/web/js/lib/
	cp packages/dejs/src/js/*.d.ts bin/web/js/lib/
	cp packages/dejs/src/js/*.map bin/web/js/lib/

	cp ext-packages/bootstrap/dist/js/*.js bin/web/js/lib/
	cp ext-packages/bootstrap/dist/css/*.css bin/web/css/
	cp ext-packages/bootstrap/dist/fonts/* bin/web/fonts/

	cp ext-packages/backbone/backbone-min.js bin/web/js/lib/
	cp ext-packages/backbone/backbone.js bin/web/js/lib/

	cp ext-packages/underscore/underscore-min.js bin/web/js/lib/
	cp ext-packages/underscore/underscore.js bin/web/js/lib/

	cp ext-packages/DefinitelyTyped/underscore/*.d.ts bin/web/js/lib/
	cp ext-packages/DefinitelyTyped/jquery/*.d.ts bin/web/js/lib/

	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/Require/*.js bin/web/js/lib/
	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/Require/*.ts bin/web/js/lib/
	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/JQuery/*.js bin/web/js/lib/

	cp -r bin/web src/DatenMeisterWeb/bin/Debug/


.PHONY: clean
clean:
	make clean -C packages/burnsystems
	make clean -C packages/burnsystems.parser
	make clean -C packages/burnsystems.webserver
	make clean -C packages/burnsystems.flexbg
	make clean -C packages/dejs
	rm -rf packages/bin
	rm -rf bin

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

