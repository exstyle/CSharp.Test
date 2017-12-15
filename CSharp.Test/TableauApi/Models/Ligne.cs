using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Test.TableauApi.Models
{
    public class Ligne
    {
        public Ligne(string nomLigne)
        {
            NomLigne = nomLigne;
        }

        public string NomLigne { get; set; }

        public int Position { get; set; }

        public int Indentation { get; set; }

        public string Style { get; set; }

        public Tableau Tableau { get; set; }
    }
}
