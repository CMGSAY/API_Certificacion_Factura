-- ========================================
-- CREACIÓN DE BASE DE DATOS
-- ========================================
CREATE DATABASE CertificadorDB;


USE CertificadorDB;


-- ========================================
-- TABLA: Emisores
-- ========================================
CREATE TABLE Emisor (
    EmisorId INT IDENTITY(1,1) PRIMARY KEY,
    Nit NVARCHAR(12) NOT NULL UNIQUE,
    RazonSocial NVARCHAR(200) NOT NULL,
    EstadoActivo BIT NOT NULL,
    PuedeCertificar BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);


-- ========================================
-- TABLA: Establecimientos
-- ========================================
CREATE TABLE Establecimiento (
    EstablecimientoId INT IDENTITY(1,1) PRIMARY KEY,
    NombreEstablecimiento NVARCHAR(200) NOT NULL,
    NIT NCHAR(35) NOT NULL,
    Direccion NVARCHAR(300) NOT NULL,
    EstadoActivo BIT NOT NULL,
    CodigoUnicoEstablecimiento NVARCHAR(12) NOT NULL,
    EmisorId INT NOT NULL,
    CONSTRAINT FK_Establecimiento_Emisor
        FOREIGN KEY (EmisorId) REFERENCES Emisor(EmisorId)
);


-- ========================================
-- TABLA: DTE_Facturas
-- ========================================
CREATE TABLE DTE_Factura (
    FacturaId INT IDENTITY(1,1) PRIMARY KEY,
    FechaEmision DATETIME2 NOT NULL,
    FechaIngreso DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    ClienteNIT NVARCHAR(35) NOT NULL,
    ClienteRazonSocial NVARCHAR(300) NOT NULL,
    Total DECIMAL(18,2) NOT NULL CHECK (Total >= 0),
    Estado TINYINT NOT NULL, -- 1=emitida 2=certificada 3=anulada...
    FechaProcesamiento DATETIME2 NULL,
    EstablecimientoId INT NOT NULL,
    CONSTRAINT FK_Factura_Establecimiento
        FOREIGN KEY (EstablecimientoId) REFERENCES Establecimiento(EstablecimientoId)
);


-- ========================================
-- TABLA: DTE_Detalle
-- ========================================
CREATE TABLE DTE_Detalle (
    DetalleId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(200) NOT NULL,
    Precio DECIMAL(18,2) NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    FacturaId INT NOT NULL,
    CONSTRAINT FK_Detalle_Factura
        FOREIGN KEY (FacturaId) REFERENCES DTE_Factura(FacturaId)
);


-- ========================================
-- TABLA: DTE_Certificaciones
-- ========================================
CREATE TABLE DTE_Certificacion (
    CertificacionId INT IDENTITY(1,1) PRIMARY KEY,
    NumeroAutorizacion NVARCHAR(50) NOT NULL,
    Serie NVARCHAR(50) NOT NULL,
    Correlativo INT NOT NULL,
    FechaIngreso DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FechaProcesamiento DATETIME2 NULL,
    FechaCertificacion DATETIME2 NOT NULL,
    FacturaId INT NOT NULL UNIQUE,
    CONSTRAINT FK_Certificacion_Factura
        FOREIGN KEY (FacturaId) REFERENCES DTE_Factura(FacturaId)
);


-- ========================================
-- TABLA: DTE_Anulaciones
-- ========================================
CREATE TABLE DTE_Anulacion (
    AnulacionId INT IDENTITY(1,1) PRIMARY KEY,
    Motivo NVARCHAR(500) NOT NULL,
    FechaIngreso DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    FechaAnulacion DATETIME2 NOT NULL,
    FechaProcesamiento DATETIME2 NULL,
    FacturaId INT NOT NULL UNIQUE,
    CONSTRAINT FK_Anulacion_Factura
        FOREIGN KEY (FacturaId) REFERENCES DTE_Factura(FacturaId)
);


-- ========================================
-- TABLA: Logs_Bitacora
-- ========================================
CREATE TABLE Logs_Bitacora (
    BitacoraId INT IDENTITY(1,1) PRIMARY KEY,
    Accion NVARCHAR(50) NOT NULL,
    FechaAccion DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    Descripcion NVARCHAR(1000) NOT NULL,
    FechaProcesamiento DATETIME2 NULL,
    FacturaId INT NOT NULL,
    CONSTRAINT FK_Log_Factura
        FOREIGN KEY (FacturaId) REFERENCES DTE_Factura(FacturaId)
);


CREATE INDEX IX_Factura_Estado ON DTE_Factura(Estado);
CREATE INDEX IX_Factura_Fecha ON DTE_Factura(FechaEmision);
CREATE INDEX IX_Detalle_Factura ON DTE_Detalle(FacturaId);


-- ========================================
-- EMISORES
-- ========================================

-- 1. Insertar Emisores 
INSERT INTO Emisor (Nit, RazonSocial, EstadoActivo, PuedeCertificar) VALUES
('123456789', 'DEPORTIVOS XELA S.A.', 1, 1),
('987654321', 'EQUIPOS DEPORTIVOS ALTOS S.A.', 1, 1),
('456789123', 'DISTRIBUIDORA DEPORTIVA QUETZAL', 1, 1),
('321654987', 'DEPORTES VOLCÁN S.A.', 1, 1),
('789123456', 'ALTO RENDIMIENTO XELA', 1, 1),
('654987321', 'DEPORTIVOS SAN MARCOS S.A.', 1, 1),
('147258369', 'EQUIPAMIENTO DEPORTIVO KICHE', 1, 1),
('258369147', 'MUNDO DEPORTIVO TOTONICAPAN', 1, 1),
('369147258', 'DEPORTES HUEHUETENANGO S.A.', 1, 1),
('951753852', 'DISTRIBUIDORA DEPORTIVA RETALHULEU', 1, 1);
GO

-- 2. Insertar Establecimientos
INSERT INTO Establecimiento (NombreEstablecimiento, NIT, Direccion, EstadoActivo, CodigoUnicoEstablecimiento, EmisorId) VALUES
-- Emisor 1: DEPORTIVOS XELA S.A.
('DEPORTIVOS XELA - CENTRO', '123456789', '15 Avenida 4-50 Zona 1, Quetzaltenango', 1, 'XELA001', 1),
('DEPORTIVOS XELA - MODELO', '123456789', '9a Calle 9-49 Zona 2, Quetzaltenango', 1, 'XELA002', 1),
('DEPORTIVOS XELA - SAN MATEO', '123456789', '12 Avenida 7-32 Zona 3, Quetzaltenango', 1, 'XELA003', 1),

-- Emisor 2: EQUIPOS DEPORTIVOS ALTOS S.A.
('EQUIPOS ALTOS - CENTRO', '987654321', '5a Calle 12-45 Zona 1, Quetzaltenango', 1, 'XELA004', 2),
('EQUIPOS ALTOS - LAS AMERICAS', '987654321', '8a Avenida 6-23 Zona 2, Quetzaltenango', 1, 'XELA005', 2),

-- Emisor 3: DISTRIBUIDORA DEPORTIVA QUETZAL
('DISTRIBUIDORA QUETZAL - TERMINAL', '456789123', 'Centro Comercial Terminal, Zona 2, Quetzaltenango', 1, 'XELA006', 3),
('DISTRIBUIDORA QUETZAL - PASAJE', '456789123', 'Pasaje Enríquez, Zona 1, Quetzaltenango', 1, 'XELA007', 3),

-- Emisor 4: DEPORTES VOLCÁN S.A.
('DEPORTES VOLCÁN - CENTRO', '321654987', '14 Avenida 3-67 Zona 1, Quetzaltenango', 1, 'XELA008', 4),
('DEPORTES VOLCÁN - SANTA ELENA', '321654987', '6a Calle 15-89 Zona 3, Quetzaltenango', 1, 'XELA009', 4),

-- Emisor 5: ALTO RENDIMIENTO XELA
('ALTO RENDIMIENTO - ZONA 1', '789123456', '11 Avenida 8-34 Zona 1, Quetzaltenango', 1, 'XELA010', 5),
('ALTO RENDIMIENTO - PLAZA COMERCIAL', '789123456', 'Plaza Comercial Xela, Zona 2, Quetzaltenango', 1, 'XELA011', 5),

-- Emisor 6: DEPORTIVOS SAN MARCOS S.A.
('DEPORTIVOS SAN MARCOS - XELA', '654987321', '7a Avenida 5-78 Zona 1, Quetzaltenango', 1, 'XELA012', 6),

-- Emisor 7: EQUIPAMIENTO DEPORTIVO KICHE
('EQUIPAMIENTO KICHE - CENTRO', '147258369', '13 Avenida 2-56 Zona 1, Quetzaltenango', 1, 'XELA013', 7),

-- Emisor 8: MUNDO DEPORTIVO TOTONICAPAN
('MUNDO DEPORTIVO - XELA', '258369147', '10 Avenida 9-12 Zona 2, Quetzaltenango', 1, 'XELA014', 8),

-- Emisor 9: DEPORTES HUEHUETENANGO S.A.
('DEPORTES HUEHUE - SUCURSAL XELA', '369147258', '4a Calle 11-34 Zona 3, Quetzaltenango', 1, 'XELA015', 9),

-- Emisor 10: DISTRIBUIDORA DEPORTIVA RETALHULEU
('DISTRIBUIDORA RETA - XELA', '951753852', '16 Avenida 7-89 Zona 1, Quetzaltenango', 1, 'XELA016', 10);



-- 3. Insertar Facturas con correlación real por establecimiento
INSERT INTO DTE_Factura (FechaEmision, ClienteNIT, ClienteRazonSocial, Total, Estado, EstablecimientoId) VALUES
-- Establecimiento 1 (XELA001) - Serie A
('2024-01-10 08:30:00', 'CF', 'CONSUMIDOR FINAL', 1250.00, 2, 1),
('2024-01-10 11:15:00', '1234567', 'ACADEMIA FUTBOL XELA JUNIOR', 2800.00, 2, 1),
('2024-01-11 14:20:00', '7654321', 'GIMNASIO MUSCLE XELA', 950.50, 2, 1),

-- Establecimiento 2 (XELA002) - Serie B
('2024-01-12 09:45:00', 'CF', 'CONSUMIDOR FINAL', 680.25, 2, 2),
('2024-01-12 16:30:00', '8912345', 'COLEGIO SAN JOSÉ DE XELA', 3200.75, 2, 2),

-- Establecimiento 3 (XELA003) - Serie C
('2024-01-13 10:00:00', 'CF', 'CONSUMIDOR FINAL', 420.00, 2, 3),
('2024-01-13 15:45:00', '1122334', 'UNIVERSIDAD RAFAEL LANDIVAR', 1850.00, 2, 3),

-- Establecimiento 4 (XELA004) - Serie D
('2024-01-14 08:15:00', 'CF', 'CONSUMIDOR FINAL', 1560.00, 2, 4),
('2024-01-14 12:30:00', '4455667', 'CLUB DEPORTIVO XELAJÚ', 2750.25, 2, 4),

-- Establecimiento 5 (XELA005) - Serie E
('2024-01-15 09:20:00', 'CF', 'CONSUMIDOR FINAL', 890.75, 2, 5),
('2024-01-15 17:00:00', '7788990', 'ESCUELA MARÍA INMACULADA', 1680.00, 2, 5),

-- Establecimiento 6 (XELA006) - Serie F
('2024-01-16 10:30:00', 'CF', 'CONSUMIDOR FINAL', 1350.50, 2, 6),
('2024-01-16 14:15:00', '9988776', 'ASOCIACIÓN DEPORTIVA XELA', 3200.00, 2, 6),

-- Establecimiento 7 (XELA007) - Serie G
('2024-01-17 11:00:00', 'CF', 'CONSUMIDOR FINAL', 720.25, 2, 7),
('2024-01-17 16:45:00', '5544332', 'CENTRO DEPORTIVO MUNICIPAL', 1950.75, 2, 7),

-- Establecimiento 8 (XELA008) - Serie H
('2024-01-18 08:45:00', 'CF', 'CONSUMIDOR FINAL', 980.00, 2, 8),
('2024-01-18 13:20:00', '6655443', 'LIGA DEPORTIVA ALTOS', 2450.50, 2, 8),

-- Establecimiento 9 (XELA009) - Serie I
('2024-01-19 09:30:00', 'CF', 'CONSUMIDOR FINAL', 1150.25, 2, 9),
('2024-01-19 15:10:00', '2233445', 'ACADEMIA DE TENIS XELA', 1850.00, 2, 9),

-- Establecimiento 10 (XELA010) - Serie J
('2024-01-20 10:15:00', 'CF', 'CONSUMIDOR FINAL', 650.75, 2, 10),
('2024-01-20 16:30:00', '8877665', 'CLUB DE NATACIÓN XELA', 2750.00, 2, 10);


-- 4. Insertar Detalles de Facturas
-- Facturas Establecimiento
INSERT INTO DTE_Detalle (Nombre, Precio, Cantidad, FacturaId) VALUES
('Balón Fútbol Profesional Adidas', 350.00, 2, 1),
('Tenis Running Nike Revolution', 550.00, 1, 1),
('Balón Fútbol Training', 180.00, 3, 2),
('Conos Entrenamiento x20', 220.00, 2, 2),
('Chalecos Entrenamiento x12', 300.00, 1, 2),
('Mancuernas Acero 10kg', 450.50, 1, 3),
('Bandas Elasticas Resistencia', 125.00, 4, 3);


INSERT INTO DTE_Detalle (Nombre, Precio, Cantidad, FacturaId) VALUES
('Raqueta Tenis Wilson', 680.25, 1, 4),
('Pelotas Tenis x3', 120.00, 2, 5),
('Red Voleibol Profesional', 850.00, 1, 5),
('Balones Voleibol Mikasa', 180.25, 3, 5),
('Uniforme Deportivo Completo', 320.00, 2, 6),
('Zapatos Fútbol Sala', 280.00, 1, 7),
('Guantes Portero', 195.00, 2, 7),
('Rodilleras Protección', 85.75, 3, 8),
('Mochila Deportiva', 240.00, 1, 9),
('Bottles Agua 1L', 45.00, 4, 9);


-- 5. Insertar Certificaciones con series correlativas por establecimiento
INSERT INTO DTE_Certificacion (NumeroAutorizacion, Serie, Correlativo, FechaCertificacion, FacturaId) VALUES
-- Establecimiento 1 - Serie A
('AUT-XELA-2024-001-0001', 'A', 1, '2024-01-10 08:35:22', 1),
('AUT-XELA-2024-001-0002', 'A', 2, '2024-01-10 11:20:15', 2),
('AUT-XELA-2024-001-0003', 'A', 3, '2024-01-11 14:25:30', 3),

-- Establecimiento 2 - Serie B
('AUT-XELA-2024-002-0001', 'B', 1, '2024-01-12 09:50:45', 4),
('AUT-XELA-2024-002-0002', 'B', 2, '2024-01-12 16:35:20', 5),

-- Establecimiento 3 - Serie C
('AUT-XELA-2024-003-0001', 'C', 1, '2024-01-13 10:05:15', 6),
('AUT-XELA-2024-003-0002', 'C', 2, '2024-01-13 15:50:40', 7),

-- Establecimiento 4 - Serie D
('AUT-XELA-2024-004-0001', 'D', 1, '2024-01-14 08:20:35', 8),
('AUT-XELA-2024-004-0002', 'D', 2, '2024-01-14 12:35:25', 9),

-- Establecimiento 5 - Serie E
('AUT-XELA-2024-005-0001', 'E', 1, '2024-01-15 09:25:50', 10),
('AUT-XELA-2024-005-0002', 'E', 2, '2024-01-15 17:05:15', 11);


-- 6. Insertar algunas Anulaciones
INSERT INTO DTE_Anulacion (Motivo, FechaAnulacion, FacturaId) VALUES
('Producto con defecto de fábrica', '2024-01-16 11:30:00', 6),
('Cliente solicitó cambio de producto', '2024-01-18 14:15:00', 12);


-- Actualizar estados de facturas anuladas
UPDATE DTE_Factura SET Estado = 3 WHERE FacturaId IN (6, 12);


-- 7. Insertar Logs de Bitácora
INSERT INTO Logs_Bitacora (Accion, Descripcion, FacturaId) VALUES
('CREACION', 'Factura creada - Consumidor Final - DEPORTIVOS XELA CENTRO', 1),
('CERTIFICACION', 'Factura certificada - Serie A Correlativo 1', 1),
('CREACION', 'Factura creada - Academia Fútbol Xela Junior', 2),
('CERTIFICACION', 'Factura certificada - Serie A Correlativo 2', 2),
('ANULACION', 'Factura anulada por defecto de producto', 6),
('CREACION', 'Factura creada - Club Deportivo Xelajú', 9),
('CERTIFICACION', 'Factura certificada - Serie D Correlativo 2', 9);


-- ========================================
-- CONSULTAS DE VERIFICACIÓN
-- ========================================

-- Verificar emisores y establecimientos
SELECT 
    e.RazonSocial,
    e.Nit,
    COUNT(est.EstablecimientoId) as CantidadEstablecimientos,
    STRING_AGG(est.NombreEstablecimiento, ', ') as Establecimientos
FROM Emisor e
LEFT JOIN Establecimiento est ON e.EmisorId = est.EmisorId
GROUP BY e.RazonSocial, e.Nit
ORDER BY e.RazonSocial;
GO

-- Resumen de facturas por establecimiento
SELECT 
    est.NombreEstablecimiento,
    e.RazonSocial,
    COUNT(f.FacturaId) as TotalFacturas,
    SUM(CASE WHEN f.Estado = 2 THEN f.Total ELSE 0 END) as TotalCertificado,
    SUM(CASE WHEN f.Estado = 3 THEN f.Total ELSE 0 END) as TotalAnulado
FROM Establecimiento est
INNER JOIN Emisor e ON est.EmisorId = e.EmisorId
LEFT JOIN DTE_Factura f ON est.EstablecimientoId = f.EstablecimientoId
GROUP BY est.NombreEstablecimiento, e.RazonSocial
ORDER BY e.RazonSocial, est.NombreEstablecimiento;
GO

-- Verificar correlativos por serie
SELECT 
    c.Serie,
    c.Correlativo,
    f.FacturaId,
    est.NombreEstablecimiento,
    f.ClienteRazonSocial
FROM DTE_Certificacion c
INNER JOIN DTE_Factura f ON c.FacturaId = f.FacturaId
INNER JOIN Establecimiento est ON f.EstablecimientoId = est.EstablecimientoId
ORDER BY c.Serie, c.Correlativo;
