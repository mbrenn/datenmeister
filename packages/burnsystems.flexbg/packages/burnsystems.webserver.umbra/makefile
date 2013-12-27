CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser build_burnsystems_webserver bin/BurnSystems.WebServer.Umbra.dll


.PHONY: build_burnsystems
build_burnsystems:
	make -C packages/burnsystems

.PHONY: build_burnsystems_parser
build_burnsystems_parser:
	make -C packages/burnsystems.parser

.PHONY: build_burnsystems_webserver
build_burnsystems_webserver:
	make -C packages/burnsystems.webserver

packages/bin/BurnSystems.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems/bin/* packages/bin/

packages/bin/BurnSystems.Parser.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems.parser/bin/* packages/bin/

packages/bin/BurnSystems.WebServer.dll: packages/burnsystems/bin/BurnSystems.dll packages/burnsystems.parser/bin/BurnSystems.Parser.dll
	mkdir -p packages/bin
	cp -r packages/burnsystems.webserver/bin/* packages/bin/


bin/BurnSystems.WebServer.Umbra.dll: $(CS_FILES) packages/bin/BurnSystems.dll packages/bin/BurnSystems.Parser.dll packages/bin/BurnSystems.WebServer.dll
	xbuild src/BurnSystems.WebServer.Umbra/BurnSystems.WebServer.Umbra.csproj
	mkdir -p bin/
	mkdir -p bin/htdocs
	cp src/BurnSystems.WebServer.Umbra/bin/Debug/* bin/
	cp -r src/BurnSystems.WebServer.Umbra/htdocs/* bin/htdocs/
	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/JQuery/jquery.js bin/htdocs/js/lib/
	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/Require/require.js bin/htdocs/js/lib/
	cp packages/burnsystems.webserver/src/BurnSystems.WebServer/Resources/DateFormat/dateformat.js bin/htdocs/js/lib/

clean:
	make -C packages/burnsystems clean
	make -C packages/burnsystems.parser clean
	make -C packages/burnsystems.webserver clean
	rm -rf bin
	rm -rf packages/bin
	rm -rf src/BurnSystems.WebServer.Umbra/bin
