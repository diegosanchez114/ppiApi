CREATE TABLE Entidades (
  idEntidad UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  nombre NVARCHAR(100) NULL,
  descripcion NVARCHAR(100) NULL,
  direccion NVARCHAR(100) NULL,
  telefono NVARCHAR(100) NULL,
  fechaCreacion DATE NOT NULL,
  fechaActualizacion DATE NULL
);

CREATE TABLE Parametricas (
    id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    nombre NVARCHAR(MAX) NOT NULL,
	valor NVARCHAR(MAX) NOT NULL,
    descripcion NVARCHAR(MAX),
    fechaCreacion DATE NOT NULL,
    fechaActualizacion DATE NULL
);

CREATE TABLE Roles (
    RoleId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoleName NVARCHAR(50) NOT NULL,
    RoleDescription NVARCHAR(MAX),
    RoleCreated DATE,
    RoleUpdated DATE
);

CREATE TABLE Scopes (
    ScopeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    ScopeName NVARCHAR(50) NOT NULL,
    ScopePath NVARCHAR(200) NOT NULL,
    ScopeMethod NVARCHAR(10) NOT NULL,
    ScopeDescription NVARCHAR(MAX),
    ScopeCreated DATE,
    ScopeUpdated DATE  
);

CREATE TABLE RoleScopes (
    RoleScopeId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoleId UNIQUEIDENTIFIER NOT NULL,
    ScopeId UNIQUEIDENTIFIER NOT NULL,
    RoleScopeCreated DATE,
    RoleScopeUpdated DATE,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    FOREIGN KEY (ScopeId) REFERENCES Scopes(ScopeId)
);

CREATE TABLE Users (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserEmail NVARCHAR(100) UNIQUE NOT NULL,
    UserIdentificationNumber NVARCHAR(20) NOT NULL,
    UserDocumentType NVARCHAR(20) NOT NULL,    
    UserFirstName NVARCHAR(50) NOT NULL,
    UserLastName NVARCHAR(50) NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    EntityId UNIQUEIDENTIFIER NOT NULL,
    UserPhoneNumber NVARCHAR(15) NOT NULL,
    UserPosition NVARCHAR(50),
    UserState BIT,
    UserPassword NVARCHAR(100) NOT NULL,
    UserCreated DATE,
    UserUpdated DATE,
    FOREIGN KEY (EntityId) REFERENCES Entidades(idEntidad),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

CREATE TABLE Logs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Timestamp DATETIMEOFFSET NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    LogLevel NVARCHAR(50) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    ExceptionInfo NVARCHAR(MAX) NULL,
    UserId NVARCHAR(200) NULL,
    Username NVARCHAR(256) NULL,
    EventId INT NULL,
    Source NVARCHAR(256) NULL,
    StackTraceInfo NVARCHAR(MAX) NULL,
    CorrelationId NVARCHAR(200) NULL,
    IpAddress NVARCHAR(45) NULL,
    RequestUrl NVARCHAR(2048) NULL,
    HttpMethodRequest NVARCHAR(10) NULL,
    UserAgent NVARCHAR(512) NULL,
    AdditionalData NVARCHAR(MAX) NULL
);




-- NOTA EL SIGUIENTE SCRIPT BORRA LAS TABLAS SI ES NECESARIO RECREARLAS

-- Verificar si las tablas existen antes de intentar eliminarlas
IF OBJECT_ID('dbo.PPI_Avances', 'U') IS NOT NULL
    DROP TABLE dbo.PPI_Avances;
GO

IF OBJECT_ID('dbo.PPI_Contratos', 'U') IS NOT NULL
    DROP TABLE dbo.PPI_Contratos;
GO

IF OBJECT_ID('dbo.PPI_Proyectos', 'U') IS NOT NULL
    DROP TABLE dbo.PPI_Proyectos;
GO

IF OBJECT_ID('dbo.Entidades', 'U') IS NOT NULL
    DROP TABLE dbo.Entidades;
GO

IF OBJECT_ID('dbo.Parametricas', 'U') IS NOT NULL
    DROP TABLE dbo.Parametricas;
GO