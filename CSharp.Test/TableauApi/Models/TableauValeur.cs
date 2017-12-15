using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Test.TableauApi.Models
{
    public class TableauValeur
    {

        public Colonne Colonne { get; set; }

        public Ligne Ligne { get; set; }

        public double Value { get; set; }
    }
}
