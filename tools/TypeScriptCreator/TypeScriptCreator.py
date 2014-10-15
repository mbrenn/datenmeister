# Loading DatenMeister
import clr
clr.AddReferenceToFile("DatenMeister")
import DatenMeister
from BurnSystems.DatenMeister import TypeScriptFactory
from BurnSystems.DatenMeister import CSharpFactory

#
# Creates the information for the DatenMeister
print('Creating the DatenMeister')
tsUMLFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.dm.objects.ts"
umlTypes = [
         DatenMeister.Entities.DM.RecentProject ]
TypeScriptFactory.createFiles(tsUMLFilename, umlTypes)

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\DM.Objects.cs"
CSharpFactory.createFiles(csFilename, umlTypes, "DatenMeister.Entities.AsObject.DM");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\DM.Types.cs"
CSharpFactory.createTypeFile(csFilename, umlTypes, "DatenMeister.Entities.AsObject.DM", "Types", "datenmeister:///types/datenmeister");

#
# Creates the information for the fieldinfos, which is used by Datenmeister
print('Creating the DatenMeister Fieldinfo-File')
tsFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.fieldinfo.objects.ts"
types = [
         DatenMeister.Entities.FieldInfos.Comment, 
         DatenMeister.Entities.FieldInfos.General, 
         DatenMeister.Entities.FieldInfos.Checkbox, 
         DatenMeister.Entities.FieldInfos.TextField,
         DatenMeister.Entities.FieldInfos.DatePicker,
         DatenMeister.Entities.FieldInfos.ActionButton,
         DatenMeister.Entities.FieldInfos.ReferenceBase,
         DatenMeister.Entities.FieldInfos.ReferenceByValue,
         DatenMeister.Entities.FieldInfos.ReferenceByRef,
         DatenMeister.Entities.FieldInfos.MultiReferenceField,
         DatenMeister.Entities.FieldInfos.View,
         DatenMeister.Entities.FieldInfos.FormView,
         DatenMeister.Entities.FieldInfos.TableView]
TypeScriptFactory.createFiles(tsFilename, types)

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\FieldInfo.Objects.cs"
CSharpFactory.createFiles(csFilename, types, "DatenMeister.Entities.AsObject.FieldInfo");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\FieldInfo.Types.cs"
CSharpFactory.createTypeFile(csFilename, types, "DatenMeister.Entities.AsObject.FieldInfo", "Types", "datenmeister:///types/fieldinfo");

print('Creating the UML-File')

#
# Creates the information for the UML Objects
tsUMLFilename = "..\\..\\src\\DatenMeisterWeb\\js\\datenmeister\\datenmeister.uml.objects.ts"
umlTypes = [
         DatenMeister.Entities.UML.NamedElement,       
         DatenMeister.Entities.UML.Type,       
         DatenMeister.Entities.UML.Property ]
TypeScriptFactory.createFiles(tsUMLFilename, umlTypes)

csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\UML.Objects.cs"
CSharpFactory.createFiles(csFilename, umlTypes, "DatenMeister.Entities.AsObject.Uml");
csFilename = "..\\..\\src\\DatenMeister\\Entities\\AsObject\\UML.Types.cs"
CSharpFactory.createTypeFile(csFilename, umlTypes, "DatenMeister.Entities.AsObject.Uml", "Types", "datenmeister:///types/uml");

#
# Creates the entities for the ProjektMeister
print('Creating the ProjektMeister')
clr.AddReferenceToFile("ProjektMeister")
import ProjektMeister
projektMeisterTypes = [
         ProjektMeister.Data.Entities.Person,       
         ProjektMeister.Data.Entities.Task ]

csFilename = "..\\..\\..\\projektmeister\\src\\ProjektMeister\\Data\\Entities\\AsObject\\PM.Objects.cs"
CSharpFactory.createFiles(csFilename, projektMeisterTypes, "ProjektMeister.Data.Entities.AsObject");
csFilename = "..\\..\\..\\projektmeister\\src\\ProjektMeister\\Data\\Entities\\AsObject\\PM.Types.cs"
CSharpFactory.createTypeFile(csFilename, projektMeisterTypes, "ProjektMeister.Data.Entities.AsObject", "Types", "datenmeister:///projektmeister/types");

print('Compiling the TypeScript-File')
TypeScriptFactory.compile(tsFilename)
TypeScriptFactory.compile(tsUMLFilename)
 
print('Files have been created and compiled')