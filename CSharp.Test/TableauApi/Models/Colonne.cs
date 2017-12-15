using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Test.TableauApi.Models
{
    public class Colonne
    {
        public Colonne(string nomColonne)
        {
            NomColonne = nomColonne;
        }

        public string NomColonne { get; set; }

        public int Position { get; set; }

        public string Style { get; set; }
    }
}
