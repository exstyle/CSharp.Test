using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Test.TableauApi.Models
{
    public class Tableau
    {
        public List<Ligne> Lignes { get; set; }

        public List<Colonne> Colonnes { get; set; }

        public List<TableauValeur> Values { get; set; }

        public string THeadClass { get; set; }

        public string TBodyClass { get; set; }
    }
}
