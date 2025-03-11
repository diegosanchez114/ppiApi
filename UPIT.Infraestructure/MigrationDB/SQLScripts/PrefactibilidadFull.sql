CREATE TABLE Pre_Proyectos (
  idProyecto UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  nombre NVARCHAR(100) NOT NULL,
  observaciones NVARCHAR(MAX) NULL DEFAULT '',
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL
);

CREATE TABLE Pre_TiposAvance (
  idTipoAvance UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  nombre NVARCHAR(100) NOT NULL,
  observaciones NVARCHAR(MAX) NULL DEFAULT '',
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL
);

CREATE TABLE Pre_Avances (
  idAvance UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  idProyecto UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Pre_Proyectos(idProyecto),
  idTipoAvance UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Pre_TiposAvance(idTipoAvance),
  numAvance INT NOT NULL,
  observaciones NVARCHAR(MAX) NULL DEFAULT '',
  fechaAvance DATE NOT NULL,
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NOT NULL
);
