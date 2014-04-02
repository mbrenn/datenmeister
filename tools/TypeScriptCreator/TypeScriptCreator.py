# Loading DatenMeister
import clr
clr.AddReferenceToFile("DatenMeister")
import DatenMeister
from BurnSystems.DatenMeister import TypeScriptFactory
from BurnSystems.DatenMeister import CSharpFactory

# Creating the TypeScript-File

print('Creating the Fieldinfo-File')
tsFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.fieldinfo.objects.ts"
types = [
         DatenMeister.Entities.FieldInfos.Comment, 
         DatenMeister.Entities.FieldInfos.General, 
         DatenMeister.Entities.FieldInfos.Checkbox, 
         DatenMeister.Entities.FieldInfos.TextField,
         DatenMeister.Entities.FieldInfos.ActionButton,
         DatenMeister.Entities.FieldInfos.View,
         DatenMeister.Entities.FieldInfos.FormView,
         DatenMeister.Entities.FieldInfos.TableView]
TypeScriptFactory.createFiles(tsFilename, types)

print('Creating the UML-File')
tsUMLFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.uml.objects.ts"
umlTypes = [
         DatenMeister.Entities.UML.NamedElement,       
         DatenMeister.Entities.UML.Type ]
TypeScriptFactory.createFiles(tsUMLFilename, umlTypes)

print('Creating the C#-File')

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\FieldInfo.Objects.cs"
CSharpFactory.createFiles(csFilename, types, "DatenMeister.Entities.AsObject.FieldInfo");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\FieldInfo.Types.cs"
CSharpFactory.createTypeFile(csFilename, types, "DatenMeister.Entities.AsObject.FieldInfo", "Types");

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\UML.Objects.cs"
CSharpFactory.createFiles(csFilename, umlTypes, "DatenMeister.Entities.AsObject.Uml");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\UML.Types.cs"
CSharpFactory.createTypeFile(csFilename, umlTypes, "DatenMeister.Entities.AsObject.Uml", "Types");

print('Compiling the TypeScript-File')
TypeScriptFactory.compile(tsFilename)
TypeScriptFactory.compile(tsUMLFilename)
 
print('Files have been created and compiled')