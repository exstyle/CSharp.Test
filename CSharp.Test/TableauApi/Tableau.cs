using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Resources;

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

        public string Style { get; set; }

    }

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

    public class TableauValeur
    {
        public Colonne Colonne { get; set; }

        public Ligne Ligne { get; set; }

        public double Value { get; set; }
    }

    internal static class TestExtension
    {
        public static Colonne Colonne (this Tableau tableau, string name,  int position = 0)
        {
            return tableau.Colonnes.Where(x => x.NomColonne == name).Skip(position).FirstOrDefault();
        }

        public static Colonne AddColonne(this Tableau tableau, string colonne)
        {
            Colonne currentColonne = colonne.ToCol();
            if (tableau.Colonnes == null)
                tableau.Colonnes = new List<Colonne>();

            currentColonne.Position = tableau.Colonnes.Count;
            tableau.Colonnes.Add(currentColonne);

            return currentColonne;

        }

        public static Ligne AddLigne(this Tableau tableau, string name)
        {
            if (tableau.Lignes == null)
                tableau.Lignes = new List<Ligne>();

            Ligne ligne = new Ligne(name);
            ligne.Position = tableau.Lignes.Count;
            ligne.Indentation = 0;
            ligne.Tableau = tableau;
            tableau.Lignes.Add(ligne);

            return ligne;

        }

        public static Ligne AddLigne(this Ligne ligne, string name)
        {
            Ligne currentLigne = new Ligne(name);
            currentLigne.Position = ligne.Position + 1;
            currentLigne.Indentation = ligne.Indentation;
            currentLigne.Tableau = ligne.Tableau;
            ligne.Tableau.Lignes.Add(currentLigne);

            return currentLigne;

        }

        public static Ligne AddChildLigne(this Tableau tableau, string name)
        {
            Ligne ligne = new Ligne(name);
            var last = tableau.Lignes.Last();
            ligne.Position = last.Position + 1;
            ligne.Indentation = last.Indentation + 1;
            ligne.Tableau = tableau;
            tableau.Lignes.Add(ligne);

            return ligne;

        }

        public static Ligne AddChildLigne(this Ligne ligne, string name)
        {
            Ligne currentLigne = new Ligne(name);
            currentLigne.Tableau = ligne.Tableau;
            currentLigne.Position = ligne.Position + 1;
            currentLigne.Indentation = ligne.Indentation + 1;
            ligne.Tableau.Lignes.Add(currentLigne);

            return currentLigne;

        }

        public static Colonne ToCol(this string name)
        {
            return new Colonne(name);
        }

        public static Ligne ToLigne(this string name)
        {
            return new Ligne(name);
        }

        public static void AddColonneStyle(this Colonne colonne, string style)
        {
            colonne.Style = style;
        }

        public static Ligne AddLigneStyle(this Ligne ligne, string style)
        {
            ligne.Style = style;
            return ligne;
        }

        public static void AddValue(this Tableau tab, Ligne ligne, Colonne colonnne, double value)
        {
            if (tab.Values == null)
                tab.Values = new List<TableauValeur>();

            tab.Values.Add(new TableauValeur() { Colonne = colonnne, Ligne = ligne, Value = value });
        }

    }

    public interface IDataManager
    {

    }

    public static class ManagerRun
    {
        public static void Run()
        {
            Manager ma = new Manager();
            var re = ma.LoadResults();
        }

    }

    public class Manager : IDataManager
    {

        public Tableau Tableau { get; set; } = new Tableau();

        public Manager()
        {
            Tableau.AddColonne(TableauRes.Tableau1ColonneTotal).AddColonneStyle("background:yellow");
            Tableau.AddColonne(TableauRes.Tableau1ColonneGaz);
            Tableau.AddColonne(TableauRes.Tableau1ColonneElec);
            Tableau.AddColonne(TableauRes.Tableau1ColonneAutre);
            Tableau.AddColonne(TableauRes.Tableau1ColonneTeleReleve);
            Tableau.AddColonne(TableauRes.Tableau1ColonneProfile);

            Tableau.AddLigne(TableauRes.LMargeBrute).
                AddChildLigne(TableauRes.LMargeBruteSem1).AddLigneStyle("background:red").
                AddLigne(TableauRes.LMargeBruteSem2).AddLigneStyle("background:red").
                AddLigne(TableauRes.LMargeBruteSem3).AddLigneStyle("background:red");

            Tableau.AddLigne(TableauRes.LMargeBruteSansTacite).AddLigneStyle("SuperStyle");
            Tableau.AddLigne(TableauRes.LMargeExtreme)
                .AddChildLigne(TableauRes.LMargeExtremeSem1)
                .AddLigne(TableauRes.LMargeExtremeSem2)
                .AddLigne(TableauRes.LMargeExtremeSem3)
                .AddLigne(TableauRes.LMargeExtremeSem4)
                .AddLigne(TableauRes.LMargeExtremeSem5)
                .AddLigne(TableauRes.LMargeExtremeSem6)
                    .AddChildLigne(TableauRes.LMargeExtremeSem6Lundi);

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
                results.Add(new TableauValeur()
                {
                    Colonne = Tableau.Colonne(TableauRes.Tableau1ColonneTotal),
                    Ligne = GetLigne(item.Item1),
                    Value = item.Item2
                });

                results.Add(new TableauValeur()
                {
                    Colonne = Tableau.Colonne(TableauRes.Tableau1ColonneGaz),
                    Ligne = GetLigne(item.Item1),
                    Value = item.Item2 - 1000
                });
            }

            return results;
        }

        public Ligne GetLigne(string ligneName)
        {
            return Tableau.Lignes.Where(x => x.NomLigne == ligneName).SingleOrDefault();
        }

        public Colonne GetColonneDeclaration(string name)
        {
            return new Colonne(name);
        }

        public Tableau LoadResults()
        {
            Tableau.Values = GetResultats();
            return Tableau;
        }
    }
}