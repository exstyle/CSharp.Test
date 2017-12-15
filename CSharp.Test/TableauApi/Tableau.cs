using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.Test.TableauApi
{
    
    public class Tableau
    {
        public List<Ligne> Lignes { get; set; }

        public List<Colonne> Colonnes { get; set; }

        public List<TableauValeur> Values { get; set; }
    }
    
    public class Colonne
    {
        public Colonne(string nomColonne)
        {
            NomColonne = nomColonne;
        }

        public string NomColonne { get; set; }

        public int Position { get; set; }
    }

    public class Ligne
    {
        public Ligne(string nomLigne)
        {
            NomLigne = nomLigne;
        }

        public string NomLigne { get; set; }

        public int Position { get; set; }
    }

    public class TableauValeur
    {
        public Colonne Colonne { get; set; }

        public Ligne Ligne { get; set; }

        public double Value { get; set; }
    }

    internal static class TestExtension
    {
        public static Tableau AddColonne(this Tableau tableau, Colonne colonne)
        {
            if (tableau.Colonnes == null)
                tableau.Colonnes = new List<Colonne>();

            colonne.Position = tableau.Colonnes.Count;
            tableau.Colonnes.Add(colonne);

            return tableau;

        }

        public static Tableau AddLigne(this Tableau tableau, Ligne ligne)
        {
            if (tableau.Lignes == null)
                tableau.Lignes = new List<Ligne>();

            ligne.Position = tableau.Colonnes.Count;
            tableau.Lignes.Add(ligne);

            return tableau;

        }

        public static void AddColonneStyle(this Tableau tableau, string colonneName)
        {

        }

        public static void AddValue(this Tableau tab, Ligne ligne, Colonne colonnne, double value)
        {
            if (tab.Values == null)
                tab.Values = new List<TableauValeur>();

            tab.Values.Add(new TableauValeur() { Colonne = colonnne, Ligne = ligne, Value = value });
        }

        public static Ligne GetLigne (this string name)
        {
            return new Ligne("rjkajrkaj");
        }
    }

    public interface IDataManager
    {

    }
    public class Manager : IDataManager
    {
        public Colonne cTotal = new Colonne("Total");
        public Colonne cGaz = new Colonne("Gaz");
        public Colonne cProfile = new Colonne("Profile");
        public Colonne cTeleReleve = new Colonne("TeleReleve");
        public Colonne cAutre = new Colonne("Autre");
        public Colonne cService = new Colonne("Service");

        public Ligne lMargeBrute = new Ligne("Marge brute 2017");
        public Ligne lMargeBruteSansTacite = new Ligne("Marge brute sans tacite 2017");
        public Ligne lMargeExtreme = new Ligne("Marge extrême sur année 2017");

        public Tableau tableau { get; set; }

        public Manager()
        {
            tableau = new Tableau();
            tableau.AddColonne(cTotal).AddColonneStyle("background:yellow");
            tableau.AddColonne(cGaz);
            tableau.AddColonne(cProfile);
            tableau.AddColonne(cTeleReleve);
            tableau.AddColonne(cAutre);
            tableau.AddColonne(cService);

            tableau.AddLigne(lMargeBrute);
            tableau.AddLigne(lMargeBruteSansTacite);
            tableau.AddLigne(lMargeExtreme);
        }

        public List<TableauValeur> GetResultats()
        {
            List<TableauValeur> results = new List<TableauValeur>();

            List<Tuple<string, double>> query = new List<Tuple<string, double>>() {
                                        { new Tuple<string, double>("Marge brute 2017", 2010) },
                                        { new Tuple<string, double>("Marge brute sans tacite 2017", 2015) },
                                        { new Tuple<string, double>("Marge extrême sur année 2017", 2020) }};
            
            foreach (var item in query)
            {
                results.Add(new TableauValeur() { Colonne = cTotal, Ligne = item.Item1.GetLigne() });
            }
            
            return results;
        }
        
        public void LoadResults()
        {
            tableau.Values = GetResultats();
        }
    }
}

