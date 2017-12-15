using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using CSharp.Test.TableauApi.Models;

namespace CSharp.Test.TableauApi
{
    internal static class TableauApi
    {
        public static Colonne Colonne(this Tableau tableau, string name, int position = 0)
        {
            return tableau.Colonnes.Where(x => x.NomColonne == name).Skip(position).FirstOrDefault();
        }

        public static Ligne Ligne(this Tableau tableau, string name, int position = 0)
        {
            return tableau.Lignes.Where(x => x.NomLigne == name).Skip(position).FirstOrDefault();
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
}
