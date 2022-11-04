
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender
Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports Microsoft.VisualBasic.CommandLine.Reflection
Imports Microsoft.VisualBasic.Scripting.MetaData

<Package("MSIfilter")>
Module Filters

    <ExportAPI("log_scale")>
    Public Function logScaler() As LogScaler
        Return New LogScaler
    End Function

    <ExportAPI("quantile_scale")>
    Public Function quantileScaler() As QuantileScaler
        Return New QuantileScaler
    End Function

    <ExportAPI("TrIQ_scale")>
    Public Function TrIQScaler() As TrIQScaler
        Return New TrIQScaler
    End Function

    <ExportAPI("soften_scale")>
    Public Function softenScaler() As SoftenScaler
        Return New SoftenScaler(New TrIQThreshold(0.999))
    End Function

    <ExportAPI("knn_scale")>
    Public Function knnScaler() As KNNScaler
        Return New KNNScaler
    End Function
End Module
