Imports BioNovoGene.Analytical.MassSpectrometry.MsImaging.Blender.Scaler
Imports ggplot.options

Public Class MSIFilterPipelineOption : Inherits ggplotOption

    Public Property pipeline As RasterPipeline

    Public Overrides Function Config(ggplot As ggplot.ggplot) As ggplot.ggplot
        Throw New NotImplementedException()
    End Function
End Class
