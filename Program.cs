using System;
using System.ComponentModel;
using System.IO;

namespace DAMAS
{
    class program
    {
        static void Main(string[] args)
        {
            int[,] tablero = new int[8, 8];
            string[] historial = new string[100];
            int Contadormovimientos = 0;

            int turno = 1;
            
            for (int f = 0; f < 8; f++)
            {
                for (int c = 0; c < 8; c++)
                {
                    tablero[f, c] = 0;
                    if ((f + c) % 2 != 0)
                    {
                        if (f < 3)
                        { 
                            tablero[f,c] = 2;
                        }
                        if (f > 4 )
                        {
                            tablero[f,c] = 1;
                        }
                    }
                }
            }
            Console.WriteLine("----JUEGO DE DAMAS----");
            Console.Write("Quieres cargar la partida anterior? (s/n): ");
            string cargar = Console.ReadLine();

           if (cargar =="s"   || cargar == "S")
            {
                if(File.Exists("Partida.txt"))
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

                        int ficha = tablero[fo,co];
                        tablero[fd, cd] = ficha; 
                        tablero[fo, co] = 0;


                        if (Math.Abs(fd - fo) ==2)
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
                    Console.ReadLine();
                }
            }
            bool jugando = true;

            while (jugando)
            {
                Console.Clear();
                Console.WriteLine("   0 1 2 3 4 5 6 7");
                Console.WriteLine("-------------------");

                for (int f = 0; f < 8; f++)
                {
                    Console.ResetColor();
                    Console.Write(f + "| ");

                    for (int c = 0; c < 8; c++)
                    {
                        if ((f + c) % 2 == 0)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
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

                Console.Write("Fila de la ficha a mover: ");
                int filaOrigen = int.Parse(Console.ReadLine());
                Console.Write("Columa de la ficha a elegir: ");
                int colOrigen = int.Parse(Console.ReadLine());

                if (tablero[filaOrigen, colOrigen] != turno)
                {
                    Console.WriteLine("Ahi no hay uan ficha tuya! presiona ENTER para reintentar.");
                    Console.ReadLine();
                    continue;
                }


            Console.WriteLine("\n--- Sentidos donde puedes moverte ---");
            if (turno == 1)
                {
                    if (colOrigen - 1 >= 0  && filaOrigen - 1 >=0)
                       Console.WriteLine("- Izquierda Arriba (fila: " + (filaOrigen - 1) + ", Col: " + (colOrigen - 1) + ")");
                    if (colOrigen + 1 <=7 && filaOrigen - 1 >=0)
                    Console.WriteLine("- Derecha Arriba (fila: " + (filaOrigen - 1) + ", Col: " + (colOrigen + 1) + ")");
                }
                else
                {
                    if (colOrigen - 1 >= 0  && filaOrigen + 1 >=7)
                    Console.WriteLine("-Izquierda Abajo (Fila: " + (filaOrigen + 1) + ", Col: " + (colOrigen - 1) + ")");
                    if (colOrigen + 1 >= 7  && filaOrigen + 1 >=7)
                    Console.WriteLine("-Derecha  Abajo (Fila: " + (filaOrigen + 1) + ", Col: " + (colOrigen + 1) + ")");
                }

                Console.Write("\nFila destino: ");
                int FilaDestino = int.Parse(Console.ReadLine());
                Console.Write("Columna destino: ");
                int ColDestino = int.Parse(Console.ReadLine());

                bool movimientoValido = false;

                if (tablero[FilaDestino, ColDestino] == 0)
                {
                    if (turno == 1 && FilaDestino == filaOrigen - 1 && Math.Abs(ColDestino - colOrigen) == 1)
                    {
                        movimientoValido = true;
                    }
                    else if(turno == 2 && FilaDestino == filaOrigen + 1 && Math.Abs(ColDestino - colOrigen) == 1)
                    {
                        movimientoValido = true;
                    }
                    
                    if (Math.Abs(FilaDestino - filaOrigen) == 2 && Math.Abs(ColDestino - colOrigen) ==2)
                    {
                        int FilaMedio = (filaOrigen + FilaDestino) / 2;
                        int ColMedio = (colOrigen + ColDestino) / 2;

                        if (tablero[FilaMedio, ColMedio] != 0 && tablero[FilaMedio,ColMedio] != turno)
                        {
                            tablero[FilaMedio,ColMedio] = 0; 
                            movimientoValido = true;
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

                    if (guardar == "s"  || guardar == "S")
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

                    if (turno == 1) turno= 2;
                    else turno= 1;
                }
                else
                {
                    Console.WriteLine("Movimiento no valido. Presiona ENTER para intentarlo de nuevo");
                    Console.ReadLine();
                }
           }
        }
    }
}   