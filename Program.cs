using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
 
// =====================================================================
// TP N 13 - Composicion y Agregacion
// =====================================================================
//
// EJERCICIO 10: DIFERENCIA ENTRE COMPOSICION Y AGREGACION
// ---------------------------------------------------------------------
// Las dos son relaciones "TIENE UN" (has-a): un objeto contiene a otro.
// La diferencia esta en quien es DUENIO del objeto contenido y en el
// CICLO DE VIDA de cada uno.
//
// COMPOSICION (relacion fuerte, "parte-de"):
//   - El contenedor CREA a la parte (new dentro de su constructor/metodos).
//   - La parte NO existe de forma independiente: nace y muere con el todo.
//   - Nadie mas deberia tener referencia a la parte.
//   - Ejemplos de este TP: Auto-Motor, Computadora-CPU/RAM, Factura-Linea.
//   - Pista en el codigo: "new Motor(...)" adentro de Auto.
//
// AGREGACION (relacion debil, "usa-a" / "esta formado por"):
//   - El contenedor RECIBE al objeto desde afuera (parametro o metodo).
//   - El objeto existe antes, durante y despues de la relacion.
//   - Puede asignarse, quitarse o moverse a otro contenedor.
//   - Ejemplos de este TP: Colectivo-Conductor, Equipo-Jugador.
//   - Pista en el codigo: metodos AsignarConductor(...), AgregarJugador(...)
//     y propiedades que pueden ser null.
//
// RESUMEN RAPIDO:
//   Composicion -> el todo crea y destruye a la parte. (Auto y su Motor)
//   Agregacion  -> el todo solo referencia a la parte. (Colectivo y su Conductor)
//
//   Pregunta para decidir: "si destruyo al contenedor, tiene sentido que
//   el contenido siga existiendo?"  No -> composicion.  Si -> agregacion.
// =====================================================================
 
 
// =====================================================================
// EJERCICIO 1: Auto con Motor por composicion
// =====================================================================
 
// COMPOSICION: Motor creado por Auto, muere con el
public class Motor
{
    public int Cilindros { get; }
    public Motor(int c) => Cilindros = c;
    public void Arrancar() => Console.WriteLine("Vroom!");
}
 
public class Auto
{
    public string Marca { get; }
    public Motor Motor { get; }
 
    public Auto(string marca, int cil)
    {
        Marca = marca;
        Motor = new Motor(cil);  // composicion: el Auto crea su propio Motor
    }
}
 
 
// =====================================================================
// EJERCICIO 2: Computadora con CPU y RAM por composicion
// =====================================================================
 
public class CPU
{
    public string Modelo { get; }
    public int Nucleos { get; }
 
    public CPU(string modelo, int nucleos)
    {
        Modelo = modelo;
        Nucleos = nucleos;
    }
 
    public void Procesar() =>
        Console.WriteLine($"CPU {Modelo} procesando con {Nucleos} nucleos...");
}
 
public class RAM
{
    public int CapacidadGB { get; }
    public RAM(int capacidadGB) => CapacidadGB = capacidadGB;
 
    public void Cargar() =>
        Console.WriteLine($"RAM de {CapacidadGB} GB cargando datos...");
}
 
// COMPOSICION: la Computadora crea su CPU y su RAM
public class Computadora
{
    public string Nombre { get; }
    public CPU Cpu { get; }
    public RAM Ram { get; }
 
    public Computadora(string nombre, string modeloCpu, int nucleos, int gbRam)
    {
        Nombre = nombre;
        Cpu = new CPU(modeloCpu, nucleos);  // composicion
        Ram = new RAM(gbRam);               // composicion
    }
 
    public void Encender()
    {
        Console.WriteLine($"Encendiendo {Nombre}...");
        Ram.Cargar();
        Cpu.Procesar();
    }
}
 
 
// =====================================================================
// EJERCICIOS 4, 5, 6 y 9: Colectivo con Conductor por agregacion
// =====================================================================
 
// AGREGACION: Conductor existe independientemente
public class Conductor
{
    public string Nombre { get; }
    public Conductor(string nombre) => Nombre = nombre;
}
 
public class Colectivo
{
    public string Patente { get; }
    public Conductor? Conductor { get; private set; }  // puede ser null
 
    public Colectivo(string patente) => Patente = patente;
 
    public void AsignarConductor(Conductor c) => Conductor = c;  // viene de afuera
    public void Desvincular() => Conductor = null;               // el Conductor sigue vivo
}
 
 
// =====================================================================
// EJERCICIO 7: Equipo con lista de Jugadores por agregacion
// =====================================================================
 
public class Jugador
{
    public string Nombre { get; }
    public int Camiseta { get; }
 
    public Jugador(string nombre, int camiseta)
    {
        Nombre = nombre;
        Camiseta = camiseta;
    }
}
 
public class Equipo
{
    public string Nombre { get; }
    private readonly List<Jugador> _jugadores = new();
    public IReadOnlyList<Jugador> Jugadores => _jugadores;
 
    public Equipo(string nombre) => Nombre = nombre;
 
    // AGREGACION: los jugadores se crean afuera y se agregan al equipo
    public void AgregarJugador(Jugador j) => _jugadores.Add(j);
    public bool QuitarJugador(Jugador j) => _jugadores.Remove(j);
}
 
 
// =====================================================================
// EJERCICIO 8: Factura con lista de Lineas por composicion
// =====================================================================
 
public class Linea
{
    public string Descripcion { get; }
    public int Cantidad { get; }
    public decimal PrecioUnitario { get; }
    public decimal Subtotal => Cantidad * PrecioUnitario;
 
    public Linea(string descripcion, int cantidad, decimal precioUnitario)
    {
        Descripcion = descripcion;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
    }
}
 
// COMPOSICION: la Factura crea sus propias Lineas; sin Factura no tienen sentido
public class Factura
{
    public int Numero { get; }
    private readonly List<Linea> _lineas = new();
    public IReadOnlyList<Linea> Lineas => _lineas;
    public decimal Total => _lineas.Sum(l => l.Subtotal);
 
    public Factura(int numero) => Numero = numero;
 
    // No se recibe una Linea ya armada: la Factura la crea (composicion)
    public void AgregarLinea(string descripcion, int cantidad, decimal precioUnitario) =>
        _lineas.Add(new Linea(descripcion, cantidad, precioUnitario));
}
 
 
// =====================================================================
// PROGRAMA PRINCIPAL: demuestra cada ejercicio
// =====================================================================
 
public class Program
{
    public static void Main()
    {
        Titulo("EJERCICIO 1: Auto con Motor (composicion)");
        Ejercicio1();
 
        Titulo("EJERCICIO 2: Computadora con CPU y RAM (composicion)");
        Ejercicio2();
 
        Titulo("EJERCICIO 3: Motor no tiene sentido sin Auto");
        Ejercicio3();
 
        Titulo("EJERCICIO 4: Colectivo con Conductor (agregacion)");
        Ejercicio4();
 
        Titulo("EJERCICIO 5: Conductor existe sin Colectivo");
        Ejercicio5();
 
        Titulo("EJERCICIO 6: Reasignar conductor a otro colectivo");
        Ejercicio6();
 
        Titulo("EJERCICIO 7: Equipo con Jugadores (agregacion)");
        Ejercicio7();
 
        Titulo("EJERCICIO 8: Factura con Lineas (composicion)");
        Ejercicio8();
 
        Titulo("EJERCICIO 9: Acceso seguro con ?.");
        Ejercicio9();
 
        Titulo("EJERCICIO 10: Diferencia entre composicion y agregacion");
        Console.WriteLine("Ver los comentarios al inicio de este archivo.");
        Console.WriteLine("Composicion: el todo CREA a la parte y la parte muere con el.");
        Console.WriteLine("Agregacion : el todo RECIBE a la parte y la parte vive por su cuenta.");
    }
 
    static void Titulo(string texto)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 60));
        Console.WriteLine(texto);
        Console.WriteLine(new string('=', 60));
    }
 
    // ---------- 1 ----------
    static void Ejercicio1()
    {
        Auto auto = new Auto("Ford", 4);
        Console.WriteLine($"Auto: {auto.Marca} con motor de {auto.Motor.Cilindros} cilindros");
        auto.Motor.Arrancar();
    }
 
    // ---------- 2 ----------
    static void Ejercicio2()
    {
        Computadora pc = new Computadora("PC-Oficina", "Ryzen 5", 6, 16);
        pc.Encender();
    }
 
    // ---------- 3 ----------
    // Se crea el Auto en un metodo aparte. Al terminar el metodo, el Auto
    // queda sin referencias y su Motor (que solo el conocia) tambien.
    [MethodImpl(MethodImplOptions.NoInlining)]
    static WeakReference CrearAutoYObservarMotor()
    {
        Auto auto = new Auto("Fiat", 4);
        return new WeakReference(auto.Motor);  // referencia debil: no mantiene vivo al Motor
    }
 
    static void Ejercicio3()
    {
        // a) El Motor lo crea el Auto: no hay forma de tener un Auto sin Motor
        Auto auto = new Auto("Renault", 6);
        Console.WriteLine($"El {auto.Marca} siempre nace con su motor de {auto.Motor.Cilindros} cilindros.");
 
        // b) Cuando el Auto desaparece, el Motor desaparece con el
        WeakReference refMotor = CrearAutoYObservarMotor();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
 
        if (refMotor.IsAlive)
            Console.WriteLine("El GC todavia no libero el Motor (depende del runtime).");
        else
            Console.WriteLine("El Auto fue destruido y su Motor desaparecio con el: no tiene sentido sin Auto.");
    }
 
    // ---------- 4 ----------
    static void Ejercicio4()
    {
        Conductor juan = new Conductor("Juan");
        Colectivo colectivo = new Colectivo("AB123CD");
 
        colectivo.AsignarConductor(juan);
        Console.WriteLine($"Colectivo {colectivo.Patente} manejado por {colectivo.Conductor?.Nombre}");
    }
 
    // ---------- 5 ----------
    static void Ejercicio5()
    {
        // El Conductor existe ANTES de tener un Colectivo
        Conductor maria = new Conductor("Maria");
        Console.WriteLine($"Conductor creado solo: {maria.Nombre} (todavia no hay ningun colectivo)");
 
        // Se asigna, se desvincula y el Conductor sigue existiendo
        Colectivo? colectivo = new Colectivo("EF456GH");
        colectivo.AsignarConductor(maria);
        colectivo.Desvincular();
        Console.WriteLine($"Despues de desvincular, {maria.Nombre} sigue existiendo.");
 
        // Incluso si el Colectivo desaparece, el Conductor sigue vivo
        colectivo = null;
        GC.Collect();
        Console.WriteLine($"Sin colectivo alguno, {maria.Nombre} sigue existiendo.");
    }
 
    // ---------- 6 ----------
    static void Ejercicio6()
    {
        Conductor pedro = new Conductor("Pedro");
        Colectivo colectivo1 = new Colectivo("AAA111");
        Colectivo colectivo2 = new Colectivo("BBB222");
 
        colectivo1.AsignarConductor(pedro);
        Console.WriteLine($"Antes    -> {colectivo1.Patente}: {colectivo1.Conductor?.Nombre ?? "(sin conductor)"} | " +
                          $"{colectivo2.Patente}: {colectivo2.Conductor?.Nombre ?? "(sin conductor)"}");
 
        // Reasignacion: se saca del primero y se pasa al segundo
        Conductor? conductor = colectivo1.Conductor;
        colectivo1.Desvincular();
        if (conductor != null)
            colectivo2.AsignarConductor(conductor);
 
        Console.WriteLine($"Despues  -> {colectivo1.Patente}: {colectivo1.Conductor?.Nombre ?? "(sin conductor)"} | " +
                          $"{colectivo2.Patente}: {colectivo2.Conductor?.Nombre ?? "(sin conductor)"}");
    }
 
    // ---------- 7 ----------
    static void Ejercicio7()
    {
        // Los jugadores se crean AFUERA del equipo
        Jugador j1 = new Jugador("Lionel", 10);
        Jugador j2 = new Jugador("Angel", 11);
        Jugador j3 = new Jugador("Emiliano", 1);
 
        Equipo equipo = new Equipo("Los Pibes");
        equipo.AgregarJugador(j1);
        equipo.AgregarJugador(j2);
        equipo.AgregarJugador(j3);
 
        Console.WriteLine($"Equipo {equipo.Nombre} ({equipo.Jugadores.Count} jugadores):");
        foreach (Jugador j in equipo.Jugadores)
            Console.WriteLine($"  #{j.Camiseta} {j.Nombre}");
 
        // Si sale del equipo, el jugador sigue existiendo
        equipo.QuitarJugador(j2);
        Console.WriteLine($"\n{j2.Nombre} salio del equipo, pero sigue existiendo (camiseta #{j2.Camiseta}).");
        Console.WriteLine($"Jugadores en el equipo ahora: {equipo.Jugadores.Count}");
    }
 
    // ---------- 8 ----------
    static void Ejercicio8()
    {
        Factura factura = new Factura(1001);
        factura.AgregarLinea("Teclado", 2, 15000m);
        factura.AgregarLinea("Mouse", 3, 8000m);
        factura.AgregarLinea("Monitor", 1, 120000m);
 
        Console.WriteLine($"Factura N {factura.Numero}");
        foreach (Linea l in factura.Lineas)
            Console.WriteLine($"  {l.Descripcion,-10} {l.Cantidad} x ${l.PrecioUnitario:F2} = ${l.Subtotal:F2}");
        Console.WriteLine($"  TOTAL: ${factura.Total:F2}");
        Console.WriteLine("Las Lineas las creo la Factura: sin Factura no tienen sentido.");
    }
 
    // ---------- 9 ----------
    static void Ejercicio9()
    {
        Colectivo conChofer = new Colectivo("CCC333");
        conChofer.AsignarConductor(new Conductor("Ana"));
        Colectivo sinChofer = new Colectivo("DDD444");  // Conductor == null
 
        // Con ?. no hay excepcion: si Conductor es null, el resultado es null
        string? nombre1 = conChofer.Conductor?.Nombre;
        string? nombre2 = sinChofer.Conductor?.Nombre;
 
        Console.WriteLine($"{conChofer.Patente}: {nombre1 ?? "(sin conductor)"}");
        Console.WriteLine($"{sinChofer.Patente}: {nombre2 ?? "(sin conductor)"}");
 
        // Sin ?. se produciria una NullReferenceException
        try
        {
            string peligroso = sinChofer.Conductor!.Nombre;  // el ! fuerza el acceso
            Console.WriteLine(peligroso);
        }
        catch (NullReferenceException)
        {
            Console.WriteLine("Sin ?. -> NullReferenceException (Conductor era null).");
        }
    }
}