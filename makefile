CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser build_burnsystems_webserver build_burnsystems_flexbg
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

packages/bin/BurnSystems.FlexBG.dll: packages/burnsystems.webserver/bin/BurnSystems.WebServer.dll
	mkdir -p packages/bin
	cp -r packages/burnsystems.flexbg/bin/* packages/bin/

#bin/BurnSystems.Website.Social.dll: packages/bin/BurnSystems.dll packages/bin/BurnSystems.Parser.dll packages/bin/BurnSystems.WebServer.dll packages/bin/BurnSystems.FlexBG.dll $(CS_FILES)
#	xbuild src/BurnSystems.Website.Social/BurnSystems.Website.Social.csproj
#	mkdir -p bin
#	cp -r src/BurnSystems.Website.Social/bin/Debug/* bin/


.PHONY: clean
clean:
	make clean -C packages/burnsystems
	make clean -C packages/burnsystems.parser
	make clean -C packages/burnsystems.webserver
	make clean -C packages/burnsystems.flexbg
	rm -rf packages/bin
	rm -rf bin


