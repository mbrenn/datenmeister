# Loading DatenMeister
import clr
clr.AddReferenceToFile("DatenMeister")
import DatenMeister
from BurnSystems.DatenMeister import TypeScriptFactory

# Creating the TypeScript-File
filename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\test.ts";
types = [DatenMeister.Web.ServerInfo, DatenMeister.Web.JsonExtentInfo]
TypeScriptFactory.createFiles(filename, types)

TypeScriptFactory.compile(filename)

print('Files have been created')