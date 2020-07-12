using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryManagerLib
{
    /// <summary>
    /// Klasa <c>Settings</c> zawiera parametry możliwych do skonfigurowania przez użytkownika elementów interfejsu programu.
    /// </summary>
    public class Settings
    {
        private int menuResultsPerScreen;

        /// <summary>
        /// Parametr <c>MenuResultsPerScreen</c> przechowuje maksymalną ilość wyników wyświetlanych na ekranie.
        /// </summary>
        public int MenuResultsPerScreen
        {
            get => menuResultsPerScreen;

            set
            {
                if (value > 0)
                    menuResultsPerScreen = value;
                else
                    throw new ArgumentException("Na stronie musi pojawiać się przynajmniej jeden wynik");
            }
        }

        /// <summary>
        /// Konstruktor klasy <c>Settings</c>.
        /// </summary>
        /// <param name="menuResultsPerScreen">Ilość wyników wyświetlanych na ekranie</param>
        public Settings(int menuResultsPerScreen = 5)
        {
            MenuResultsPerScreen = menuResultsPerScreen;
        }

    }
}
