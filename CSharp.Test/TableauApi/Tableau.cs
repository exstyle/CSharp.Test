using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Resources;
using CSharp.Test.TableauApi.Models;

namespace CSharp.Test.TableauApi
{
    public static class ManagerRun
    {
        public static void Run()
        {
            Manager ma = new Manager();
            var re = ma.LoadResults();
        }

    }

    public class Manager 
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
                    Ligne = Tableau.Ligne(item.Item1),
                    Value = item.Item2
                });

                results.Add(new TableauValeur()
                {
                    Colonne = Tableau.Colonne(TableauRes.Tableau1ColonneGaz),
                    Ligne = Tableau.Ligne(item.Item1),
                    Value = item.Item2 - 1000
                });
            }

            return results;
        }
        
        public Tableau LoadResults()
        {
            Tableau.Values = GetResultats();
            return Tableau;
        }
    }
}