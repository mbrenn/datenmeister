CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser bin/BurnSystems.WebServer.dll bin/BurnSystems.WebServer.UnitTests.dll bin/test-server/SimpleTestServer.exe

.PHONY: build_burnsystems
build_burnsystems:
	make -C packages/burnsystems

.PHONY: build_burnsystems_parser
build_burnsystems_parser:
	make -C packages/burnsystems.parser

packages/bin/BurnSystems.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems/bin/* packages/bin/

packages/bin/BurnSystems.Parser.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems.parser/bin/* packages/bin/

bin/BurnSystems.WebServer.dll:$(CS_FILES) packages/bin/BurnSystems.dll packages/bin/BurnSystems.Parser.dll
	xbuild src/BurnSystems.WebServer/BurnSystems.WebServer.csproj
	mkdir -p bin/
	cp src/BurnSystems.WebServer/bin/Debug/* bin/

bin/BurnSystems.WebServer.UnitTests.dll: $(CS_FILES) bin/BurnSystems.WebServer.dll
	xbuild src/BurnSystems.WebServer.UnitTests/BurnSystems.WebServer.UnitTests.csproj
	mkdir -p bin/
	cp -r src/BurnSystems.WebServer.UnitTests/bin/Debug/* bin/

build_SimpleTestServer: bin/SimpleTestServer.exe

bin/test-server/SimpleTestServer.exe: $(CS_FILES) bin/BurnSystems.WebServer.dll
	xbuild src/SimpleTestServer/SimpleTestServer.csproj
	mkdir -p bin/
	mkdir -p bin/test-server
	cp -r src/SimpleTestServer/bin/Debug/* bin/test-server
	mkdir -p bin/test-server/htdocs
	mkdir -p src/SimpleTestServer/bin/Debug/htdocs
	mkdir -p src/SimpleTestServer/bin/Debug/htdocs/js
	cp -r src/BurnSystems.WebServer/Resources/JQuery/* src/SimpleTestServer/bin/Debug/htdocs/js
	mkdir -p bin/test-server/htdocs/js
	cp -r src/BurnSystems.WebServer/Resources/JQuery/* bin/test-server/htdocs/js


.PHONY: clean
clean:
	make clean -C packages/burnsystems
	make clean -C packages/burnsystems.parser
	rm -fR src/bin
	rm -fR src/obj
	rm -fR bin
