# Web app for language learning
* Tables to display German adjective suffixes by choosing type.
* Settings for the connection between mood, tense, and person by languages.

## Technologies Used
* C#
* .NET 10
* ASP .NET core MVC
* SQL Server
* ASP .NET core Web API

## Archtecture Design
* Controller - Handles HTTP requests and responses
* Service - Contains business logic and workflow control
* Repository - Encapsulates data access and ORM operations
* Entity - Defines domain models and database structure

## Project Structure
Column A - First layer  
Column B - Second layer  
Column C - Third layer  
Purple -> Folders  
Green -> Files  
![Project Structure](Screenshots/Structure_20260209.png "Project Structure")

## Database Design
* EF Core Code First approach
* Table relationships and indexes are defined in the model configuration
* Each Entity maps directly to a database table
![DB Structure](Screenshots/DBStructure.jpeg "DBStructure")

## Request Flow Example
HTTP Request -> Controller -> Service -> Repository -> DbContext

## Using The App
### 1. ASP .NET core MVC
#### 1.1. Deutsch Adjective
To show suffixes of Gernman Adjectives and Articles by choosing a type before adjectives, then tables are displayed under the type condition, that shows the suffixes changes by cases and single/plural.  
##### 1.1.1. Choose type (All / Definite Article / Indefinite Article / No Article / Gentitive), then display tables that shows each German adjective suffix by genders and cases.  
![Deutsch Adjective](Screenshots/DeutschAdjective_20260318.png "Deutsch Adjective")
   
#### 1.2. Mood Setting      
In every language, verb forms are changing based on moods, tenses, and, persons. This function shows moods and applicable tenses and persons of each language.  
##### 1.2.1. Choose a language => It will bring out moods, tenses, persons of the language. Create button is enable, that is to add more moods and sebsequently set the applicable tenses and persons.  
##### 1.2.2. Choose a Mood => It will bring out the applicable tense and person. Modify button is enable, that is to change the setting of applicable tenses and persons.
![Mood Setting](Screenshots/MoodSetting_20260318.png "Mood Setting")

#### 1.3. Deutsch Verb/Français Verb
A verb has different form based on the mood, the tense, and the person. The relation between mood, tense, and person is defined in the function "Mood Setting".
In this funtion, users can search the verb from by entering a verb, choosing a mood and a tense, a table of each form by person is displayed when clicking on the "Search" button.
![Deutsch Verb](Screenshots/DeutschVerbSearch_20260531.png "Deutsch Verb")
![Français Verb](Screenshots/FrancaisVerbSearch_20260531.png "Français Verb")

While clicking on Create and Modify button, it goes to the page where can edit conjugation.
![Edit Conjugation](Screenshots/VerbEdit_20260531.png "Edit Conjugation")

### 2. ASP .NET core Web API
#### 2.1. Deutsch Adjective
API that correspond to 1.1, that can be called by other frontend App. (get only)

#### 2.2. Mood Setting
API that correspond to 1.2, that can be called by other frontend App.

#### 2.3. Deutsch Verb/Français Verb
API that correspond to 1.3, that can be called by other frontend App.
   
## Upcoming Changes
* Deutsch Verb: Add Verb details
