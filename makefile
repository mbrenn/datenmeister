CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser build_burnsystems_webserver build_burnsystems_flexbg datenmeister \
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
	cp src/DatenMeister.Tests/bin/Debug/* bin/

bin/DatenmeisterServer.exe: prepare_datenmeister copy_packagefiles
	xbuild src/DatenmeisterServer/DatenmeisterServer.csproj
	mkdir -p bin
	cp src/DatenmeisterServer/bin/Debug/* bin/

.PHONY:
build_web: datenmeister
	mkdir -p bin/Web
	cp -r src/DatenMeisterWeb/* bin/Web/

.PHONY: clean
clean:
	make clean -C packages/burnsystems
	make clean -C packages/burnsystems.parser
	make clean -C packages/burnsystems.webserver
	make clean -C packages/burnsystems.flexbg
	rm -rf packages/bin
	rm -rf bin


