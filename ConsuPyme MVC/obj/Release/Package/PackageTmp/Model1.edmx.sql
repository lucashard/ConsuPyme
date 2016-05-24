
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/29/2013 00:13:32
-- Generated from EDMX file: C:\Users\Lucas\Documents\Visual Studio 2012\Projects\ConsuPyme MVC\ConsuPyme MVC\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ConsuPyme];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AcarreoProducto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Acarreo] DROP CONSTRAINT [FK_AcarreoProducto];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductoDeposito]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Deposito] DROP CONSTRAINT [FK_ProductoDeposito];
GO
IF OBJECT_ID(N'[dbo].[FK_Factura_DespachoDespacho]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Factura_Despacho] DROP CONSTRAINT [FK_Factura_DespachoDespacho];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductoDespacho]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Despacho] DROP CONSTRAINT [FK_ProductoDespacho];
GO
IF OBJECT_ID(N'[dbo].[FK_FacturaFactura_Despacho]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Factura_Despacho] DROP CONSTRAINT [FK_FacturaFactura_Despacho];
GO
IF OBJECT_ID(N'[dbo].[FK_FacturaFactura_Total]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Factura] DROP CONSTRAINT [FK_FacturaFactura_Total];
GO
IF OBJECT_ID(N'[dbo].[FK_Posicion_ArancelariaProducto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Producto] DROP CONSTRAINT [FK_Posicion_ArancelariaProducto];
GO
IF OBJECT_ID(N'[dbo].[FK_Factura_Total_ProductoProducto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Factura_Total_Producto] DROP CONSTRAINT [FK_Factura_Total_ProductoProducto];
GO
IF OBJECT_ID(N'[dbo].[FK_Factura_TotalFactura_Total_Producto]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Factura_Total_Producto] DROP CONSTRAINT [FK_Factura_TotalFactura_Total_Producto];
GO
IF OBJECT_ID(N'[dbo].[FK_DespachoDespachante]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Despachante] DROP CONSTRAINT [FK_DespachoDespachante];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Acarreo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Acarreo];
GO
IF OBJECT_ID(N'[dbo].[Deposito]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Deposito];
GO
IF OBJECT_ID(N'[dbo].[Despachante]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Despachante];
GO
IF OBJECT_ID(N'[dbo].[Despacho]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Despacho];
GO
IF OBJECT_ID(N'[dbo].[Factura]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Factura];
GO
IF OBJECT_ID(N'[dbo].[Factura_Despacho]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Factura_Despacho];
GO
IF OBJECT_ID(N'[dbo].[Factura_Total]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Factura_Total];
GO
IF OBJECT_ID(N'[dbo].[Posicion_Arancelaria]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posicion_Arancelaria];
GO
IF OBJECT_ID(N'[dbo].[Producto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Producto];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Factura_Total_Producto]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Factura_Total_Producto];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Acarreo'
CREATE TABLE [dbo].[Acarreo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Importe] decimal(18,0)  NOT NULL,
    [Numero_Factura] bigint  NOT NULL,
    [DespachoId] int  NOT NULL
);
GO

-- Creating table 'Deposito'
CREATE TABLE [dbo].[Deposito] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Importe] decimal(18,0)  NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Numero_Factura] nvarchar(max)  NOT NULL,
    [DespachoId] int  NOT NULL
);
GO

-- Creating table 'Despachante'
CREATE TABLE [dbo].[Despachante] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Numero_Factura] bigint  NOT NULL,
    [Ley] decimal(18,0)  NOT NULL,
    [Djai] decimal(18,0)  NOT NULL,
    [AD_Sim] decimal(18,0)  NOT NULL,
    [Gastos_Despacho] decimal(18,0)  NOT NULL,
    [Desconsolidado] decimal(18,0)  NOT NULL,
    [Servicios] decimal(18,0)  NOT NULL,
    [DespachoId] int  NOT NULL
);
GO

-- Creating table 'Despacho'
CREATE TABLE [dbo].[Despacho] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Costo_despacho_producto] decimal(18,0)  NOT NULL,
    [Dc] nvarchar(max)  NOT NULL,
    [Oficializacion] datetime  NOT NULL,
    [Fob_Total] decimal(18,0)  NOT NULL,
    [Flete_Total] decimal(18,0)  NOT NULL,
    [Seguro_Total] decimal(18,0)  NOT NULL,
    [Cotizacion] decimal(18,0)  NOT NULL,
    [Codigo_Afip] nvarchar(max)  NOT NULL,
    [Arancel_Sim] decimal(18,0)  NOT NULL,
    [Servicio_Guarda] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Factura'
CREATE TABLE [dbo].[Factura] (
    [Flete] decimal(18,0)  NULL,
    [Envalaje] decimal(18,0)  NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Factura_Total_Id] int  NOT NULL
);
GO

-- Creating table 'Factura_Despacho'
CREATE TABLE [dbo].[Factura_Despacho] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DespachoId] int  NOT NULL,
    [FacturaId] int  NOT NULL
);
GO

-- Creating table 'Factura_Total'
CREATE TABLE [dbo].[Factura_Total] (
    [N_Orden] nvarchar(max)  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Creacion] datetime  NOT NULL,
    [Numero_Factura] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Posicion_Arancelaria'
CREATE TABLE [dbo].[Posicion_Arancelaria] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Numero_Posicion] bigint  NOT NULL,
    [Porcentaje_Iva] decimal(18,0)  NOT NULL,
    [Porcentaje_Taza_Estadistica] decimal(18,0)  NOT NULL,
    [Porcentaje_Importacion] decimal(18,0)  NOT NULL
);
GO

-- Creating table 'Producto'
CREATE TABLE [dbo].[Producto] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Descripcion] nvarchar(max)  NOT NULL,
    [Codigo] nvarchar(max)  NOT NULL,
    [Posicion_ArancelariaId] int  NOT NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(max)  NOT NULL,
    [Clave] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Factura_Total_Producto'
CREATE TABLE [dbo].[Factura_Total_Producto] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Num_Lote] nvarchar(max)  NULL,
    [Cantidad] int  NOT NULL,
    [Precio_Unitario] decimal(18,0)  NOT NULL,
    [ProductoId] int  NOT NULL,
    [Factura_TotalId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Acarreo'
ALTER TABLE [dbo].[Acarreo]
ADD CONSTRAINT [PK_Acarreo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Deposito'
ALTER TABLE [dbo].[Deposito]
ADD CONSTRAINT [PK_Deposito]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Despachante'
ALTER TABLE [dbo].[Despachante]
ADD CONSTRAINT [PK_Despachante]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Despacho'
ALTER TABLE [dbo].[Despacho]
ADD CONSTRAINT [PK_Despacho]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Factura'
ALTER TABLE [dbo].[Factura]
ADD CONSTRAINT [PK_Factura]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Factura_Despacho'
ALTER TABLE [dbo].[Factura_Despacho]
ADD CONSTRAINT [PK_Factura_Despacho]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Factura_Total'
ALTER TABLE [dbo].[Factura_Total]
ADD CONSTRAINT [PK_Factura_Total]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Posicion_Arancelaria'
ALTER TABLE [dbo].[Posicion_Arancelaria]
ADD CONSTRAINT [PK_Posicion_Arancelaria]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Producto'
ALTER TABLE [dbo].[Producto]
ADD CONSTRAINT [PK_Producto]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Factura_Total_Producto'
ALTER TABLE [dbo].[Factura_Total_Producto]
ADD CONSTRAINT [PK_Factura_Total_Producto]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [DespachoId] in table 'Factura_Despacho'
ALTER TABLE [dbo].[Factura_Despacho]
ADD CONSTRAINT [FK_Factura_DespachoDespacho]
    FOREIGN KEY ([DespachoId])
    REFERENCES [dbo].[Despacho]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Factura_DespachoDespacho'
CREATE INDEX [IX_FK_Factura_DespachoDespacho]
ON [dbo].[Factura_Despacho]
    ([DespachoId]);
GO

-- Creating foreign key on [FacturaId] in table 'Factura_Despacho'
ALTER TABLE [dbo].[Factura_Despacho]
ADD CONSTRAINT [FK_FacturaFactura_Despacho]
    FOREIGN KEY ([FacturaId])
    REFERENCES [dbo].[Factura]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FacturaFactura_Despacho'
CREATE INDEX [IX_FK_FacturaFactura_Despacho]
ON [dbo].[Factura_Despacho]
    ([FacturaId]);
GO

-- Creating foreign key on [Factura_Total_Id] in table 'Factura'
ALTER TABLE [dbo].[Factura]
ADD CONSTRAINT [FK_FacturaFactura_Total]
    FOREIGN KEY ([Factura_Total_Id])
    REFERENCES [dbo].[Factura_Total]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FacturaFactura_Total'
CREATE INDEX [IX_FK_FacturaFactura_Total]
ON [dbo].[Factura]
    ([Factura_Total_Id]);
GO

-- Creating foreign key on [Posicion_ArancelariaId] in table 'Producto'
ALTER TABLE [dbo].[Producto]
ADD CONSTRAINT [FK_Posicion_ArancelariaProducto]
    FOREIGN KEY ([Posicion_ArancelariaId])
    REFERENCES [dbo].[Posicion_Arancelaria]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Posicion_ArancelariaProducto'
CREATE INDEX [IX_FK_Posicion_ArancelariaProducto]
ON [dbo].[Producto]
    ([Posicion_ArancelariaId]);
GO

-- Creating foreign key on [ProductoId] in table 'Factura_Total_Producto'
ALTER TABLE [dbo].[Factura_Total_Producto]
ADD CONSTRAINT [FK_Factura_Total_ProductoProducto]
    FOREIGN KEY ([ProductoId])
    REFERENCES [dbo].[Producto]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Factura_Total_ProductoProducto'
CREATE INDEX [IX_FK_Factura_Total_ProductoProducto]
ON [dbo].[Factura_Total_Producto]
    ([ProductoId]);
GO

-- Creating foreign key on [Factura_TotalId] in table 'Factura_Total_Producto'
ALTER TABLE [dbo].[Factura_Total_Producto]
ADD CONSTRAINT [FK_Factura_TotalFactura_Total_Producto]
    FOREIGN KEY ([Factura_TotalId])
    REFERENCES [dbo].[Factura_Total]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Factura_TotalFactura_Total_Producto'
CREATE INDEX [IX_FK_Factura_TotalFactura_Total_Producto]
ON [dbo].[Factura_Total_Producto]
    ([Factura_TotalId]);
GO

-- Creating foreign key on [DespachoId] in table 'Despachante'
ALTER TABLE [dbo].[Despachante]
ADD CONSTRAINT [FK_DespachoDespachante]
    FOREIGN KEY ([DespachoId])
    REFERENCES [dbo].[Despacho]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DespachoDespachante'
CREATE INDEX [IX_FK_DespachoDespachante]
ON [dbo].[Despachante]
    ([DespachoId]);
GO

-- Creating foreign key on [DespachoId] in table 'Deposito'
ALTER TABLE [dbo].[Deposito]
ADD CONSTRAINT [FK_DepositoDespacho]
    FOREIGN KEY ([DespachoId])
    REFERENCES [dbo].[Despacho]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepositoDespacho'
CREATE INDEX [IX_FK_DepositoDespacho]
ON [dbo].[Deposito]
    ([DespachoId]);
GO

-- Creating foreign key on [DespachoId] in table 'Acarreo'
ALTER TABLE [dbo].[Acarreo]
ADD CONSTRAINT [FK_AcarreoDespacho]
    FOREIGN KEY ([DespachoId])
    REFERENCES [dbo].[Despacho]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AcarreoDespacho'
CREATE INDEX [IX_FK_AcarreoDespacho]
ON [dbo].[Acarreo]
    ([DespachoId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------