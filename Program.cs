using System;
using System.Collections.Generic;
namespace gestorventasunidad1
{
    class programa
    {
        static void Main(string[] args)
        {
            List<string> nombres = new List<string>();
            List<decimal> precios = new List<decimal>();
            List<int> stocks = new List<int>();
            List<int> vendidos = new List<int>();
            int totalventas = 0;
            decimal totalcaja = 0;
            int opcion;
            do
            {
                imprimirencabezado("sistema gestor de ventas e inventario (mini-pos)");
                Console.WriteLine("1. registrar nuevo producto");
                Console.WriteLine("2. consultar inventario completo");
                Console.WriteLine("3. registrar una venta");
                Console.WriteLine("4. ver reporte de caja y estadisticas diarias");
                Console.WriteLine("5. salir");
                Console.WriteLine("=====================================================");
                opcion = leerentero("seleccione una opcion (1-5): ", 1, 5);
                Console.WriteLine();
                if (opcion == 1)
                {
                    string nombre = "";
                    bool nombrevalido = false;
                    while (nombrevalido == false)
                    {
                        Console.Write("nombre del producto: ");
                        nombre = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(nombre))
                        {
                            Console.WriteLine("[error] el nombre no puede estar vacio.");
                        }
                        else
                        {
                            bool repetido = false;
                            for (int i = 0; i < nombres.Count; i++)
                            {
                                if (nombres[i].ToLower() == nombre.ToLower())
                                {
                                    repetido = true;
                                }
                            }
                            if (repetido)
                            {
                                Console.WriteLine("[error] ya existe un producto con ese nombre.");
                            }
                            else
                            {
                                nombrevalido = true;
                            }
                        }
                    }
                    decimal precio = leerdecimal("precio unitario ($): ", 0.01m);
                    int stock = leerentero("stock inicial (cantidad disponible): ", 0, 1000000);
                    nombres.Add(nombre);
                    precios.Add(precio);
                    stocks.Add(stock);
                    vendidos.Add(0);
                    Console.WriteLine();
                    Console.WriteLine("[ok] producto \"" + nombre + "\" registrado con exito.");
                }
                else if (opcion == 2)
                {
                    if (nombres.Count == 0)
                    {
                        Console.WriteLine("no hay productos registrados en el inventario.");
                    }
                    else
                    {
                        for (int i = 0; i < nombres.Count; i++)
                        {
                            string alerta = "";
                            if (stocks[i] < 5)
                            {
                                alerta = "[alerta: bajo stock]";
                            }
                            Console.WriteLine((i + 1) + ". " + nombres[i] + " | precio: $" + precios[i] + " | stock: " + stocks[i] + " " + alerta);
                        }
                    }
                }
                else if (opcion == 3)
                {
                    if (nombres.Count == 0)
                    {
                        Console.WriteLine("no hay productos registrados. registre productos antes de vender.");
                    }
                    else
                    {
                        Console.WriteLine("productos disponibles:");
                        for (int i = 0; i < nombres.Count; i++)
                        {
                            string alerta = "";
                            if (stocks[i] < 5)
                            {
                                alerta = "[alerta: bajo stock]";
                            }
                            Console.WriteLine((i + 1) + ". " + nombres[i] + " | precio: $" + precios[i] + " | stock: " + stocks[i] + " " + alerta);
                        }
                        Console.WriteLine();
                        int numero = leerentero("ingrese el numero del producto a vender: ", 1, nombres.Count);
                        int indice = numero - 1;
                        int cantidad = 0;
                        bool cantidadvalida = false;
                        while (cantidadvalida == false)
                        {
                            cantidad = leerentero("ingrese la cantidad a comprar: ", 1, 1000000);
                            if (cantidad > stocks[indice])
                            {
                                Console.WriteLine("[error] stock insuficiente. solo quedan " + stocks[indice] + " unidades en inventario.");
                            }
                            else
                            {
                                cantidadvalida = true;
                            }
                        }
                        Console.Write("aplica descuento de cliente frecuente (10%)? (s/n): ");
                        string respuesta = Console.ReadLine();
                        bool tienedescuento = false;
                        if (respuesta != null && respuesta.ToUpper() == "S")
                        {
                            tienedescuento = true;
                        }
                        decimal subtotal = precios[indice] * cantidad;
                        decimal descuento;
                        decimal iva;
                        decimal total = calcularfactura(precios[indice], cantidad, tienedescuento, out descuento, out iva);
                        stocks[indice] = stocks[indice] - cantidad;
                        vendidos[indice] = vendidos[indice] + cantidad;
                        totalventas = totalventas + 1;
                        totalcaja = totalcaja + total;
                        Console.WriteLine();
                        Console.WriteLine("=====================================");
                        Console.WriteLine("           ticket de venta");
                        Console.WriteLine("=====================================");
                        Console.WriteLine("producto: " + nombres[indice] + " (x" + cantidad + ")");
                        Console.WriteLine("subtotal: $" + subtotal);
                        Console.WriteLine("descuento: $" + descuento);
                        Console.WriteLine("iva (19%): $" + iva);
                        Console.WriteLine("-------------------------------------");
                        Console.WriteLine("total a pagar: $" + total);
                        Console.WriteLine("=====================================");
                        Console.WriteLine();
                        Console.WriteLine("[ok] venta efectuada con exito. stock actualizado.");
                    }
                }
                else if (opcion == 4)
                {
                    Console.WriteLine("total de ventas realizadas: " + totalventas);
                    Console.WriteLine("total acumulado en caja: $" + totalcaja);
                    decimal promedio = 0;
                    if (totalventas > 0)
                    {
                        promedio = totalcaja / totalventas;
                    }
                    Console.WriteLine("promedio de dinero por venta: $" + promedio);
                    if (totalventas == 0)
                    {
                        Console.WriteLine("producto mas vendido: n/a (aun no hay ventas)");
                    }
                    else
                    {
                        int indicemax = 0;
                        for (int i = 1; i < vendidos.Count; i++)
                        {
                            if (vendidos[i] > vendidos[indicemax])
                            {
                                indicemax = i;
                            }
                        }
                        Console.WriteLine("producto mas vendido: " + nombres[indicemax] + " (" + vendidos[indicemax] + " unidades)");
                    }
                }
                else if (opcion == 5)
                {
                    Console.WriteLine("gracias por usar el sistema gestor de ventas e inventario.");
                    Console.WriteLine("hasta pronto!");
                }
                if (opcion != 5)
                {
                    Console.WriteLine();
                    Console.WriteLine("presione enter para continuar...");
                    Console.ReadLine();
                }
            } while (opcion != 5);
        }
        static int leerentero(string mensaje, int min, int max)
        {
            int numero;
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();
                bool esvalido = int.TryParse(texto, out numero);
                if (esvalido == false)
                {
                    Console.WriteLine("[error] entrada no valida. debe ingresar un numero entero.");
                }
                else if (numero < min || numero > max)
                {
                    Console.WriteLine("[error] opcion fuera de rango. ingrese un valor entre " + min + " y " + max + ".");
                }
                else
                {
                    return numero;
                }
            }
        }
        static decimal leerdecimal(string mensaje, decimal minimo)
        {
            decimal numero;
            while (true)
            {
                Console.Write(mensaje);
                string texto = Console.ReadLine();
                bool esvalido = decimal.TryParse(texto, out numero);
                if (esvalido == false)
                {
                    Console.WriteLine("[error] entrada no valida. debe ingresar un numero decimal.");
                }
                else if (numero < minimo)
                {
                    Console.WriteLine("[error] el valor debe ser mayor o igual a " + minimo + ".");
                }
                else
                {
                    return numero;
                }
            }
        }
        static decimal calcularfactura(decimal precio, int cantidad, bool tienedescuento, out decimal descuento, out decimal iva)
        {
            decimal subtotal = precio * cantidad;
            if (tienedescuento)
            {
                descuento = subtotal * 0.10m;
            }
            else
            {
                descuento = 0;
            }
            decimal basecondescuento = subtotal - descuento;
            iva = basecondescuento * 0.19m;
            decimal total = basecondescuento + iva;
            return total;
        }
        static void imprimirencabezado(string titulo)
        {
            try
            {
                Console.Clear();
            }
            catch
            {
            }
            Console.WriteLine("=====================================================");
            Console.WriteLine(titulo);
        }
    }
}
