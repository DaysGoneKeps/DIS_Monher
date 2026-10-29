// Data/DbInitializer.cs
using DIS_Monher.Models;
using Microsoft.EntityFrameworkCore;

namespace DIS_Monher.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Materiales.Any())
            {
                return; // La base de datos ya fue poblada
            }

            var materiales = new Material[]
            {
                new Material { Codigo = "MAT-001", Nombre = "Madera de Roble (tablón)", Descripcion = "Madera seca de 2 pulgadas", Categoria = "Maderas", UnidadMedida = "unidades", StockActual = 120, StockMinimo = 40, Almacen = "Almacén Principal", UsuarioResponsable = "Administrador" },
                new Material { Codigo = "MAT-002", Nombre = "Madera de Nogal (tablón)", Descripcion = "Madera premium para mesas", Categoria = "Maderas", UnidadMedida = "unidades", StockActual = 60, StockMinimo = 20, Almacen = "Almacén Principal", UsuarioResponsable = "Administrador" },
                new Material { Codigo = "MAT-003", Nombre = "Barniz poliuretano", Descripcion = "Acabado brillante", Categoria = "Químicos", UnidadMedida = "litros", StockActual = 18, StockMinimo = 25, Almacen = "Sección B - Norte", UsuarioResponsable = "Administrador" },
                new Material { Codigo = "MAT-004", Nombre = "Tornillos de ensamble", Descripcion = "Acero zincado 2 pulgadas", Categoria = "Ferretería", UnidadMedida = "unidades", StockActual = 2500, StockMinimo = 500, Almacen = "Sección B - Norte", UsuarioResponsable = "Administrador" },
                new Material { Codigo = "MAT-005", Nombre = "Tela tapiz gris", Descripcion = "Tela antimanchas", Categoria = "Textiles", UnidadMedida = "metros", StockActual = 90, StockMinimo = 30, Almacen = "Almacén Principal", UsuarioResponsable = "Administrador" }
            };
            context.Materiales.AddRange(materiales);
            context.SaveChanges();

            var productos = new ProductoTerminado[]
            {
                new ProductoTerminado { Codigo = "PROD-001", Nombre = "Cama Doble 1.40m - Roble", TipoCaja = "Cama", StockActual = 15, PrecioUnitario = 1190.00m },
                new ProductoTerminado { Codigo = "PROD-002", Nombre = "Mesa Comedor 6 plazas - Nogal", TipoCaja = "Mesa", StockActual = 3, PrecioUnitario = 1250.00m },
                new ProductoTerminado { Codigo = "PROD-003", Nombre = "Sofá Modular 3 plazas - Gris", TipoCaja = "Sofá", StockActual = 0, PrecioUnitario = 1800.00m }
            };
            context.ProductosTerminados.AddRange(productos);
            context.SaveChanges();

            // Composición (receta) por unidad producida
            context.ProductoMateriales.AddRange(
                new ProductoMaterial { ProductoTerminadoId = productos[0].Id, MaterialId = materiales[0].Id, CantidadRequerida = 6 },
                new ProductoMaterial { ProductoTerminadoId = productos[0].Id, MaterialId = materiales[2].Id, CantidadRequerida = 1.5m },
                new ProductoMaterial { ProductoTerminadoId = productos[0].Id, MaterialId = materiales[3].Id, CantidadRequerida = 40 },
                new ProductoMaterial { ProductoTerminadoId = productos[1].Id, MaterialId = materiales[1].Id, CantidadRequerida = 8 },
                new ProductoMaterial { ProductoTerminadoId = productos[1].Id, MaterialId = materiales[2].Id, CantidadRequerida = 2 },
                new ProductoMaterial { ProductoTerminadoId = productos[1].Id, MaterialId = materiales[3].Id, CantidadRequerida = 32 },
                new ProductoMaterial { ProductoTerminadoId = productos[2].Id, MaterialId = materiales[4].Id, CantidadRequerida = 14 },
                new ProductoMaterial { ProductoTerminadoId = productos[2].Id, MaterialId = materiales[0].Id, CantidadRequerida = 4 },
                new ProductoMaterial { ProductoTerminadoId = productos[2].Id, MaterialId = materiales[3].Id, CantidadRequerida = 24 }
            );
            context.SaveChanges();

            var cliente = new Cliente
            {
                Documento = "41882736",
                RazonSocial = "Gloria Paredes",
                Contacto = "+51 987654321",
                Direccion = "Av. Javier Prado Este 1230, San Borja, Lima",
                TipoCliente = "VIP"
            };
            context.Clientes.Add(cliente);
            context.SaveChanges();

            // Alertas de Stock Mínimo iniciales (HU005)
            foreach (var m in materiales) InventarioHelper.EvaluarAlerta(context, m);
                context.SaveChanges();
        }
    }
}