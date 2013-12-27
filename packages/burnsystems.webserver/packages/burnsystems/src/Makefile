MCS_FLAGS = -warn:4
MCS = gmcs

CSHARP_FILES = *.cs \
	Collections/*.cs \
	Graphics/*.cs \
	Interfaces/*.cs IO/*.cs \
	Logging/*.cs \
	Net/*.cs \
	Properties/*.cs \
	Serialization/*.cs \
	Synchronisation/*.cs \
	Test/*.cs \
	UnitTests/*.cs

BURNSYSTEMS_REFERENCES = System.Drawing.dll,System.Web.dll

all: 	bin/Release/BurnSystems.dll

bin/Release/BurnSystems.dll: $(CSHARP_FILES) LocalizationBS.resources
	mkdir -p bin
	mkdir -p bin/Release
	$(MCS) $(MCS_FLAGS) -out:bin/Release/BurnSystems.dll -target:library -r:$(BURNSYSTEMS_REFERENCES) -resource:LocalizationBS.resources $(CSHARP_FILES)

LocalizationBS.resources: LocalizationBS.resx
	resgen2 LocalizationBS.resx

.PHONY: clean
clean:
	rm -rf bin
	rm *.resources
