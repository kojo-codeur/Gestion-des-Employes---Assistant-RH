# Gestion-des-Employes---Assistant-RH-->
![Version](https://img.shields.io/badge/version-1.0.0-blue.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-purple.svg)
![MySQL](https://img.shields.io/badge/MySQL-8.0+-orange.svg)
![License](https://img.shields.io/badge/license-MIT-green.svg)

# 🧑‍💼 Gestion des Employés - Assistant RH

> Application de gestion des présences, alertes, congés et assistant IA intégré (Groq API).

![Demo]("docs/application.gif")
*Animation de l'interface utilisateur*

## 📌 Fonctionnalités

- ✅ **Authentification** avec rôles (Utilisateur / Admin)
- ✅ **Pointage** entrée/sortie (avec détection retard/départ anticipé)
- ✅ **Gestion des absences** et justifications
- ✅ **Alertes** personnalisées avec CRUD (création, lecture, marquage lue)
- ✅ **Assistant IA** (via Groq Llama 3.1) – conseils RH, lecture base de données, actions admin sécurisées
- ✅ **Demandes de congé** avec validation admin
- ✅ **Jours fériés** configurables
- ✅ **Statistiques et graphiques** (inscriptions, présences par jour, etc.)
- ✅ **Interface moderne** (bulles de discussion, thème épuré)

## 🖼️ Captures d'écran

| Dashboard | Assistant IA |
|-----------|---------------|
|![Dashboard](docs/dashboard.png) | ![IA](docs/screenshot_1.png) |


## 📦 Prérequis

- Windows 10/11
- [.NET 10.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/mysql/) (ou MariaDB)
- Optionnel : [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) pour gérer la base

## 🚀 Installation

1. **Cloner le dépôt** :
   ```bash
   git clone https://github.com/VOTRE_USERNAME/Gestion-des-Employes.git
