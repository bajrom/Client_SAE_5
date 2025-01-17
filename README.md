# Client de Visualisation et Gestion des Données Capteurs et Structures

Ce projet est une application permettant de visualiser et gérer les données de capteurs et de structures, ainsi que d’effectuer des prédictions basées sur l’intelligence artificielle.

## Structure

### Backend :
- **Technologies** :
  - .NET 8
  - Base de données distante PostgreSQL sur AlwaysData pour les données de structure.
  - API C# Restful :
    [https://api-ovhiutannecy-sae5-gwbxhugwhjb2aqdu.canadacentral-01.azurewebsites.net/api/](https://api-ovhiutannecy-sae5-gwbxhugwhjb2aqdu.canadacentral-01.azurewebsites.net/api/)
  - Base de données locale InfluxDB pour les données de capteurs sur Raspberry Pi.

### Frontend :
- **Technologies** :
  - Blazor pour l’interface utilisateur.
  - Babylon.js pour la visualisation 3D des salles.
    - dans le systeme 3d, on retrouve : un moteur 3d -> babylone.js, des modeles 3d modélisé sous blender 4.1 et 4.2.
    - dans les modèles 3d on retrouve: une fenetre, une vitre, une porte, un radiateur et deux capteurs. Ces modeles sont implémentés dans la visualisation, une ou pluisieurs fois.
  - Grafana pour la visualisation des données capteurs.


### Autres :
- API de prédiction IA avec des données fictives :
  [http://10.103.101.128:5173/api/InfluxData/data/](http://10.103.101.128:5173/api/InfluxData/data/)

---

## Prérequis
- Une Raspberry Pi connecté à l'IUT en salle D101.
    - Identifiant: boo.
    - Mot de passe : 1234.
    - Pour lire la suite d'instruction de la Raspberry faite la commande cd ~/Desktop/SAE_5A.01/Capiteur/Capiteur_SAE5 et le Readme se trouve dedans.
- Avoir **Visual Studio 2022** installé.
- Disposer d'un navigateur web.

Démarrer (projet en HTTP):
Lancer le projet :
dotnet run dans Client_SAE_5

Lancer les tests :
cd PlaywrightE2ETests
dotnet build
bin/Debug/net8.0/playwright.ps1 install
Appuyer sur les touches [CTRL + F5]
Appuyer sur les touches [CTRL + R, A] ou avec l'invite de commande: dotnet test



L'application contient une page d'acceuil, un menu général (en haut) pour naviguer vers : "Gestion CRUD", "Grafana", "Alarme", "Visualisation 3D", "Données capteurs"

# Gestion CRUD:

    dans la naviguation de la gestion CRUD (/crud) un menu de naviguation (à gauche) permet de naviguer vers les difféntes tables de la BD, par défaut sur les capteurs (/crud/capteurs).

    Chaque tables à sa page de gestion et de détails sauf UniteCapteur qui est la seule table de liaison qui est géré dans add et update de capteurs.

    Chaque tables contient une page pour ajouter (popup), modifier (le même popup), supprimer (avec confirmation) et ainsi qu'une autre page pour avoir plus de détails.

    Chaque page générale contient des informations clé qui est une Grid blazor bootstrap qui est traduit en français car le texte est de base en anglais (méthode GridFiltersTranslationProvider à \Utils\BlazorBootstrapUtils.cs). Quand un élément est ajouter, modifier et supprimer un Toasts de bootstrap est affiché pour confirmer. Pendant qu'un élément est en train d'être ajouter ou modifier une popup Modal bootstrap est afficher. Une popup ConfirmDialog de suppretion demande la confirmation pour suppretion d'un élément.

    Vérification si les coordonnées x et y ne sont pas en dehors du mur dans add/update des capteurs et équipements, cela nécessite de prendre les détails du mur sélectionner (fetch by id) pour avoir la longueur et hauteur du mur.
    
    La page de détail des équipements, des capteurs, des murs donnent une visualisation CSS de leur positions et tailles. La page de détail d'une salle donne une visualisation de la salle avec un générateur de SVG (méthode GenererPlanSalleSVG à \Utils\SVG_Generator.cs)

# Grafana:

    Connexion au dashboard public avec un iframe

# Alarme:

    Alarme est une page entièrement en CSS qui permet d'allumer (fictivement) une alarme. Pour allumer l'allarme, une popup ConfirmDialog demande confirmation. Une popup Toasts bootstrap confirme que l'alarme est allumé ou éteinte

# Page de visualisation 3D:

    La page est un canva utilisant babylon.js pour afficher les modèles 3D

# Page de prédiction de l'IA:

    Pour la page, utilisation boostrap d'un spinner, des input et de LineChart.
    Le ligne chart est définie au chargement de la page comme vide, puis lorsque l'utilisateur sélectionne la data, il est remplit.
