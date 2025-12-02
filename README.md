# Projet de Session – Application WinUI 3  
### Développement d’applications Windows – Automne 2025  
**Étudiant : Christophe Merlen**

---

## Résumé du projet

Ce projet a pour objectif de développer une application de bureau moderne en **WinUI 3**, permettant la gestion simplifiée des élections municipales de la Ville de Gatineau.

L’application utilise une architecture **MVVM**, une base de données **SQLite** gérée avec **Entity Framework Core**, et inclut plusieurs modules :

- Gestion des **districts électoraux**
- Gestion des **candidats**
- Gestion des **électeurs**
- Navigation via **NavigationView**
- Validation des entrées utilisateur
- Calcul automatique du gagnant d’un district
- Tests unitaires sur les classes du projet Data

Le projet permet d’appliquer les notions vues en cours : architecture logicielle, data binding, validation, séparation en projets, EF Core, tests unitaires, et interface WinUI 3.

---

## Division des tâches

### **Christophe Merlen**
Projet réalisé **seul**, conformément aux consignes.

Responsabilités :

- Architecture générale du projet WinUI  
- Développement des pages principales : Districts, Candidats, Électeurs  
- Mise en place du pattern **MVVM** avec CommunityToolkit.MVVM  
- Création du projet **Data**  
- Implémentation du **DbContext** et des **modèles EF Core**  
- Migrations et génération de la base SQLite  
- Développement du CRUD complet  
- Validation des champs et gestion des erreurs  
- Navigation entre pages  
- Tests unitaires (xUnit) sur la couche Data  
- Diagrammes UML et documentation (Avec l'aide de ChatGPT)

---

## Tests unitaires

5 tests unitaires ont été réalisés avec **xUnit**, conformément aux instructions de la professeure.

Ils portent **uniquement sur le projet Data**, et non sur WinUI, afin d’éviter les incompatibilités :

- Vérification de l’assignation des propriétés des modèles  
- Tests sur la valeur par défaut des champs  
- Tests sur la validité des données d’un district  
- Tests sur les objets Candidat et Electeur  

Tous les tests réussissent.

---

## Fonctionnalités principales

- Application WinUI 3 moderne et stable  
- Architecture **MVVM** complète  
- Base de données **SQLite**  
- CRUD complet sur Districts, Candidats et Électeurs  
- Validation dynamique des formulaires  
- Calcul LINQ du gagnant d’un district  
- Navigation fluide entre les pages  
- Tests unitaires fonctionnels  
- Interface simple et propre  
- Données initiales intégrées (seed)  

---

## Diagramme de cas d’utilisation

```mermaid
flowchart TD

A[Utilisateur] -->|Consulter| B[Afficher les districts]
A -->|CRUD complet| C[Gestion des districts]

A -->|Consulter| D[Liste des candidats]
A -->|Ajouter / Modifier / Supprimer| E[Gestion des candidats]

A -->|Consulter| F[Liste des électeurs]
A -->|CRUD complet| G[Gestion des électeurs]

B --> H[Voir les candidats du district]
H --> I[Calcul du gagnant via LINQ]

---

# Diagramme UML de classes (Mermaid)**

```md
## Diagramme de classes

```mermaid
classDiagram

class DistrictElectoral {
    int DistrictElectoralId
    string NomDistrict
    int Population
}

class Candidat {
    int CandidatId
    string Nom
    string PartiPolitique
    int VotesObtenus
    int DistrictElectoralId
}

class Electeur {
    int ElecteurId
    string Nom
    string Adresse
    DateTime DateNaissance
    int DistrictElectoralId
}

DistrictElectoral "1" --> "many" Candidat : possède >
DistrictElectoral "1" --> "many" Electeur : contient >

---

## Conclusion

Le projet remplit toutes les exigences du travail de session :  
architecture MVVM, séparation des projets, base de données relationnelle, interface WinUI moderne, validation, navigation, LINQ, et tests unitaires fonctionnels.

Le résultat final est une application structurée, robuste et facile à maintenir.

