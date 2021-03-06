USE [master]
GO
/****** Object:  Database [ConsuPyme]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE DATABASE [ConsuPyme]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ConsuPyme', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.LUCAS1\MSSQL\DATA\ConsuPyme.mdf' , SIZE = 7168KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ConsuPyme_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.LUCAS1\MSSQL\DATA\ConsuPyme_log.ldf' , SIZE = 4672KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ConsuPyme] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ConsuPyme].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ConsuPyme] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ConsuPyme] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ConsuPyme] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ConsuPyme] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ConsuPyme] SET ARITHABORT OFF 
GO
ALTER DATABASE [ConsuPyme] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ConsuPyme] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ConsuPyme] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ConsuPyme] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ConsuPyme] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ConsuPyme] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ConsuPyme] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ConsuPyme] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ConsuPyme] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ConsuPyme] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ConsuPyme] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ConsuPyme] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ConsuPyme] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ConsuPyme] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ConsuPyme] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ConsuPyme] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ConsuPyme] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ConsuPyme] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ConsuPyme] SET RECOVERY FULL 
GO
ALTER DATABASE [ConsuPyme] SET  MULTI_USER 
GO
ALTER DATABASE [ConsuPyme] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ConsuPyme] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ConsuPyme] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ConsuPyme] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ConsuPyme', N'ON'
GO
USE [ConsuPyme]
GO
/****** Object:  StoredProcedure [dbo].[baja_despacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[baja_despacho](@id_despacho int)
as
delete fd from Factura_Despacho fd where fd.DespachoId=@id_despacho
delete d from Despacho d where Id=@id_despacho


GO
/****** Object:  StoredProcedure [dbo].[baja_factura]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[baja_factura]
(
@id_factura int
)
as
--delete Factura_Producto where FacturaId=@id_factura
--DELETE Factura_Despacho WHERE FacturaId=@id_factura
--delete ft FROM Factura_Total as ft inner join Factura as f on ft.Id=f.Factura_Total_Id where  f.Id=@id_factura
delete ftp FROM Factura_Total as ft inner join Factura as f on ft.Id=f.Factura_Total_Id inner join Factura_Total_Producto ftp on ft.Id=ftp.Factura_TotalId  where  f.Id=@id_factura
delete Factura where id=@id_factura

GO
/****** Object:  StoredProcedure [dbo].[cantidad_Producto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[cantidad_Producto](@Id int)
as
declare @cantidad int
set @cantidad=(select sum(ftp.Cantidad) from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id where d.id=@Id)

--DEPOSITO
declare @importe_deposito decimal
declare @total_deposito decimal
select @importe_deposito = Importe from Deposito where DespachoId=@Id
set @total_deposito=@importe_deposito/@cantidad

--ACARREO
declare @importe_acarreo decimal
declare @total_acarreo decimal
select @importe_acarreo = Importe from Acarreo where DespachoId=@Id
set @total_acarreo=@importe_acarreo/@cantidad

--DESPACHANTE
declare @ley decimal
declare @djai decimal
declare @ad_sim decimal
declare @gastos_despacho decimal
declare @desconsolidado decimal
declare @servicios decimal
declare @total_despachante decimal

select @ley=Ley,@djai=Djai,@ad_sim=AD_Sim,@gastos_despacho=Gastos_Despacho,@desconsolidado=Desconsolidado,@servicios=Servicios from Despachante where DespachoId=@Id
set @total_despachante=(@ley+@djai+@ad_sim+@gastos_despacho+@desconsolidado+@servicios)/@cantidad

--DESPACHO

declare @fob_total decimal
declare @flete_total decimal
declare @seguro_total decimal
declare @arancel_sim decimal
declare @servicio_guarda decimal
declare @total_despacho decimal

select @fob_total=Fob_Total,@flete_total=Flete_Total,@seguro_total=Seguro_Total,@arancel_sim=Arancel_Sim,@Servicio_Guarda=Servicio_Guarda from Despacho where Id=@Id
set @total_despacho=(@fob_total+@flete_total+@seguro_total+@arancel_sim+@servicio_guarda)/@cantidad

--TOTAL
declare @total decimal
set @total=@total_despacho+@total_acarreo+@total_deposito+@total_despachante

--select Cantidad+@total from Factura_Total_Producto
--select Cantidad*Precio_Unitario+@total from Factura_Total_Producto --WHERE 


select p.Descripcion as producto,(ftp.Cantidad*ftp.Precio_Unitario)+@total as total from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id where d.id=@Id

--exec cantidad_Producto 6
GO
/****** Object:  StoredProcedure [dbo].[listado_acarreo]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[listado_acarreo] (@dc nvarchar(max))
as
if (@dc is null )
begin
select d.Id,d.Dc,d.Cotizacion,d.Codigo_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Acarreo de on d.Id=de.DespachoId where de.Id is null
end
else
begin
select d.Id,d.Dc,d.Cotizacion,d.Codigo_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Acarreo  de on d.Id=de.DespachoId where de.Id is null 
and d.Dc like '%'+@dc+'%'
end
GO
/****** Object:  StoredProcedure [dbo].[listado_deposito]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[listado_deposito] (@dc nvarchar(max))
as
if (@dc is null )
begin
select d.Id,d.Dc,d.Cotizacion,d.Codigo_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Deposito  de on d.Id=de.DespachoId where de.Id is null
end
else
begin
select d.Id,d.Dc,d.Cotizacion,d.Codigo_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Deposito  de on d.Id=de.DespachoId where de.Id is null 
and d.Dc like '%'+@dc+'%'
end
GO
/****** Object:  StoredProcedure [dbo].[listado_despachante]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[listado_despachante] (@dc nvarchar(max))
as
if (@dc is null )
begin
select d.Id,d.Dc,d.Cotizacion,d.Codigo_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Despachante de on d.Id=de.DespachoId where de.Id is null
end
else
begin
select d.Id,d.Dc,d.Cotizacion,d.Codigo_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Despachante  de on d.Id=de.DespachoId where de.Id is null 
and d.Dc like '%'+@dc+'%'
end
GO
/****** Object:  StoredProcedure [dbo].[listado_factura_producto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[listado_factura_producto]
as
select distinct p.Codigo,ftp.Factura_TotalId as 'id factura total',p.id as 'id producto',ftp.Precio_Unitario,ftp.Cantidad,ftp.Num_Lote,p.Descripcion from Producto p left join Factura_Total_Producto ftp on p.Id=ftp.ProductoId --where ftp. is not null
GO
/****** Object:  StoredProcedure [dbo].[listado_Facturas]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[listado_Facturas](@nombre varchar(max)=null)
as
if (@nombre is null)
begin
select ft.Id,ft.Creacion,ft.Vencimiento_Factura,ft.Nombre,ft.Numero_Factura from Factura_Total ft inner join Factura f on ft.Id=f.Factura_Total_Id left join
Factura_Despacho fd on f.Id=fd.FacturaId where fd.FacturaId is null
end
else
begin
select ft.Id,ft.Creacion,ft.Vencimiento_Factura,ft.Nombre,ft.Numero_Factura from Factura_Total ft inner join Factura f on ft.Id=f.Factura_Total_Id left join
Factura_Despacho fd on f.Id=fd.FacturaId where fd.FacturaId is null and ft.Nombre like '%'+@nombre+'%'
end
GO
/****** Object:  StoredProcedure [dbo].[precio_Producto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[precio_Producto](@id int)
as 
select ftp.Precio_Unitario*ftp.Cantidad as Total,p.Codigo as Codigo from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id where d.id=@id
GO
/****** Object:  StoredProcedure [dbo].[Ver_Despacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[Ver_Despacho]
as

--VER
--acarreo y deposito
select distinct ft.Nombre,d.id  from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id inner join Acarreo a on d.Id=a.DespachoId inner join Deposito dep on d.Id=dep.DespachoId


GO
/****** Object:  Table [dbo].[Acarreo]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Acarreo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Importe] [decimal](18, 0) NOT NULL,
	[Numero_Factura] [nvarchar](max) NOT NULL,
	[DespachoId] [int] NOT NULL,
 CONSTRAINT [PK_Acarreo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Deposito]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deposito](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Importe] [decimal](18, 0) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Numero_Factura] [nvarchar](max) NOT NULL,
	[DespachoId] [int] NOT NULL,
 CONSTRAINT [PK_Deposito] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Despachante]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Despachante](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Numero_Factura] [nvarchar](max) NOT NULL,
	[Ley] [decimal](18, 0) NOT NULL,
	[Djai] [decimal](18, 0) NOT NULL,
	[AD_Sim] [decimal](18, 0) NOT NULL,
	[Gastos_Despacho] [decimal](18, 0) NOT NULL,
	[Desconsolidado] [decimal](18, 0) NOT NULL,
	[Servicios] [decimal](18, 0) NOT NULL,
	[DespachoId] [int] NOT NULL,
 CONSTRAINT [PK_Despachante] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Despacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Despacho](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Dc] [nvarchar](max) NOT NULL,
	[Oficializacion] [datetime] NOT NULL,
	[Fob_Total] [decimal](18, 0) NOT NULL,
	[Flete_Total] [decimal](18, 0) NOT NULL,
	[Seguro_Total] [decimal](18, 0) NOT NULL,
	[Cotizacion] [decimal](18, 0) NOT NULL,
	[Codigo_Afip] [nvarchar](max) NOT NULL,
	[Arancel_Sim] [decimal](18, 0) NOT NULL,
	[Servicio_Guarda] [decimal](18, 0) NOT NULL,
	[Tipo_CambioId] [int] NOT NULL,
 CONSTRAINT [PK_Despacho] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Factura]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura](
	[Flete] [decimal](18, 0) NULL,
	[Envalaje] [decimal](18, 0) NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Factura_Total_Id] [int] NOT NULL,
 CONSTRAINT [PK_Factura] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Factura_Despacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura_Despacho](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DespachoId] [int] NOT NULL,
	[FacturaId] [int] NOT NULL,
 CONSTRAINT [PK_Factura_Despacho] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Factura_Total]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura_Total](
	[Vencimiento_Factura] [datetime] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Creacion] [datetime] NOT NULL,
	[Numero_Factura] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Factura_Total] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Factura_Total_Producto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura_Total_Producto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Num_Lote] [nvarchar](max) NULL,
	[Cantidad] [int] NOT NULL,
	[Precio_Unitario] [decimal](18, 0) NOT NULL,
	[ProductoId] [int] NOT NULL,
	[Factura_TotalId] [int] NOT NULL,
 CONSTRAINT [PK_Factura_Total_Producto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Posicion_Arancelaria]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posicion_Arancelaria](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Numero_Posicion] [nvarchar](max) NOT NULL,
	[Porcentaje_Iva] [decimal](18, 0) NOT NULL,
	[Porcentaje_Taza_Estadistica] [decimal](18, 0) NOT NULL,
	[Porcentaje_Importacion] [decimal](18, 0) NOT NULL,
 CONSTRAINT [PK_Posicion_Arancelaria] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Producto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producto](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[Codigo] [nvarchar](max) NOT NULL,
	[Posicion_ArancelariaId] [int] NOT NULL,
 CONSTRAINT [PK_Producto] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tipo_Cambio]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tipo_Cambio](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cambio] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Tipo_Cambio] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Usuarios]    Script Date: 25/01/2015 06:26:09 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuarios](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](max) NOT NULL,
	[Clave] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Index [IX_FK_AcarreoDespacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_AcarreoDespacho] ON [dbo].[Acarreo]
(
	[DespachoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_DepositoDespacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_DepositoDespacho] ON [dbo].[Deposito]
(
	[DespachoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_DespachoDespachante]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_DespachoDespachante] ON [dbo].[Despachante]
(
	[DespachoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_DespachoTipo_Cambio]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_DespachoTipo_Cambio] ON [dbo].[Despacho]
(
	[Tipo_CambioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_FacturaFactura_Total]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_FacturaFactura_Total] ON [dbo].[Factura]
(
	[Factura_Total_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Factura_DespachoDespacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_Factura_DespachoDespacho] ON [dbo].[Factura_Despacho]
(
	[DespachoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_FacturaFactura_Despacho]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_FacturaFactura_Despacho] ON [dbo].[Factura_Despacho]
(
	[FacturaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Factura_Total_ProductoProducto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_Factura_Total_ProductoProducto] ON [dbo].[Factura_Total_Producto]
(
	[ProductoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Factura_TotalFactura_Total_Producto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_Factura_TotalFactura_Total_Producto] ON [dbo].[Factura_Total_Producto]
(
	[Factura_TotalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Posicion_ArancelariaProducto]    Script Date: 25/01/2015 06:26:09 p.m. ******/
CREATE NONCLUSTERED INDEX [IX_FK_Posicion_ArancelariaProducto] ON [dbo].[Producto]
(
	[Posicion_ArancelariaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Acarreo]  WITH CHECK ADD  CONSTRAINT [FK_AcarreoDespacho] FOREIGN KEY([DespachoId])
REFERENCES [dbo].[Despacho] ([Id])
GO
ALTER TABLE [dbo].[Acarreo] CHECK CONSTRAINT [FK_AcarreoDespacho]
GO
ALTER TABLE [dbo].[Deposito]  WITH CHECK ADD  CONSTRAINT [FK_DepositoDespacho] FOREIGN KEY([DespachoId])
REFERENCES [dbo].[Despacho] ([Id])
GO
ALTER TABLE [dbo].[Deposito] CHECK CONSTRAINT [FK_DepositoDespacho]
GO
ALTER TABLE [dbo].[Despachante]  WITH CHECK ADD  CONSTRAINT [FK_DespachoDespachante] FOREIGN KEY([DespachoId])
REFERENCES [dbo].[Despacho] ([Id])
GO
ALTER TABLE [dbo].[Despachante] CHECK CONSTRAINT [FK_DespachoDespachante]
GO
ALTER TABLE [dbo].[Despacho]  WITH CHECK ADD  CONSTRAINT [FK_DespachoTipo_Cambio] FOREIGN KEY([Tipo_CambioId])
REFERENCES [dbo].[Tipo_Cambio] ([Id])
GO
ALTER TABLE [dbo].[Despacho] CHECK CONSTRAINT [FK_DespachoTipo_Cambio]
GO
ALTER TABLE [dbo].[Factura]  WITH CHECK ADD  CONSTRAINT [FK_FacturaFactura_Total] FOREIGN KEY([Factura_Total_Id])
REFERENCES [dbo].[Factura_Total] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Factura] CHECK CONSTRAINT [FK_FacturaFactura_Total]
GO
ALTER TABLE [dbo].[Factura_Despacho]  WITH CHECK ADD  CONSTRAINT [FK_Factura_DespachoDespacho] FOREIGN KEY([DespachoId])
REFERENCES [dbo].[Despacho] ([Id])
GO
ALTER TABLE [dbo].[Factura_Despacho] CHECK CONSTRAINT [FK_Factura_DespachoDespacho]
GO
ALTER TABLE [dbo].[Factura_Despacho]  WITH CHECK ADD  CONSTRAINT [FK_FacturaFactura_Despacho] FOREIGN KEY([FacturaId])
REFERENCES [dbo].[Factura] ([Id])
GO
ALTER TABLE [dbo].[Factura_Despacho] CHECK CONSTRAINT [FK_FacturaFactura_Despacho]
GO
ALTER TABLE [dbo].[Factura_Total_Producto]  WITH CHECK ADD  CONSTRAINT [FK_Factura_Total_ProductoProducto] FOREIGN KEY([ProductoId])
REFERENCES [dbo].[Producto] ([Id])
GO
ALTER TABLE [dbo].[Factura_Total_Producto] CHECK CONSTRAINT [FK_Factura_Total_ProductoProducto]
GO
ALTER TABLE [dbo].[Factura_Total_Producto]  WITH CHECK ADD  CONSTRAINT [FK_Factura_TotalFactura_Total_Producto] FOREIGN KEY([Factura_TotalId])
REFERENCES [dbo].[Factura_Total] ([Id])
GO
ALTER TABLE [dbo].[Factura_Total_Producto] CHECK CONSTRAINT [FK_Factura_TotalFactura_Total_Producto]
GO
ALTER TABLE [dbo].[Producto]  WITH CHECK ADD  CONSTRAINT [FK_Posicion_ArancelariaProducto] FOREIGN KEY([Posicion_ArancelariaId])
REFERENCES [dbo].[Posicion_Arancelaria] ([Id])
GO
ALTER TABLE [dbo].[Producto] CHECK CONSTRAINT [FK_Posicion_ArancelariaProducto]
GO
USE [master]
GO
ALTER DATABASE [ConsuPyme] SET  READ_WRITE 
GO
