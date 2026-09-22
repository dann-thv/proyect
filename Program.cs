using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace DAMAS
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] tablero = new int[8, 8];
            string[] historial = new string[100];
            int Contadormovimientos = 0;
            int turno = 1;

            DateTime tiempoInicio = DateTime.Now;

            int cursorFila = 5;
            int cursorcol = 0;

            int filaOrigen = -1;
            int colOrigen = -1;

            for (int f = 0; f < 8; f++)
            {
                for (int c = 0; c < 8; c++)
                {
                    tablero[f, c] = 0;
                    if ((f + c) % 2 != 0)
                    {
                        if (f < 3) tablero[f, c] = 2;
                        if (f > 4) tablero[f, c] = 1;
                    }
                }
            }
            Console.WriteLine("----JUEGO DE DAMAS----");
            Console.Write("Quieres cargar la partida anterior? (s/n): ");
            string cargar = Console.ReadLine();

            if (cargar == "s" || cargar == "S")
            {
                if (File.Exists("Partida.txt"))
                {
                    StreamReader lector = new StreamReader("partida.txt");
                    turno = int.Parse(lector.ReadLine());
                    string linea = lector.ReadLine();

                    while (linea != null)
                    {
                        historial[Contadormovimientos] = linea;
                        Contadormovimientos++;

                        string[] datos = linea.Split(',');
                        int fo = int.Parse(datos[0]);
                        int co = int.Parse(datos[1]);
                        int fd = int.Parse(datos[2]);
                        int cd = int.Parse(datos[3]);

                        int ficha = tablero[fo, co];
                        tablero[fd, cd] = ficha;
                        tablero[fo, co] = 0;


                        if (Math.Abs(fd - fo) == 2)
                        {
                            int fm = (fo + fd) / 2;
                            int cm = (co + cd) / 2;
                            tablero[fm, cm] = 0;
                        }
                        linea = lector.ReadLine();
                    }
                    lector.Close();
                    Console.WriteLine("Partida cargada exitosamente. Presiona ENTER");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("No existe el aarchivo partida txt. Se jugara desde inicio.Presiona ENTER");
                    Console.ReadLine();
                }
            }

            bool jugando = true;

            while (jugando)
            {
                Console.Clear();
                TimeSpan tiempoJugado = DateTime.Now - tiempoInicio;

                Console.WriteLine("---JUEGO DE DAMAS---");
                Console.WriteLine("Tiempo transcurrido: {tiempoTranscurrido.Minutes:D2}:{TiempoTranscurrido.Seconds:D2}");
                Console.WriteLine("Controles: [Flechas] Moverse | [ENTER] Selecciona/Mover | [R] Rendirse\n");

                Console.WriteLine("   0 1 2 3 4 5 6 7");
                Console.WriteLine("-------------------");

                for (int f = 0; f < 8; f++)
                {
                    Console.ResetColor();
                    Console.Write(f + "| ");

                    for (int c = 0; c < 8; c++)
                    {
                        if (f == cursorFila && c == cursorcol)
                        {
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else if (f == filaOrigen && c == colOrigen)
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if ((f + c) % 2 == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        if (tablero[f, c] == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(". ");
                        }
                        else if (tablero[f, c] == 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("O ");
                        }
                        else if (tablero[f, c] == 2)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("X ");
                        }
                    }
                    Console.ResetColor();
                    Console.WriteLine();
                }
                Console.WriteLine("\n Turno del jugador" + turno);
                if (filaOrigen != -1)
                {
                    Console.Write("Ficha elegida en (" + filaOrigen + "," + colOrigen + "). Muevete al destino y presiona ENTER. ");
                }
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                if (tecla.Key == ConsoleKey.UpArrow)
                {
                    if (cursorFila > 0) cursorFila--;
                }
                else if (tecla.Key == ConsoleKey.DownArrow)
                {
                    if (cursorFila < 7) cursorFila++;
                }
                else if (tecla.Key == ConsoleKey.LeftArrow)
                {
                    if (cursorcol > 0) cursorcol--;
                }
                else if (tecla.Key == ConsoleKey.RightArrow)
                {
                    if (cursorcol < 7) cursorcol++;
                }
                else if (tecla.Key == ConsoleKey.R)

                {
                    int ganador = (turno == 1) ? 2 : 1;
                    Console.WriteLine("\n jugador " + turno + " se ha rendido!");
                    Console.WriteLine("GANA EL JUGADOR" + ganador + "!");
                    Console.WriteLine($"Tiempo total: {tiempoJugado.Minutes:D2}m {tiempoJugado.Seconds:D2}s");
                    jugando = false;
                    Console.ReadLine();
                    break;
                }

                else if (tecla.Key == ConsoleKey.Enter)
                {
                    if (filaOrigen == -1)
                    {
                        if (tablero[cursorFila, cursorcol] == turno)
                        {
                            filaOrigen = cursorFila;
                            colOrigen = cursorcol;

                            Console.WriteLine("\n--- Sentidos donde puedes moverte ---");
                            if (turno == 1)
                            {

                                if (colOrigen - 1 >= 0 && filaOrigen - 1 >= 0)
                                    Console.WriteLine("- Izquierda Arriba (fila: " + (filaOrigen - 1) + ", Col: " + (colOrigen - 1) + ")");
                                if (colOrigen + 1 <= 7 && filaOrigen - 1 >= 0)
                                    Console.WriteLine("- Derecha Arriba (fila: " + (filaOrigen - 1) + ", Col: " + (colOrigen + 1) + ")");
                            }

                            else
                            {
                                if (colOrigen - 1 >= 0 && filaOrigen + 1 >= 7)
                                    Console.WriteLine("-Izquierda Abajo (Fila: " + (filaOrigen + 1) + ", Col: " + (colOrigen - 1) + ")");
                                if (colOrigen + 1 >= 7 && filaOrigen + 1 >= 7)
                                    Console.WriteLine("-Derecha  Abajo (Fila: " + (filaOrigen + 1) + ", Col: " + (colOrigen + 1) + ")");
                            }
                            Console.WriteLine("Presiona ENETER para continuar y mover la ficha.");
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("\nAhi no hay una ficha tuya! Presiona ENTER para reintentar");
                            Console.ReadLine();
                        }

                    }
                    else
                    {
                        int FilaDestino = cursorFila;
                        int ColDestino = cursorcol;
                        bool movimientoValido = false;

                        if (tablero[FilaDestino, ColDestino] == 0)
                        {
                            int difFila = FilaDestino - filaOrigen;
                            int difCol = Math.Abs(ColDestino - colOrigen);

                            if (difCol == 1)
                            {
                                if (turno == 1 && difFila == -1)
                                {
                                    movimientoValido = true;
                                }
                                else if (turno == 2 && difFila == 1)
                                {
                                    movimientoValido = true;
                                }
                            }
                            else if (difCol == 2 && Math.Abs(difFila) == 2)
                            {
                                bool direccionCorrecta = (turno == 1 && difFila == -2) || (turno == 2 && difFila == 2);

                                if (direccionCorrecta)
                                {
                                    int FilaMedio = (filaOrigen + FilaDestino) / 2;
                                    int ColMedio = (colOrigen + ColDestino) / 2;
                                    int fichaEnemiga = (turno == 1) ? 2 : 1;

                                    if (tablero[FilaMedio, ColMedio] == fichaEnemiga)
                                    {
                                        tablero[FilaMedio, ColMedio] = 0;
                                        movimientoValido = true;
                                    }
                                }
                            }

                        }
                        if (movimientoValido)
                        {
                            tablero[FilaDestino, ColDestino] = tablero[filaOrigen, colOrigen];
                            tablero[filaOrigen, colOrigen] = 0;

                            historial[Contadormovimientos] = filaOrigen + "," + colOrigen + "," + FilaDestino + "," + ColDestino;
                            Contadormovimientos++;

                            Console.Write("Quieres guardar la partida actual?) (s/n): ");
                            string guardar = Console.ReadLine();

                            if (guardar == "s" || guardar == "S")
                            {
                                StreamWriter escritor = new StreamWriter("partida.txt");
                                escritor.WriteLine(turno == 1 ? 2 : 1);

                                for (int i = 0; i < Contadormovimientos; i++)
                                {
                                    escritor.WriteLine(historial[i]);
                                }
                                escritor.Close();
                                Console.WriteLine("Partida guardada!");
                            }

                            if (turno == 1) turno = 2;
                            else turno = 1;
                        }
                        else
                        {
                            Console.WriteLine("\nMovimiento no valido. Cancela seleccion. Presiona ENTER para intentarlo de nuevo");
                            filaOrigen = -1;
                            colOrigen = -1;
                            Console.ReadLine();

                        }
                    }
                }
            }
        }
    }
}