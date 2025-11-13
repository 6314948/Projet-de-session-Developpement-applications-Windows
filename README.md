# Projet de session – Développement d’applications Windows (420-3P3-HU)
### Automne 2025  
**Étudiant :** Christophe Merlen  
**Remise :** 4 décembre 2025  

---

## Description du projet

Ce projet consiste à développer une **application Windows (WinUI 3)** qui permet de **gérer les élections municipales de la Ville de Gatineau**.  
L’application utilise **Entity Framework Core** pour gérer une petite base de données SQLite.  

L’objectif est de démontrer la capacité à concevoir une application complète en suivant le modèle **MVVM**, avec une interface fonctionnelle, un accès aux données, et des tests unitaires.

Le thème vient d’un projet précédent fait en SQL, mais ici le but est de **simplifier le modèle** pour se concentrer sur la logique et l’interface.

---

## Fonctionnalités principales

- Gestion des **districts électoraux**
- Gestion des **candidats**
- Gestion des **électeurs**
- Affichage des liens entre les entités (ex. : candidats d’un district)
- Calcul du **candidat gagnant** par district (LINQ)
- Validation des champs et messages d’erreur en français
- Une page respecte les **règles d’accessibilité** (contraste, tabulation, labels clairs)

---

## Modèles de données

L’application contient trois modèles principaux :

| Classe | Description | Exemple de propriétés |
|--------|--------------|------------------------|
| **DistrictElectoral** | Représente un district de la ville | `DistrictId`, `NomDistrict`, `Population` |
| **Candidat** | Représente un candidat aux élections | `CandidatId`, `Nom`, `PartiPolitique`, `VotesObtenus`, `DistrictId` |
| **Electeur** | Représente un électeur inscrit | `ElecteurId`, `Nom`, `Adresse`, `DateNaissance`, `DistrictId` |

Les relations sont gérées par **Entity Framework Core** :
- Un **district** peut avoir plusieurs **candidats** et **électeurs**.  
- Chaque **candidat** et **électeur** appartient à un seul **district**.

---

## Structure du projet
```
ProjetElectionsWinUI/
│
├── Models/ → Classes C# (District, Candidat, Electeur)
├── Data/ → DbContext + Migrations
├── ViewModels/ → Logique MVVM
├── Views/ → Pages XAML (Districts, Candidats, Électeurs)
├── Tests/ → Tests unitaires
└── README.md → Ce fichier
```
