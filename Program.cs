#region Inicial

using CursoLinQ;
using Newtonsoft.Json;

string[] niveles = { "Básico", "Intermedio", "Avanzado" };
Console.WriteLine("Count en array:" + niveles.Length);
var n = niveles.Where(x => x.Length > 6);
foreach (var nivel in n)
{
    Console.WriteLine("Nivel:" + nivel);
}

var nuevoNivel = from nvls in niveles where nvls.Length > 6 orderby nvls ascending select nvls;
foreach (var nuevo in nuevoNivel)
{
    Console.WriteLine("Nivel ordenado: " + nuevo);
}

#endregion

#region Objeto
List<Empleado> empleados = new(){
    new Empleado
    {
        Id = Guid.NewGuid(),
        Nombre = "Toni",
        Apellido = "Perez",
        Edad = 29,
        Departamento = Departamento.Admin,
        IdExterno = 1
    },
    new Empleado{
        Id = Guid.NewGuid(),
        Nombre = "Ana",
        Apellido = "Lopez Ventura",
        Edad = 40,
        Departamento = Departamento.It,
        IdExterno = 2
    },
    new Empleado{
        Id = Guid.NewGuid(),
        Nombre = "Fabio",
        Apellido = "Rodriguez",
        Edad = 25,
        Departamento = Departamento.Desarrollo,
        IdExterno = 3
    },
    new Empleado {
        Id = Guid.NewGuid(),
        Nombre = "Fabiola",
        Apellido = "Cortes",
        Edad = 22,
        Departamento = Departamento.Desarrollo,
        IdExterno = 4,
        Pagos = new List<Pago>
        {
            new() {
                Descripcion = "Quincena #1: Diciembre",
                Fecha = new DateTime(2020,12,15),
                Cantidad = 15000.95f,
                Procesado = true
                }
        }
    },
    new Empleado {
         Id = Guid.NewGuid(),
        Nombre = "Monica",
        Apellido = "Correa",
        Edad = 55,
        Departamento = Departamento.Soporte,
        IdExterno = 5,
        Pagos = new List<Pago> {
            new() {
                Descripcion = "Quincena #21: Noviembre",
                Fecha = new DateTime(2020,11,15),
                Cantidad = 18000.95f,
                Procesado = true,
            },
            new() {
                Descripcion = "Quincena #22: Noviembre",
                Fecha = new DateTime(2020,11,30),
                Cantidad = 20000.95f,
                Procesado = false,
             }
        }
    }
};

var nPagos = new List<Pago>
{
    new() {
        Descripcion = "Quincena Junio",
        Fecha = new DateTime(2020,06,15),
        Cantidad = 12000.95f,
        Procesado = true,
        IdExternoEmpleado = 2
    },
    new() {
        Descripcion = "Quincena Septiembre",
        Fecha = new DateTime(2020,06,30),
        Cantidad = 22000.95f,
        Procesado = false,
        IdExternoEmpleado = 3
    },
    new() {
        Descripcion = "Bono Diciembre",
        Fecha = new DateTime(2020,12,15),
        Cantidad = 50000.00f,
        Procesado = true,
        IdExternoEmpleado = 3
    }
};

#endregion
#region Encadenamiento de operadores


var filtro = empleados.Where(
    e => e.Departamento == Departamento.Desarrollo && e.Nombre.ToLower().Contains('f'))
    .OrderBy(e => e.Id)
    .Select(e => new
    {
        e.Id,
        e.Nombre,
        e.Apellido,
        e.Departamento
    });
Console.WriteLine(" **************");
var encabezado = string.Format("{0,-40} {1,-10} {2,-10} {3}",
                            "Id", "Nombre", "Apellido", "Departamento");

Console.WriteLine(encabezado);

foreach (var f in filtro)
{
    string fila = string.Format("{0,-40} {1,-10} {2,-10} {3}",
                    f.Id, f.Nombre, f.Apellido, f.Departamento);
    Console.WriteLine(fila);
}

var filtro2 = empleados.Where(
    f => (f.Departamento == Departamento.Desarrollo || f.Departamento == Departamento.Soporte)
    && f.Apellido.ToLower().StartsWith('c'))
    .OrderByDescending(f => f.Nombre)
    .Select(f => new
    {
        f.Nombre,
        f.Apellido,
        Depto = f.Departamento
    });
Console.WriteLine(" **************");
var encabezado2 = string.Format("{0,-20} {1,-20} {2}", "Nombre", "Apellido", "Depto");
Console.WriteLine(encabezado2);

foreach (var f2 in filtro2)
{
    string fila2 = string.Format("{0,-20} {1,-20} {2}",
                                f2.Nombre, f2.Apellido, f2.Depto);
    Console.WriteLine(fila2);
}
#endregion
#region Subconsultas
Console.WriteLine(" **************");
var filtroSubconsulta = empleados.Where(x => x.Apellido.Split().LastOrDefault().StartsWith('V'));

string encabezado3 = string.Format("{0,-20} {1,-20} {2}", "Nombre", "Apellido", "Depto");
Console.WriteLine(encabezado3);

foreach (var f3 in filtroSubconsulta)
{
    string fila3 = string.Format("{0,-20} {1,-20} {2}",
                                f3.Nombre, f3.Apellido, f3.Departamento);
    Console.WriteLine(fila3);
}

Console.WriteLine(" **************");

var filtroSubconsulta2 = empleados.Where(x => x.Nombre.Length == empleados.OrderBy(eb => eb.Apellido.Length)
                                        .Select(eb => eb.Apellido.Length)
                                        .First());

string encabezado4 = string.Format("{0,-20} {1}", "Nombre", "Apellido");
Console.WriteLine(encabezado4);

foreach (var f4 in filtroSubconsulta2)
{
    string fila4 = string.Format("{0,-20} {1}",
                                f4.Nombre, f4.Apellido);
    Console.WriteLine(fila4);
}

#endregion
#region Operadores
Console.WriteLine(" **************");
//Filtro empleados con edad igual o menor a 30
var filtroEdad = empleados.Where(e => e.Edad <= 30).Reverse();
ImprimeEmpleados(filtroEdad, "************** REVERSE **************");
//Skip
var fs = filtroEdad.Skip(1);
ImprimeEmpleados(fs, "************** SKIP **************");
//Filtra, ordena y agrupa
Console.WriteLine("************** FILTRA, ORDENA Y AGRUPA **************");
var foa = empleados.Where(e => e.Edad <= 30).OrderBy(e => e.Nombre).ThenBy(e => e.Departamento == Departamento.Desarrollo && e.Pagos != null);
ImprimeEmpleados(foa, "");
var empleadoDesarrolloPagado = empleados.Where(e => e.Departamento == Departamento.Desarrollo)
                                        .SelectMany(e => e.Pagos, (e, pago) => new
                                        {
                                            e.Nombre,
                                            pago.Descripcion,
                                            pago.Cantidad
                                        });
//var cantidadPagos = empleadoDesarrolloPagado.Count();
//var promedioPagos = empleadoDesarrolloPagado.Average(p => p.Cantidad);
//Console.WriteLine($"Promedio de cantidad {promedioPagos}");

//var cantidad = 20000f;
//var existePagoMayor = empleadoDesarrolloPagado.Any(e => e.Cantidad <= cantidad);

#region Join
ImprimeEmpleados(filtroEdad, "************** JOIN **************");
var empleadosPagos = empleados.Join(nPagos,
                                    emp => emp.IdExterno,
                                    pago => pago.IdExternoEmpleado,
                                    (emp, pago) => new
                                    {
                                        emp.Nombre,
                                        pago.Cantidad
                                    });

var empleadosPagosGrupo = empleados.GroupJoin(nPagos,
                                            emp => emp.IdExterno,
                                            pago => pago.IdExternoEmpleado,
                                            (emp, pagos) => new
                                            {
                                                Empleado = emp.Nombre,
                                                PagosAgregados = pagos
                                            });

foreach (var e in empleadosPagosGrupo)
{
    if (e.PagosAgregados.Any())
        Console.WriteLine(e.Empleado);
    foreach (var p in e.PagosAgregados)
        Console.WriteLine(p.Cantidad);
}
#endregion


static void ImprimeEmpleado(Empleado e)
{
    string fila = string.Format("{0,-40} {1,-10} {2,-20} {3,-10} {4}",
    e.Id, e.Nombre, e.Apellido, e.Edad, e.Departamento);
    Console.WriteLine(fila);
}

static void ImprimeEmpleados(IEnumerable<Empleado> lista, string titulo = "/** --- ** /")
{
    Console.WriteLine(titulo);
    var encabezado = string.Format("{0,-40} {1,-10} {2,-20} {3,-10} {4}",
                           "ID", "Nombre", "Apellido", "Edad", "Departamento");
    Console.WriteLine(encabezado);
    foreach (var el in lista)
    {
        ImprimeEmpleado(el);
    }
}
#endregion

#region JSON
Console.WriteLine("************** JSON **************");
var archivo = @"C:\repo\CursoLinQ\CursoLinQ\Empleado.json";
var empeleados = ObtenerEmpleadosDeRuta(archivo);
var nombre = empeleados.Where(e => e.Edad <= 25).Select(e => e.Nombre).FirstOrDefault();
Console.WriteLine(nombre);

static List<NewEmpleado> ObtenerEmpleadosDeRuta(string ruta){
    List<NewEmpleado> listaEmpleado = new();
    using var reader = new StreamReader(ruta);
    string json = reader.ReadToEnd();
    listaEmpleado = JsonConvert.DeserializeObject<List<NewEmpleado>>(json);
    
    return listaEmpleado;
}
#endregion

public class Empleado
{
    public Guid Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public int Edad { get; set; }
    public Departamento Departamento { get; set; }
    public int IdExterno { get; set; }
    public List<Pago> Pagos { get; set; }
}

public class Pago
{
    public string? Descripcion { get; set; }
    public DateTime? Fecha { get; set; }
    public float? Cantidad { get; set; }
    public bool? Procesado { get; set; }
    public int? IdExternoEmpleado { get; set; }
}

public enum Departamento
{
    Admin,
    Desarrollo,
    It,
    Soporte
}



