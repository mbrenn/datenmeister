# Loading DatenMeister
import clr
clr.AddReferenceToFile("DatenMeister")
import DatenMeister
from BurnSystems.DatenMeister import TypeScriptFactory
from BurnSystems.DatenMeister import CSharpFactory

# Creating the TypeScript-File

print('Creating the TypeScript-File')
filename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.fieldinfo.objects.ts"
types = [
         DatenMeister.Entities.FieldInfos.Comment, 
         DatenMeister.Entities.FieldInfos.General, 
         DatenMeister.Entities.FieldInfos.TextField,
         DatenMeister.Entities.FieldInfos.ActionButton,
         DatenMeister.Entities.FieldInfos.View,
         DatenMeister.Entities.FieldInfos.FormView,
         DatenMeister.Entities.FieldInfos.TableView]
TypeScriptFactory.createFiles(filename, types)

print('Creating the C#-File')

csFilename = "FieldInfoObjects.cs"
CSharpFactory.createFiles(csFilename, types, "BurnSystems.Test");

print('Compiling the TypeScript-File')
TypeScriptFactory.compile(filename)

print('Files have been created and compiled')