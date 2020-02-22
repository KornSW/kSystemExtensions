Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

<Assembly: AssemblyTitle("kSystemExtensions")>
<Assembly: AssemblyDescription("kSystemExtensions")>
<Assembly: AssemblyProduct("kSystemExtensions")>
<Assembly: AssemblyCompany("KornSW")>
<Assembly: AssemblyCopyright("KornSW")>
<Assembly: AssemblyTrademark("KornSW")>

<Assembly: CLSCompliant(True)>
<Assembly: ComVisible(False)>
<Assembly: Guid("e8c74000-040a-480d-8519-6a39e15fb6a0")>

<Assembly: AssemblyVersion(Major + "." + Minor + "." + Fix + "." + BuildNumber)>
<Assembly: AssemblyInformationalVersion(Major + "." + Minor + "." + Fix + "-" + BuildType)>

Public Module SemanticVersion

  'increment this on breaking change:
  Public Const Major = "2"

  'increment this on new feature (w/o breaking change):
  Public Const Minor = "3"

  'increment this on internal fix (w/o breaking change):
  Public Const Fix = "1"

  'AND DONT FORGET TO UPDATE THE VERSION-INFO OF THE *.nuspec FILE!!!
#Region "..."

  'dont touch this, beacuse it will be replaced ONLY by the build process!!!

  Public Const BuildNumber = "*"
  Public Const BuildType = "LOCALBUILD"

#End Region
End Module