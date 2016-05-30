using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;

namespace ConsuPyme_MVC.Controllers
{
    public class FacturaController : Controller
    {
        //
        // GET: /Factura/
        private static List<Grilla> selectList = new List<Grilla>();

        private IFacturas _Facturas;
        public FacturaController(IFacturas factura)
        {
            _Facturas = factura;
        }

        [HttpPost]
        public ActionResult CambioGrila(String id, String lote, String precio,String cantidad)
        {
            var id1 = Convert.ToInt32(id);
            var seleccion = Prod.Single(x => x.Id == id1);
            seleccion.Num_Lote = lote;
            seleccion.Precio_Unitario = Decimal.Parse(precio.Replace(".", ","), NumberStyles.AllowDecimalPoint);
            seleccion.Cantidad= Convert.ToInt32(cantidad);
            seleccion.productos.Visible = true;
            decimal total = Convert.ToDecimal(seleccion.Precio_Unitario * seleccion.Cantidad);
            ViewBag.Total = total > 0 ? "El total es:" + total.ToString("F") : string.Empty;
            return Content(total.ToString("F"));
            
        }


        [HttpPost]
        public ActionResult Select(bool isChecked,String id,String lote,String precio,String cantidad, Grilla Grilla1)
        {
            if (isChecked && !selectList.Contains(Grilla1))
            {
                selectList.Add(Grilla1);
                int id1 = Convert.ToInt32(id);
                Prod.Single(x => x.Id == id1).Visible = true;
                Prod.Single(x => x.Id == id1).Num_Lote = lote;
                Prod.Single(x => x.Id == id1).Cantidad = Convert.ToInt32(cantidad);
                Prod.Single(x => x.Id == id1).Precio_Unitario = Decimal.Parse(precio.Replace(".", ","), NumberStyles.AllowDecimalPoint);
                Prod.Single(x => x.Id == id1).Total= Convert.ToInt32(cantidad)*Convert.ToDecimal(precio);
                Prod.Single(x => x.Id == id1).productos.Visible= true;
            }
            else if (!isChecked && selectList.Contains(Grilla1))
            {
                selectList.RemoveAll(s => s.id == Grilla1.id);
                int id1 = Convert.ToInt32(id);
                Prod.Single(x => x.Id == id1).Visible = false;
                Prod.Single(x => x.Id == id1).Num_Lote = string.Empty;
                Prod.Single(x => x.Id == id1).Cantidad = Convert.ToInt32(0);
                Prod.Single(x => x.Id == id1).Precio_Unitario = Convert.ToDecimal(0);
            }
            HttpContext.Session["SelectList"] = selectList;

            decimal total = selectList.Sum(elemento => Decimal.Parse(elemento.precio.Replace(".", ","), NumberStyles.AllowDecimalPoint) * Convert.ToDecimal(elemento.cantidad));
            ViewBag.Total = total > 0 ? "El total es:  " + total.ToString("F") : string.Empty;
            
            return Content(total.ToString("F"));
        }

        public ActionResult Index()
        {
            var fact = _Facturas.grid();
            selectList.Clear();
            return View(fact);
        }
       
        //
        // GET: /Factura/Create
        [HttpGet]
        public ActionResult Create(int ? page)
        {
                try
                {
                    Prod=new List<Facturas_Totales_Productos>();
                    ViewData["SelectList"] = HttpContext.Session["SelectList"] ?? selectList;
                    var obj=_Facturas.creacion();
                    var lista = obj;
                    var query=_Facturas.creacion().lista_Productos;
                    TraerGrilla(query);
                    foreach (var elem in query)
                    {
                        elem.Num_Lote = elem.Num_Lote ?? string.Empty;
                        Prod.Add(new Facturas_Totales_Productos(){Cantidad = Convert.ToInt32(elem.Cantidad),Num_Lote = elem.Num_Lote,Precio_Unitario = elem.Precio_Unitario,Id = elem.Id,productos = elem,Codigo = elem.Codigo,Descripcion = elem.Descripcion,Visible = false});
                    }
                    ViewBag.productos = Prod;
                    ViewBag.Vencimientos = new SelectList(new List<Vencimiento>() { new Vencimiento() { Id = 30, Vencimientos = "30" }, new Vencimiento() { Id = 60, Vencimientos = "60" }, new Vencimiento() { Id = 90, Vencimientos = "90" }, new Vencimiento() { Id = 120, Vencimientos = "120" }, new Vencimiento() { Id = 150, Vencimientos = "150" }, new Vencimiento() { Id = 200, Vencimientos = "200" }, new Vencimiento() { Id = 300, Vencimientos = "300" } }, "Id", "Vencimientos", "SELECCIONE");
                    
                    ViewBag.Proveedor = new SelectList(_Facturas.ObtenerProveedores(), "Id", "Nombre", "SELECCIONE");

                    if (Request.IsAjaxRequest())
                    {
                        return PartialView("Grilla");
                    }
                    return View(lista);
                }
                catch
                {
                    return View();
                }
        }

        

        private static void TraerGrilla(List<Productos> query)
        {
            foreach (var elemento in selectList)
            {
                foreach (var elemento1 in query)
                {
                    if (elemento.id.Equals(elemento1.Id.ToString()))
                    {
                        elemento1.Visible = true;
                        elemento1.Precio_Unitario = Convert.ToDecimal(elemento.precio);
                        elemento1.Cantidad = Convert.ToInt32(elemento.cantidad);
                        elemento1.Total = elemento1.Cantidad*elemento1.Precio_Unitario;
                    }
                }
            }
        }

        //
        // POST: /Factura/Create

        [HttpGet]
        public ActionResult Create1(string bus)
        {
                try
                {
                    var lista = _Facturas.ObtenerGrilla(bus.Trim());
                    var query = lista.lista_Productos;
                    TraerGrilla(query);
                    ViewBag.Productos = query;
                    return PartialView("Grilla");
                    
                }
                catch
                {
                    return View();
                }
                //var lista1 = _Facturas.ObtenerGrilla(bus);
                //return View("Create", lista1);

        }

        [HttpPost]
        public ActionResult ObtenerFecha(string fecha, int valor)
        {
            try
            {
                var fecha1 = DateTime.ParseExact(fecha, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                fecha1 = fecha1.AddDays(valor);
                return Content(fecha1.ToString("dd/MM/yyyy"));
            }
            catch 
            {
                return Content(string.Empty);
            }
        }
        
        [HttpGet]
        [ActionName("Busqueda")]
        public ActionResult Busqueda(string bus)
        {
            try
            {
                var lista = _Facturas.ObtenerGrilla(bus.Trim());
                var query = lista.lista_Productos;
                TraerGrilla(query);
                var l = new List<Facturas_Totales_Productos>();
                foreach (var ele in lista.lista_Productos.OrderBy(P => P.Visible))
                {
                    l.Add(new Facturas_Totales_Productos(){Cantidad = Convert.ToInt32(ele.Cantidad),Precio_Unitario = Convert.ToDecimal(ele.Precio_Unitario),Num_Lote = ele.Num_Lote,Producto_Id = ele.Id,productos = new Productos(){Id = ele.Id,Cantidad = Convert.ToInt32(ele.Cantidad),Codigo = ele.Codigo,Precio_Unitario = Convert.ToDecimal(ele.Precio_Unitario),Num_Lote = ele.Num_Lote,Visible = ele.Visible,Descripcion = ele.Descripcion}});
                }
                if (string.Empty.Equals(bus))
                {
                    foreach (Grilla elemGrilla in selectList)
                    {
                        foreach (Facturas_Totales_Productos elemProductos in Prod)
                        {
                            if (Convert.ToInt32(elemGrilla.id) == elemProductos.productos.Id)
                            {
                                elemProductos.productos.Visible = true;
                                elemProductos.Cantidad = Convert.ToInt32(elemGrilla.cantidad);
                                elemProductos.Precio_Unitario = Convert.ToDecimal(elemGrilla.precio);
                                elemProductos.Num_Lote = elemGrilla.lote;
                                
                            }
                        }
                    }
                    l = Prod;
                }
                ViewBag.productos = l;
                return PartialView("GrillaEdicion");

            }
            catch
            {
                return View();
            }


        }


        [HttpPost]
        public ActionResult Create(Facturas idArray)
        {
            if (!selectList.Any())
            {
                ModelState.AddModelError("Productos", "Por favor seleccione lo/s producto de la factura");
            }
            if (ModelState.IsValid)
            {
                idArray.FacturasTotales.Vencimiento_Factura = idArray.FacturasTotales.Creacion.AddDays(idArray.Vencimientos );
                try
                {
                    idArray.FacturasTotales.ProveedorId = idArray.ProveedorId;
                    int id_factura = _Facturas.Alta(idArray.FacturasTotales);
                    idArray.Factura_Total_Id = id_factura;

                    _Facturas.Alta_Factura(idArray);
                    foreach (var item in selectList)
                    {
                        item.facturaId = id_factura;
                        _Facturas.alta_Factura_Producto(item);
                    }
                    ViewBag.productos = _Facturas.creacion().lista_Productos;
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            ViewBag.Vencimientos = new SelectList(new List<Vencimiento>() { new Vencimiento() { Id = 30, Vencimientos = "30" }, new Vencimiento() { Id = 60, Vencimientos = "60" }, new Vencimiento() { Id = 90, Vencimientos = "90" }, new Vencimiento() { Id = 120, Vencimientos = "120" }, new Vencimiento() { Id = 150, Vencimientos = "150" }, new Vencimiento() { Id = 200, Vencimientos = "200" }, new Vencimiento() { Id = 300, Vencimientos = "300" } }, "Id", "Vencimientos", "SELECCIONE");
            decimal total = Prod.Sum(elemento => elemento.Precio_Unitario*elemento.Cantidad);
            @ViewBag.Total = total>0?"El total es:" + total:string.Empty;
            ViewBag.productos = Prod;//_Facturas.creacion().lista_Productos;
            ViewBag.Proveedor = new SelectList(_Facturas.ObtenerProveedores(), "Id", "Nombre", "SELECCIONE");
            return View(idArray);
        }


        private int pageSize = 10;
        private static List<Facturas_Totales_Productos> Prod;
        //
        // GET: /Factura/Edit/5
        [HttpGet]
        public ActionResult Edit(int  id,int ? page)
        {
            ViewData["SelectList"] = HttpContext.Session["SelectList"] ?? selectList;
            
            var lista = _Facturas.Detalle(id);
            var prod = _Facturas.Buscar_Id_Productos(lista.Factura_Total_Id);
            Prod = new List<Facturas_Totales_Productos>();
            foreach (Facturas_Totales_Productos elem in prod)
            {
                elem.Num_Lote = elem.Num_Lote ?? string.Empty;
                Prod.Add(new Facturas_Totales_Productos()
                {
                    Cantidad = Convert.ToInt32(elem.productos.Cantidad),
                    Num_Lote = elem.productos.Num_Lote,
                    Precio_Unitario = elem.Precio_Unitario,
                    Id = elem.productos.Id,
                    productos = elem.productos,
                    Codigo = elem.Codigo,
                    Descripcion = elem.Descripcion,
                    Visible = false
                });
            }
            foreach (var elemento in prod)
            {
                if (elemento.productos.Visible)
                {
                    selectList.Add(new Grilla()
                    {
                        cantidad = elemento.Cantidad.ToString(),
                        facturaId = elemento.Factura_TotalId,
                        lote = elemento.Num_Lote,
                        precio = elemento.Precio_Unitario.ToString(),
                        id = elemento.productos.Id.ToString()
                    });
                }
            }
            selectList = selectList.GroupBy(J => J.id).Select(I => I.First()).ToList();
            foreach (var elemento in prod)
            {
                foreach (var el in selectList)
                {
                  
                    if (elemento.productos.Id==Convert.ToInt32(el.id))
                    {
                        elemento.Precio_Unitario = Convert.ToDecimal(el.precio);
                        elemento.Cantidad = Convert.ToInt32(el.cantidad);
                        elemento.Id = Convert.ToInt32(el.id);
                        elemento.Num_Lote = el.lote;
                        elemento.productos.Visible = true;
                    }
                }
            }
            foreach (Facturas_Totales_Productos el in prod)
            {
                el.Id = el.productos.Id;
                el.Total = Convert.ToDecimal(el.Precio_Unitario)*Convert.ToDecimal(el.Cantidad);
            }

            page = page ?? 0;
            if (page == 0)
            {
                Prod = prod;
            }
            ViewBag.Productos = prod;
            

            var creacion=lista.FacturasTotales.Creacion;
            var vencimiento = lista.FacturasTotales.Vencimiento_Factura;
            
            TimeSpan diferencia = vencimiento - creacion;
            lista.Vencimientos= diferencia.Days;


            ViewBag.Vence = new SelectList(new List<Vencimiento>() { new Vencimiento() { Id = 30, Vencimientos = "30" }, new Vencimiento() { Id = 60, Vencimientos = "60" }, new Vencimiento() { Id = 90, Vencimientos = "90" }, new Vencimiento() { Id = 120, Vencimientos = "120" }, new Vencimiento() { Id = 150, Vencimientos = "150" }, new Vencimiento() { Id = 200, Vencimientos = "200" }, new Vencimiento() { Id = 300, Vencimientos = "300" } }, "Id", "Vencimientos", "SELECCIONE");
            decimal total = prod.Sum(ele => ele.Precio_Unitario*Convert.ToDecimal(ele.Cantidad));
            ViewBag.Total = "El total es: " + total;
            ViewBag.Vencimiento = "El vencimiento es: " + lista.Flete.Value.AddDays(lista.Vencimientos).ToString("dd-MM-yyyy");
            lista.FacturasTotales.ProveedorId = lista.ProveedorId;
            lista.Envalaje = Convert.ToDecimal(lista.Envalaje, new CultureInfo("en-US"));
            ViewBag.Proveedor = new SelectList(_Facturas.ObtenerProveedores(), "Id", "Nombre", lista.ProveedorId.ToString());
            if (Request.IsAjaxRequest())
            {
                return PartialView("GrillaEdicion");
            }
            else
            {
                return View(lista);
            }

            
        }

      


        //
        // POST: /Factura/Edit/5
        [HttpPost]
        public ActionResult Edit(Facturas ft, FormCollection collection)
        {

            ft.FacturasTotales.Vencimiento_Factura = ft.FacturasTotales.Creacion.AddDays(ft.Vencimientos);

            
            if (selectList.Count == 0)
            {
                ModelState.AddModelError("Productos", "Por favor seleccione lo/s producto de la factura");
            }
                if (ModelState.IsValid)
                {
                    try
                    {
                        List<int> lista_Id_productos = new List<int>();
                        var lista = new List<Facturas_Totales_Productos>();
                        foreach (var element in selectList)
                        {
                            lista_Id_productos.Add(int.Parse(element.id));
                            var cont = new Facturas_Totales_Productos()
                            {
                                Id = Convert.ToInt32(element.id),
                                Cantidad = Convert.ToInt32(element.cantidad),
                                Num_Lote = element.lote,
                                Precio_Unitario = Convert.ToDecimal(element.precio),
                                Producto_Id = Convert.ToInt32(element.id),
                                Factura_TotalId = element.facturaId
                            };
                            lista.Add(cont);
                        }
                        ft.FacturasTotales.FacturasTotalesProductos = lista;
                        if (!lista_Id_productos.Equals(null))
                        {
                            ft.Producto_Id1 = ft.FacturasTotales.FacturasTotalesProductos;
                            ft.FacturasTotales.ProveedorId = ft.ProveedorId;
                            _Facturas.Update(ft);
                        }
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        return View();
                    }
                }
                
            //METER LA GRILLA CON EL SELECTLIST STATIC RECARGADO CON LA SELECCION
                var prod = _Facturas.Buscar_Id_Productos(ft.Factura_Total_Id);

                foreach (var elemento in prod)
                {
                    if (elemento.productos.Visible)
                    {
                        selectList.Add(new Grilla()
                        {
                            cantidad = elemento.Cantidad.ToString(),
                            facturaId = elemento.Factura_TotalId,
                            lote = elemento.Num_Lote,
                            precio = elemento.Precio_Unitario.ToString(),
                            id = elemento.productos.Id.ToString()
                        });
                    }
                }
                selectList = selectList.GroupBy(J => J.id).Select(I => I.First()).ToList();
                foreach (var elemento in prod)
                {
                    foreach (var el in selectList)
                    {
                        if (elemento.productos.Id == Convert.ToInt32(el.id))
                        {
                            elemento.productos.Visible = true;
                            elemento.Precio_Unitario = Convert.ToDecimal(el.precio);
                            elemento.Cantidad = Convert.ToInt32(el.cantidad);
                            elemento.Id = Convert.ToInt32(el.id);
                            elemento.Num_Lote = el.lote;
                        }
                    }
                }
               
                ViewBag.Productos = prod;
            return View();
           
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_Facturas.Detalle(id));
            }catch(Exception)
            {
                return View();
            }
            
        }
        //
        // GET: /Factura/Delete/5
 
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                _Facturas.Baja(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
