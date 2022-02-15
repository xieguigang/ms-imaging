Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices

#if netcore5=0 then 

' 有关程序集的一般信息由以下
' 控制。更改这些特性值可修改
' 与程序集关联的信息。

'查看程序集特性的值

<Assembly: AssemblyTitle("MS-Imaging")>
<Assembly: AssemblyDescription("MS-Imaging based on ggplot data visualization framework")>
<Assembly: AssemblyCompany("BioNovoGene")>
<Assembly: AssemblyProduct("mzkit")>
<Assembly: AssemblyCopyright("Copyright © BioNovoGene 2022")>
<Assembly: AssemblyTrademark("mzkit")>

<Assembly: ComVisible(False)>

'如果此项目向 COM 公开，则下列 GUID 用于 typelib 的 ID
<Assembly: Guid("33be08e5-535e-4daf-ade2-835f5eb19389")>

' 程序集的版本信息由下列四个值组成: 
'
'      主版本
'      次版本
'      生成号
'      修订号
'
'可以指定所有这些值，也可以使用“生成号”和“修订号”的默认值
'通过使用 "*"，如下所示:

<Assembly: AssemblyVersion("2.23.55.899")>
<Assembly: AssemblyFileVersion("2.548.11.0")>
#end if