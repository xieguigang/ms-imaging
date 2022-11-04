Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports ggplot.options

Public Class MSIFilterPipelineOption : Inherits ggplotOption

    Public Property pipeline As RasterPipeline

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        DirectCast(ggplot, ggplotMSI).filter = pipeline
        Return ggplot
    End Function
End Class
