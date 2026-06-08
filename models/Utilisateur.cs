using System;

namespace Gestion_des_Employés.models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Sexe { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        public string Departement { get; set; }
        public DateTime? DateEmbauche { get; set; }
        public decimal? Salaire { get; set; }
        public string Statut { get; set; }
        public int NbAbsences { get; set; }
        public DateTime DateCreation { get; set; }

        public byte[] Avatar { get; set; }

        // Constructeur par défaut
        public Utilisateur() { }

        // Constructeur avec paramètres principaux
        public Utilisateur(int id, string nom, string prenom, string email, string role, string statut)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Role = role;
            Statut = statut;
        }

        // Méthode pour obtenir le nom complet
        public string NomComplet => $"{Nom} {Prenom}".Trim();
    }
}