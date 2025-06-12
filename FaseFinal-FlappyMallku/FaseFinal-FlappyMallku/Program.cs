using System;

namespace Flappymallku
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                using (var game = new Game1())
                    game.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepción fatal: " + ex);
                Console.ReadLine(); // Mantiene la consola abierta para que puedas ver el error
            }
        }
    }
}

