using System;

namespace Bejeweled
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Bejeweled game = new Bejeweled())
            {
                game.Run();
            }
        }
    }
#endif
}

