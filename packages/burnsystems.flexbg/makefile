CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser build_burnsystems_webserver build_burnsystems_webserver_umbra bin/BurnSystems.FlexBG.dll bin/BurnSystems.FlexBG.Test.dll

.PHONY: build_burnsystems
build_burnsystems:
	make -C packages/burnsystems

.PHONY: build_burnsystems_parser
build_burnsystems_parser:
	make -C packages/burnsystems.parser

.PHONY: build_burnsystems_webserver
build_burnsystems_webserver:
	make -C packages/burnsystems.webserver

.PHONY: build_burnsystems_webserver_umbra
build_burnsystems_webserver_umbra:
	make -C packages/burnsystems.webserver.umbra

packages/bin/BurnSystems.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems/bin/* packages/bin/

packages/bin/BurnSystems.Parser.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems.parser/bin/* packages/bin/

packages/bin/BurnSystems.WebServer.dll: packages/burnsystems/bin/BurnSystems.dll packages/burnsystems.parser/bin/BurnSystems.Parser.dll
	mkdir -p packages/bin
	cp packages/burnsystems.webserver/bin/*.dll packages/bin/
	cp packages/burnsystems.webserver/bin/*.mdb packages/bin/

packages/bin/BurnSystems.WebServer.Umbra.dll: packages/burnsystems.webserver/bin/BurnSystems.WebServer.dll
	mkdir -p packages/bin
	cp -r packages/burnsystems.webserver.umbra/bin/* packages/bin/

bin/BurnSystems.FlexBG.dll: $(CS_FILES) packages/bin/BurnSystems.dll packages/bin/BurnSystems.Parser.dll packages/bin/BurnSystems.WebServer.dll packages/bin/BurnSystems.WebServer.Umbra.dll
	xbuild src/BurnSystems.FlexBG/BurnSystems.FlexBG.csproj
	mkdir -p bin/
	cp -r src/BurnSystems.FlexBG/bin/Debug/* bin/

bin/BurnSystems.FlexBG.Test.dll: $(CS_FILES) bin/BurnSystems.WebServer.dll
	xbuild src/BurnSystems.FlexBG.Test/BurnSystems.FlexBG.Test.csproj
	mkdir -p bin/
	cp -r src/BurnSystems.FlexBG.Test/bin/Debug/* bin/

.PHONY: clean
clean:
	make clean -C packages/burnsystems
	make clean -C packages/burnsystems.parser
	make clean -C packages/burnsystems.webserver
	make clean -C packages/burnsystems.webserver.umbra
	rm -rf src/BurnSystems.FlexBG/bin
	rm -rf src/BurnSystems.FlexBG/obj
	rm -rf src/BurnSystems.FlexBG.Test/bin
	rm -rf src/BurnSystems.FlexBG.Test/obj
	rm -rf packages/bin
	rm -rf bin
