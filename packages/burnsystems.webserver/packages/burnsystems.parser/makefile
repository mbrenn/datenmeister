CS_FILES = $(shell find src/ -type f -name *.cs)
TESTS_CS_FILES = $(shell find tests/ -type f -name *.cs)

all: build_burnsystems bin/BurnSystems.Parser.dll bin/BurnSystems.Parser.UnitTests.dll

.PHONY: build_burnsystems
build_burnsystems:
	make -C packages/burnsystems

packages/bin/BurnSystems.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems/bin/* packages/bin/

bin/BurnSystems.Parser.dll: $(CS_FILES) packages/bin/BurnSystems.dll
	xbuild src/BurnSystems.Parser.csproj
	mkdir -p bin
	cp src/bin/Debug/* bin/

bin/BurnSystems.Parser.UnitTests.dll: $(TESTS_CS_FILES) bin/BurnSystems.Parser.dll
	xbuild tests/BurnSystems.Parser.UnitTests/BurnSystems.Parser.UnitTests.csproj
	mkdir -p bin
	cp tests/BurnSystems.Parser.UnitTests/bin/Debug/* bin/

.PHONY: install
install: all
	mkdir -p ~/lib/mono
	cp bin/* ~/lib/mono

.PHONY: test
test: all
	nunit-console -labels bin/BurnSystems.Parser.UnitTests.dll

.PHONY: clean
clean:
	make clean -C packages/burnsystems
	rm -fR src/bin
	rm -fR src/obj
	rm -fR bin
