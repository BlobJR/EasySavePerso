# 📘 Documentation Utilisateur - EasySave

## EasySave - Guide Utilisateur

### 📝 Introduction
EasySave est une application de sauvegarde permettant d'exécuter des sauvegardes complètes ou différentielles de fichiers et dossiers. L'application propose une interface graphique simple ainsi que des options avancées comme le chiffrement des fichiers et la gestion des logs.

---

## 🖥 Installation
### Prérequis :
- Windows 10 / 11
- .NET 8.0 installé
- Autorisations administratives pour accéder aux fichiers protégés

### Étapes d'installation :
1. Télécharger le fichier d’installation `EasySave.zip`.
2. Extraire le contenu dans un dossier.
3. Exécuter `EasySave.exe`.

---

## 🏠 Interface Utilisateur
L'interface principale est divisée en plusieurs sections :

- **Boutons d'action** : Ajouter, modifier, supprimer et exécuter des sauvegardes.
- **Liste des Jobs** : Affiche toutes les sauvegardes configurées.
- **Console Log** : Affiche les actions en temps réel et les erreurs éventuelles.
- **Options** : Changer la langue, configurer le format des logs, gérer le chiffrement.

---

## 📂 Gestion des Sauvegardes
### ➕ Ajouter une sauvegarde
1. Cliquer sur **"Ajouter"**.
2. Renseigner :
   - **Nom du job**.
   - **Chemin source** (dossier à sauvegarder).
   - **Chemin destination** (dossier où sera copiée la sauvegarde).
   - **Type de sauvegarde** (1 = complète, 2 = différentielle).
3. Valider.

### ✏ Modifier une sauvegarde
1. Sélectionner un job dans la liste.
2. Cliquer sur **"Modifier"**.
3. Modifier les champs nécessaires.
4. Valider.

### ❌ Supprimer une sauvegarde
1. Sélectionner un job dans la liste.
2. Cliquer sur **"Supprimer"**.
3. Confirmer la suppression.

---

## ▶ Exécution des Sauvegardes
### 🔹 Exécuter un job unique
1. Sélectionner un job.
2. Cliquer sur **"Exécuter"**.

### 🔹 Exécuter plusieurs jobs simultanément
1. Sélectionner plusieurs jobs (**CTRL + Clic**).
2. Cliquer sur **"Exécuter Sélection"**.

---

## 🔒 Chiffrement et Déchiffrement
### 🔹 Activer le chiffrement
1. Cliquer sur **"Chiffrer"**.
2. Saisir :
   - **Clé de chiffrement** (mot de passe pour protéger les fichiers).
   - **Extensions de fichiers à chiffrer** (ex: `.txt`, `.pdf`).
3. Valider.

### 🔹 Déchiffrer des fichiers
1. Cliquer sur **"Déchiffrer"**.
2. Choisir :
   - **Un job existant** (restaurer les fichiers chiffrés d’une sauvegarde).
   - **Un chemin spécifique** (restaurer un dossier précis).
3. Valider.

---

## 📄 Gestion des Logs
Les logs permettent de suivre les actions effectuées par EasySave.

### 🔹 Changer le format des logs
- Cliquer sur **"JSON"** ou **"XML"** selon le format souhaité.

### 🔹 Accéder aux logs
1. Ouvrir le dossier des logs : `C:\ProgramData\EasySave\logs`
2. Ouvrir le fichier correspondant à la date du jour.

---

## 🔧 Options Avancées
### 🔹 Changer la langue
- Cliquer sur **"FR"** ou **"EN"**.

### 🔹 Configurer un logiciel métier
Certains logiciels bloquent la sauvegarde pour éviter des conflits :
1. Ajouter le logiciel dans la configuration (`Settings.json`).
2. EasySave mettra en pause la sauvegarde si le logiciel est en cours d’exécution.

---

## ❓ FAQ & Dépannage

### ❌ Problèmes courants

| Problème | Solution |
|----------|---------|
| Impossible d’ajouter un job | Vérifier que les champs sont remplis correctement. |
| Message "Job introuvable" | Vérifier que le nom du job est bien saisi. |
| Fichiers non sauvegardés | Vérifier les permissions et l’espace disque disponible. |
| Chiffrement non fonctionnel | Vérifier que la clé de chiffrement est correcte. |

---

## 🛠 Support Technique

📧 **Email** : [support@easysave.com](mailto:support@easysave.com)  
📖 **Documentation en ligne** : [www.easysave.com/docs](https://www.easysave.com/docs)  
💬 **Forum** : [forum.easysave.com](https://forum.easysave.com)  

---

📌 **EasySave - Version 2.0**  
📝 **Dernière mise à jour** : *2025-02-16*
