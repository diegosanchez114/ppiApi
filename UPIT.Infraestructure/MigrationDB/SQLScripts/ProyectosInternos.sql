CREATE DATABASE ProyectosInternos

CREATE TABLE [dbo].[Avances] (
  [idAvance] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  [idProyecto] UNIQUEIDENTIFIER  NULL,
  [porcentajeAvanceTotalPlaneado] decimal(5,2)  NULL,
  [fechaInicio] date  NULL,
  [fechaFinal] date  NULL,
  [fechaFinalEfectiva] date  NULL,
  [observaciones] text COLLATE Modern_Spanish_CI_AS  NULL,
  [fechaCreacion] date  NULL,
  [fechaActualizacion] date  NULL
)
GO

CREATE TABLE [dbo].[Contratistas] (
  [idContratista] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  [nombreContratista] varchar(100) COLLATE Modern_Spanish_CI_AS  NULL,
  [nombreAccionistaContratista] varchar(100) COLLATE Modern_Spanish_CI_AS  NULL,
  [porcentajeAccionistaContratista] decimal(18,2)  NULL,
  [fechaCreacion] date  NULL,
  [fechaActualizacion] date  NULL
)
GO

CREATE TABLE [dbo].[Interventorias] (
  [idInterventoria] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  [nombreInterventoria] varchar(100) COLLATE Modern_Spanish_CI_AS  NULL,
  [nombreAccionistaInterventoria] varchar(100) COLLATE Modern_Spanish_CI_AS  NULL,
  [porcentajeAccionistaInterventoria] decimal(18,2)  NULL,
  [fechaCreacion] date  NULL,
  [fechaActualizacion] date  NULL
)
GO

CREATE TABLE [dbo].[Obligaciones] (
  [idObligacion] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  [idProyecto] UNIQUEIDENTIFIER  NULL,
  [idContratista] UNIQUEIDENTIFIER  NULL,
  [idInterventoria] UNIQUEIDENTIFIER  NULL,
  [tipoObligacion] int  NULL,
  [tiempo] int  NULL,
  [porcentajeAvancePlaneado] decimal(5,2)  NULL,
  [porcentajeAvanceEjecutado] decimal(5,2)  NULL,
  [Observaciones] text COLLATE Modern_Spanish_CI_AS  NULL,
  [fechaCreacion] date  NULL,
  [fechaActualizacion] date  NULL
)
GO

CREATE TABLE [dbo].[Proyectos] (
  [idProyecto] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  [idSubdireccion] UNIQUEIDENTIFIER  NULL,
  [nombreProyecto] varchar(100) COLLATE Modern_Spanish_CI_AS  NULL,
  [objetoProyecto] text COLLATE Modern_Spanish_CI_AS  NULL,
  [fechaInicioContrato] date  NULL,
  [fechaFinalContratoContractual] date  NULL,
  [fechaFinalRealContrato] date  NULL,
  [fechaSuscripcionContrato] date  NULL,
  [porcentajeTotalAvancePlaneado] decimal(10,2)  NULL,
  [observaciones] text COLLATE Modern_Spanish_CI_AS  NULL,
  [valorContrastistaInicial] money  NULL,
  [valorInterventoriaInicial] money  NULL,
  [fechaCreacion] date  NULL,
  [fechaActualizacion] datetime  NULL
)
GO

CREATE TABLE [dbo].[Subdirecciones] (
  [idSubdireccion] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  [nombreDependencia] varchar(100) COLLATE Modern_Spanish_CI_AS  NULL,
  [encargadoDependencia] varchar(100) COLLATE Modern_Spanish_CI_AS  NULL,
  [fechaCreacion] date  NULL,
  [fechaActualizacion] date  NULL
)
GO


ALTER TABLE [dbo].[Avances] ADD CONSTRAINT [FK_Avances_Proyectos] FOREIGN KEY ([idProyecto]) REFERENCES [dbo].[Proyectos] ([idProyecto]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Contratistas] ADD CONSTRAINT [FK_Contratistas_Proyectos] FOREIGN KEY ([idProyecto]) REFERENCES [dbo].[Proyectos] ([idProyecto]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Interventorias] ADD CONSTRAINT [FK_Interventorias_Proyectos] FOREIGN KEY ([idProyecto]) REFERENCES [dbo].[Proyectos] ([idProyecto]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Obligaciones] ADD CONSTRAINT [FK_InformacionFinanciera_Proyectos] FOREIGN KEY ([idProyecto]) REFERENCES [dbo].[Proyectos] ([idProyecto]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Obligaciones] ADD CONSTRAINT [FK_Obligaciones_Contratistas] FOREIGN KEY ([idContratista]) REFERENCES [dbo].[Contratistas] ([idContratista]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Obligaciones] ADD CONSTRAINT [FK_Obligaciones_Interventorias] FOREIGN KEY ([idInterventoria]) REFERENCES [dbo].[Interventorias] ([idInterventoria]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

ALTER TABLE [dbo].[Proyectos] ADD CONSTRAINT [FK_Proyectos_Dependencias] FOREIGN KEY ([idSubdireccion]) REFERENCES [dbo].[Subdirecciones] ([idSubdireccion]) ON DELETE NO ACTION ON UPDATE NO ACTION
GO

