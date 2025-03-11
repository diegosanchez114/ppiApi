CREATE TABLE PPI_Proyectos (
  idProyecto UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  idEntidadResponsable UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Entidades(idEntidad),
  codigo NVARCHAR(50) NULL,
  nombreLargo NVARCHAR(100) NULL,
  nombreCorto NVARCHAR(100) NULL,
  categoria NVARCHAR(100) NULL,
  modo NVARCHAR(100) NULL,
  estaRadicado BIT DEFAULT 0,
  codigoCorredor NVARCHAR(100) NULL,
  tieneEstudiosYDisenio BIT DEFAULT 0,
  programa NVARCHAR(100) NULL,
  priorizacionInvias NVARCHAR(100) NULL,
  bpinPng NVARCHAR(100) NULL,
  --valorTotalProyecto NVARCHAR(50) NULL,
  observaciones NVARCHAR(MAX) NULL DEFAULT '',
  tieneContratos BIT DEFAULT 0,
  tieneContratosObservacion NVARCHAR(MAX) NULL DEFAULT '',
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL
);

CREATE TABLE PPI_Contratos (
  idContrato UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  idProyecto UNIQUEIDENTIFIER FOREIGN KEY REFERENCES PPI_Proyectos(idProyecto),
  idEntidadResponsable UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Entidades(idEntidad),
  codigo NVARCHAR(50) NULL,
  objeto NVARCHAR(MAX) NULL DEFAULT '',
  fechaInicioContrato DATE NULL,
  fechaTerminacionContrato DATE NULL,
  valorContrato money NOT NULL,
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL
);

CREATE TABLE PPI_Avances (
  idAvance UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  idContrato UNIQUEIDENTIFIER FOREIGN KEY REFERENCES PPI_Contratos(idContrato),  
  fecha DATE NOT NULL,
  porcentajeProgramado decimal(10,2) NOT NULL,
  porcentajeEjecutado decimal(10,2) NOT NULL,
  valorEjecutado money NULL,
  observaciones NVARCHAR(MAX) NULL DEFAULT '',
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL,
  tieneAlerta BIT DEFAULT 0,
  alertaResuelta BIT DEFAULT 0
);

CREATE TABLE PPI_Novedades (
  idNovedad UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  idAvance UNIQUEIDENTIFIER FOREIGN KEY REFERENCES PPI_Avances(idAvance),  
  fecha DATE NOT NULL,  
  descripcion NVARCHAR(MAX) NULL DEFAULT '',
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL
);

CREATE TABLE PPI_NecesidadInversion (
  idNecesidad UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  idContrato UNIQUEIDENTIFIER FOREIGN KEY REFERENCES PPI_Contratos(idContrato),  
  valorInversion money NOT NULL,  
  tipoObra NVARCHAR(200) NOT NULL,
  descripcion NVARCHAR(MAX) NULL DEFAULT '',
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL
);




--ALTER TABLES
ALTER TABLE PPI_Contratos
ADD idEntidadResponsable UNIQUEIDENTIFIER;

ALTER TABLE PPI_Proyectos
ADD tieneContratos BIT DEFAULT 0;

ALTER TABLE PPI_Proyectos
ADD tieneContratosObservacion NVARCHAR(MAX) NULL DEFAULT '';

ALTER TABLE PPI_Contratos
ADD CONSTRAINT FK_PPI_Contratos_Entidades
FOREIGN KEY (idEntidadResponsable) REFERENCES Entidades(idEntidad);

ALTER TABLE PPI_Contratos
ADD valorContrato MONEY;

ALTER TABLE PPI_Avances
ADD tieneAlerta BIT DEFAULT 0,
    alertaResuelta BIT DEFAULT 0;
