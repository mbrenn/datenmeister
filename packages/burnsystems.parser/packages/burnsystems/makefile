
CS_FILES = $(shell find src/ -type f -name *.cs)
TESTS_CS_FILES = $(shell find tests/ -type f -name *.cs)

all: bin/BurnSystems.dll bin/BurnSystems.UnitTests.dll

bin/BurnSystems.dll: $(CS_FILES)
	xbuild src/BurnSystems.csproj
	mkdir -p bin
	cp src/bin/Debug/* bin

bin/BurnSystems.UnitTests.dll: $(TESTS_CS_FILES)
	xbuild tests/BurnSystems.UnitTests/BurnSystems.UnitTests.csproj
	mkdir -p bin
	cp tests/BurnSystems.UnitTests/bin/Debug/* bin/

.PHONY: install
install: all
	mkdir -p ~/lib/mono
	cp bin/* ~/lib/mono

.PHONY: test
test: all
	nunit-console -labels bin/BurnSystems.UnitTests.dll

.PHONY: clean
clean:
	rm -fR src/bin
	rm -fR src/obj
	rm -fR bin
