Client de Visualisation et gestion des données capteurs et structures

Ce projet est une application permettant de visualiser et gérer les données de capteurs et de structures, ainsi que d’effectuer des prédictions basées sur l’intelligence artificielle.

Structure :
    Backend :
        .NET 8
        BD distante PostgreSQL sur Always data pour les données de structure ainsi que son API C# Restful (EndPoints: https://api-ovhiutannecy-sae5-gwbxhugwhjb2aqdu.canadacentral-01.azurewebsites.net/api/)
        BD locale InfluxDB pour les données de capteurs sur RaspberryPI
    Frontend :
        Blazor pour l’interface utilisateur
        Babylon.js pour la visualisation 3D des salles
        Grafana pour la visualisation des données capteurs
    Autres :
        API de prédiction IA avec des données fictives (EndPoints: http://10.103.101.128:5173/api/InfluxData/data/)

Prérequis:
    RaspberryPI connecté à l'IUT
    Avoir Visual studio 2022, un navuguateur

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

Gestion CRUD:
    dans la naviguation de la gestion CRUD (/crud) un menu de naviguation (à gauche) permet de naviguer vers les difféntes tables de la BD, par défaut sur les capteurs (/crud/capteurs).

    Chaque tables à sa page de gestion et de détails sauf UniteCapteur qui est la seule table de liaison qui est géré dans capteur.

    Chaque tables contient une page pour ajouter (popup), modifier (le même popup), supprimer (avec confirmation) et ainsi qu'une autre page pour avoir plus de détails.

    Chaque page générale contient des informations clé qui est une Grid blazor bootstrap qui est traduit en français car le texte est de base en anglais (méthode GridFiltersTranslationProvider à \Utils\BlazorBootstrapUtils.cs). Quand un élément est ajouter, modifier et supprimer un Toasts de bootstrap est affiché pour confirmer. Pendant qu'un élément est en train d'être ajouter ou modifier une popup Modal bootstrap est afficher. Une popup ConfirmDialog de suppretion demande la confirmation pour suppretion d'un élément.
    
    La page de détail des équipements, des capteurs, des murs donnent une visualisation CSS de leur positions et tailles. La page de détail d'une salle donne une visualisation de la salle avec un générateur de SVG (méthode GenererPlanSalleSVG à \Utils\SVG_Generator.cs)

Grafana:
    Connexion au dashboard public avec un iframe

Alarme:
    Alarme est une page entièrement en CSS qui permet d'allumer (fictivement) une alarme. Pour allumer l'allarme, une popup ConfirmDialog demande confirmation. Une popup Toasts bootstrap confirme que l'alarme est allumé ou éteinte

Licence
Ce projet est sous licence MIT.