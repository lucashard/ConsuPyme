
CREATE PROC [dbo].[baja_despacho](@id_despacho int)
as
delete fd from Factura_Despacho fd where fd.DespachoId=@id_despacho
delete d from Deposito d where d.DespachoId=@id_despacho
delete a from Acarreo a where a.DespachoId=@id_despacho
delete d from Despachante d where d.DespachoId=@id_despacho
delete d from Despacho d where Id=@id_despacho



/****** Object:  StoredProcedure [dbo].[baja_factura]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[baja_factura]
(
@id_factura int
)
as
--ES 1
--delete Factura_Producto where FacturaId=@id_factura
--DELETE Factura_Despacho WHERE FacturaId=@id_factura
--delete ft FROM Factura_Total as ft inner join Factura as f on ft.Id=f.Factura_Total_Id where  f.Id=@id_factura

--delete ftp FROM Factura_Total as ft inner join Factura as f on ft.Id=f.Factura_Total_Id inner join Factura_Total_Producto ftp on ft.Id=ftp.Factura_TotalId  where  f.Id=@id_factura
--delete Factura where id=@id_factura

declare @id_factura_total int
declare @id_Fact int

--select * from Factura
--select * from Factura_Total--ACA PEGA EL ID 1

--select * from Factura where Factura.Factura_Total_Id=1

begin try
set @id_factura_total=(select ft.Id from Factura_Total ft where ft.Id=@id_factura)
set @id_Fact=(select f.id from Factura f where f.Factura_Total_Id=@id_factura_total)


delete f from Factura_Total_Producto as f where f.Factura_TotalId=@id_factura_total
delete fact_desp from Factura_Despacho fact_desp where fact_desp.FacturaId=@id_Fact
delete f from Factura as f where f.Factura_Total_Id=@id_factura_total
delete ft from Factura_Total as ft where ft.id=@id_factura
end try
begin catch
print N'error'
end catch

/****** Object:  StoredProcedure [dbo].[baja_pocicion_arancelaria]    Script Date: 18/12/2015 12:19:14 a.m. ******/




create proc [dbo].[baja_pocicion_arancelaria]
(
@id_pos int
)
as
delete pos from Posicion_Arancelaria pos where pos.Id=@id_pos

/****** Object:  StoredProcedure [dbo].[baja_producto]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[baja_producto]
(
@id_producto int
)
as
delete p from Producto p where p.Id=@id_producto


/****** Object:  StoredProcedure [dbo].[borrar_acarreo]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[borrar_acarreo](@id_acarreo int)
as
delete a from Acarreo a where a.Id=@id_acarreo

/****** Object:  StoredProcedure [dbo].[borrar_deposito]    Script Date: 18/12/2015 12:19:14 a.m. ******/




create proc [dbo].[borrar_deposito](@id_deposito int)
as
delete d from Deposito d where d.Id=@id_deposito

/****** Object:  StoredProcedure [dbo].[borrar_despachante]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[borrar_despachante](@id_depachante int)
as
delete desp from Despachante desp where desp.Id=@id_depachante

/****** Object:  StoredProcedure [dbo].[cantidad_Producto]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[cantidad_Producto](@Id int,@dolar decimal)
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
set @total_deposito=(@importe_deposito/@cantidad)/@dolar
--@total_deposito=@total_deposito/@dolar

--ACARREO
declare @importe_acarreo decimal
declare @total_acarreo decimal
select @importe_acarreo = Importe from Acarreo where DespachoId=@Id
set @total_acarreo=(@importe_acarreo/@cantidad)/@dolar

--DESPACHANTE
declare @ley decimal
declare @djai decimal
declare @ad_sim decimal
declare @gastos_despacho decimal
declare @desconsolidado decimal
declare @servicios decimal
declare @total_despachante decimal

select @ley=Ley,@djai=Djai,@ad_sim=AD_Sim,@gastos_despacho=Gastos_Despacho,@desconsolidado=Desconsolidado,@servicios=Servicios from Despachante where DespachoId=@Id
set @total_despachante=(@ley+@djai+@ad_sim+@gastos_despacho+@desconsolidado+@servicios)/@cantidad--)/@dolar--preguntar a NORMA

--DESPACHO

declare @fob_total decimal
declare @flete_total decimal
declare @seguro_total decimal
declare @arancel_sim decimal
declare @servicio_guarda decimal
declare @total_despacho decimal

select @flete_total=Flete_Total,@seguro_total=Seguro_Total,@arancel_sim=Arancel_Sim,@Servicio_Guarda=Servicio_Guarda from Despacho where Id=@Id
set @total_despacho=(@flete_total+@seguro_total+@arancel_sim+@servicio_guarda)/@cantidad

--TOTAL
declare @total decimal
set @total=@total_despacho+@total_acarreo+@total_deposito+@total_despachante

--select Cantidad+@total from Factura_Total_Producto
--select Cantidad*Precio_Unitario+@total from Factura_Total_Producto --WHERE 


select p.Descripcion as producto,(ftp.Precio_Unitario)+@total as total,ft.Nombre from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id where d.id=@Id


/****** Object:  StoredProcedure [dbo].[listado_acarreo]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[listado_acarreo] (@dc nvarchar(max))
as
if (@dc is null or @dc='' )
begin
--select d.Id,d.Dc,d.Cotizacion,d.Codi_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Acarreo de on d.Id=de.DespachoId where de.Id is null
select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codigo_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho
--where Despacho.Id not in (select DespachoId from Acarreo)
end
else
begin
--select d.Id,d.Dc,d.Cotizacion,d.Codi_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Acarreo  de on d.Id=de.DespachoId where de.Id is null 
--and d.Dc like '%'+@dc+'%'
select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codigo_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho
where Despacho.Id not in (select DespachoId from Acarreo) and Despacho.Dc like '%'+@dc+'%'
end

/****** Object:  StoredProcedure [dbo].[listado_deposito]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[listado_deposito] (@dc nvarchar(max))
as
if (@dc is null or @dc='' )
begin
--select d.Id,d.Dc,d.Cotizacion,d.Codi_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Deposito  de on d.Id=de.DespachoId where de.Id is null
select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codigo_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho
--where Despacho.Id not in (select DespachoId from Deposito)
end
else
begin
--select d.Id,d.Dc,d.Cotizacion,d.Codi_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Deposito  de on d.Id=de.DespachoId where de.Id is null 
--and d.Dc like '%'+@dc+'%'
select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codigo_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho
where Despacho.Id not in (select DespachoId from Deposito) and Despacho.Dc like '%'+@dc+'%'

end

/****** Object:  StoredProcedure [dbo].[listado_despachante]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[listado_despachante] (@dc nvarchar(max))
as
if (@dc is null or @dc='' )
begin
--select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codi_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho 
--where Despacho.Id not in (select Id from Despachante)
select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codigo_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho
--where Despacho.Id not in (select DespachoId from Despachante)
end
else
begin
--select d.Id,d.Dc,d.Cotizacion,d.Codi_Afip,d.Arancel_Sim,d.Flete_Total,d.Oficializacion,d.Fob_Total,d.Seguro_Total,d.Servicio_Guarda from Despacho d left join Despachante  de on d.Id=de.DespachoId where de.Id is null 
--and d.Dc like '%'+@dc+'%'
--select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codi_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho 
--where Despacho.Id not in (select Id from Despachante) and Despacho.Dc like '%'+@dc+'%'
select Despacho.Id,Despacho.Dc,Despacho.Cotizacion,Despacho.Codigo_Afip,Despacho.Arancel_Sim,Despacho.Flete_Total,Despacho.Oficializacion,Despacho.Fob_Total,Despacho.Seguro_Total,Despacho.Servicio_Guarda from Despacho
where Despacho.Id not in (select DespachoId from Despachante) and Despacho.Dc like '%'+@dc+'%'
end

/****** Object:  StoredProcedure [dbo].[listado_factura_producto]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[listado_factura_producto]
as
select distinct p.Codigo,ftp.Factura_TotalId as 'id factura total',p.id as 'id producto',ftp.Precio_Unitario,ftp.Cantidad,ftp.Num_Lote,p.Descripcion from Producto p left join Factura_Total_Producto ftp on p.Id=ftp.ProductoId --where ftp. is not null

/****** Object:  StoredProcedure [dbo].[listado_Facturas]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[listado_Facturas](@nombre varchar(max)=null)
as
if (@nombre is null or @nombre='')
begin
--select ft.Id,ft.Creacion,ft.Vencimiento_Factura,ft.Nombre,ft.Numero_Factura from Factura_Total ft inner join Factura f on ft.Id=f.Factura_Total_Id left join
--Factura_Despacho fd on f.Id=fd.FacturaId --where fd.FacturaId is null
select ft.Id,ft.Creacion,ft.Vencimiento_Factura,ft.Nombre,ft.Numero_Factura from Factura_Total ft inner join Factura f on ft.Id=f.Factura_Total_Id 
end
else
begin
--select ft.Id,ft.Creacion,ft.Vencimiento_Factura,ft.Nombre,ft.Numero_Factura from Factura_Total ft inner join Factura f on ft.Id=f.Factura_Total_Id left join
--Factura_Despacho fd on f.Id=fd.FacturaId where fd.FacturaId is null and ft.Nombre like '%'+@nombre+'%'
select ft.Id,ft.Creacion,ft.Vencimiento_Factura,ft.Nombre,ft.Numero_Factura from Factura_Total ft inner join Factura f on ft.Id=f.Factura_Total_Id 
where ft.Nombre like '%'+@nombre+'%'

end

/****** Object:  StoredProcedure [dbo].[precio_Producto]    Script Date: 18/12/2015 12:19:14 a.m. ******/




create proc [dbo].[precio_Producto](@id int)
as 
select ftp.Precio_Unitario*ftp.Cantidad as Total,p.Codigo as Codigo from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id where d.id=@id

/****** Object:  StoredProcedure [dbo].[producto_Fechas]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[producto_Fechas](@FechaDesde datetime,@FechaHasta datetime)
as
select f.Id,ft.Creacion from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id 
where ft.Creacion>=@FechaDesde and ft.Creacion<=@FechaHasta





/****** Object:  StoredProcedure [dbo].[Ver_Despacho]    Script Date: 18/12/2015 12:19:14 a.m. ******/




CREATE proc [dbo].[Ver_Despacho]
as

--VER
--acarreo y deposito
select distinct ft.Nombre,d.id  from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id inner join Acarreo a on d.Id=a.DespachoId inner join Deposito dep on d.Id=dep.DespachoId
inner join Despachante desp on desp.DespachoId=d.Id

/****** Object:  StoredProcedure [dbo].[Ver_Despacho_Fecha]    Script Date: 18/12/2015 12:19:14 a.m. ******/




create procedure [dbo].[Ver_Despacho_Fecha]
@id int
as

--VER
--acarreo y deposito
select distinct ft.creacion from Despacho d inner join Factura_Despacho fd on d.Id=fd.DespachoId
inner join Factura f on fd.FacturaId=f.Id inner join Factura_Total ft
on f.Factura_Total_Id=ft.Id inner join Factura_Total_Producto ftp 
on ft.Id=ftp.Factura_TotalId inner join Producto p
on ftp.ProductoId=p.Id inner join Acarreo a on d.Id=a.DespachoId inner join Deposito dep on d.Id=dep.DespachoId
inner join Despachante desp on desp.DespachoId=d.Id
where d.id=@id





/****** Object:  StoredProcedure [dbo].[Ver_Facturas]    Script Date: 18/12/2015 12:19:14 a.m. ******/




create proc [dbo].[Ver_Facturas] as 
select ft.Nombre,ft.Numero_Factura,ft.Vencimiento_Factura,fd.Id 
from Factura_Despacho fd inner join Factura f on fd.FacturaId=f.Id right join Factura_Total ft on f.Factura_Total_Id=ft.id


