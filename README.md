﻿﻿
# BookStore

**Inhaltsverzeichnis:**

- [BookStore](#bookstore)
  - [Lehrziele](#lehrziele)
  - [Einleitung](#einleitung)
  - [Datenmodell und Datenbank](#datenmodell-und-datenbank)
    - [Definition von ***Book***](#definition-von-book)
  - [Aufgaben](#aufgaben)
    - [Geschäftsregeln](#geschäftsregeln)
    - [Prüfung der ISBN-Nummern](#prüfung-der-isbn-nummern)
      - [Beispiele](#beispiele)
  - [Datenimport](#datenimport)
  - [Umsetzung der Aufgabe mit dem SETemplate](#umsetzung-der-aufgabe-mit-dem-setemplate)
    - [Schritt 1: Repository klonen](#schritt-1-repository-klonen)
    - [Schritt 2: `SETemplate` mit der IDE öffnen](#schritt-2-setemplate-mit-der-ide-öffnen)
    - [Schritt 3: Starten der Anwendung `TemplateTools.ConApp`](#schritt-3-starten-der-anwendung-templatetoolsconapp)
    - [Schritt 4: `SETemplate` kopieren =\> `SEBookStore`](#schritt-4-setemplate-kopieren--sebookstore)
    - [Schritt 5: `SEBookStore` mit der IDE öffnen](#schritt-5-sebookstore-mit-der-ide-öffnen)
    - [Schritt 6: Starten der Anwendung `TemplateTools.ConApp`](#schritt-6-starten-der-anwendung-templatetoolsconapp)
    - [Schritt 7: `Preprocessor` einstellen](#schritt-7-preprocessor-einstellen)
    - [Schritt 8: Entity-`Book` erstellen](#schritt-8-entity-book-erstellen)
    - [Schritt 9: Validierung für das Entity-`Book` erstellen](#schritt-9-validierung-für-das-entity-book-erstellen)
    - [Schritt 10: Starten der `CodeGenerierung`](#schritt-10-starten-der-codegenerierung)
  - [Erweiterungen nach der CodeGenerierung](#erweiterungen-nach-der-codegenerierung)
    - [Erweiterung: Datenimport](#erweiterung-datenimport)
    - [Erweiterung MVVMApp](#erweiterung-mvvmapp)
      - [Erweiterung MVVMApp-Listen-Ansicht](#erweiterung-mvvmapp-listen-ansicht)
      - [Erweiterung MVVMApp-Einzel-Ansicht](#erweiterung-mvvmapp-einzel-ansicht)
    - [Testen der MVVMApp](#testen-der-mvvmapp)

---

## Lehrziele

- Umsetzung der SE-Architektur mit dem SE-Template
- Prüfen von Geschäftsregeln
- Importieren von csv-Daten

## Einleitung

Das Projekt ***SEBookStore*** ist eine datenzentrierte Anwendung zur Verwaltung von Büchern. Diese Anwendung wird mit dem [**SE-Template**](https://github.com/leoggehrer/SETemplate) erstellt. Das Template ist eine Vorlage für die Erstellung von .NET-Projekten und enthält bereits alle notwendigen Komponenten, um eine vollständige Anwendung zu erstellen.

## Datenmodell und Datenbank

Das Datenmodell für ***BookStore*** hat folgenden Aufbau:

```txt
+-------+--------+ 
|                | 
|      Book      + 
|                | 
+-------+--------+ 
```

### Definition von ***Book***

| Name          | Type   | MaxLength | Nullable |Unique|Db-Field|Access|
|---------------|--------|-----------|----------|------|--------|------|
| Id            | int    | --------- | -------- | ---- | Yes    | R    |
| ISBNNumber    | String | 10        | No       | Yes  | Yes    | RW   |
| Author        | String | 128       | No       | Yes+ | Yes    | RW   |
| Title         | String | 256       | No       | Yes+ | Yes    | RW   |
| Description   | String | 1024      | Yes      | No   | Yes    | RW   |
| YearOfRelease | int    | --------- | -------- | No   | Yes    | RW   |
| Price         | double | --------- | -------- | No   | Yes    | RW   |

+...beide zusammen sind eindeutig

## Aufgaben  

### Geschäftsregeln  

Das System muss einige Geschäftsregeln umsetzen. Diese Regeln werden im **Backend** implementiert und müssen mit **UnitTests** überprüft werden.

> **HINWEIS:** Unter **Geschäftsregeln** (Business-Rules) versteht man **elementare technische Sachverhalte** im Zusammenhang mit Computerprogrammen. Mit *WENN* *DANN* Scenarien werden die einzelnen Regeln beschrieben.  

Für den ***SEBookStore*** sind folgende Regeln definiert:

| Rule | Subject | Type   | Operation | Description | Note |
|------|---------|--------|-----------|-------------|------|
|**A1**| `Book`  |        |           |             |      |
|      |         |**WENN**|           | ein `Book` erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss die `ISBNNumber` festgelegt sein und gültig sein (die Regeln finden Sie im Abschnitt **Prüfung der ISBN-Nummern**). | |
|**A2**| `Book`  |        |           |             |      |
|      |         |**WENN**|           | ein `Book` erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss der `Author` festgelegt sein und mindestens 3 Zeichen lang sein. | |
|**A3**| `Book`  |        |           |             |      |
|      |         |**WENN**|           | ein `Book` erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss der `Title` festgelegt sein und mindestens 5 Zeichen lang sein. | |
|**A4**| `Book`  |        |           |             |      |
|      |         |**WENN**|           | ein `Book` erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss die `YearOfRelease` festgelegt und im im Bereich von 1900 bis aktuelles Datum + 1 Jahr sein. | |
|**A5**| `Book`  |        |           |             |      |
|      |         |**WENN**|           | ein `Book` erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss der `Price` festgelegt und im Bereich von 1 EUR bis 10.000 EUR sein. | |

> **Hinweis:** Falls einer der Geschäftsregeln nicht erfüllt ist, muss eine **BusinessException** mit einer entsprechenden Fehlermeldung (in Englisch) geworfen werden.

### Prüfung der ISBN-Nummern

Die **Prüfziffer (10. Ziffer)** der ISBN-Nummer wird so berechnet:

1. Multipliziere jede der ersten 9 Ziffern mit ihrer Position (1 bis 9).  
2. Summiere alle Produkte.  
3. Teile die Summe ganzzahlig durch 11.  
4. Der **Rest** ist die Prüfziffer. Falls der Rest 10 ist, ist die Prüfziffer **„X“**.

#### Beispiele

1. **ISBN 3-499-13599-[?]**  
   `3·1 + 4·2 + 9·3 + 9·4 + 1·5 + 3·6 + 5·7 + 9·8 + 9·9 = 285`  
   `285 % 11 = 10` ⇒ Prüfziffer: **X**

2. **ISBN 3-446-19313-[?]**  
   `3·1 + 4·2 + 4·3 + 6·4 + 1·5 + 9·6 + 3·7 + 1·8 + 3·9 = 162`  
   `162 % 11 = 8` ⇒ Prüfziffer: **8**

3. **ISBN 0-7475-5100-[?]**  
   `0·1 + 7·2 + 4·3 + 7·4 + 5·5 + 5·6 + 1·7 + 0·8 + 0·9 = 116`  
   `116 % 11 = 6` ⇒ Prüfziffer: **6**

4. **ISBN 1-57231-422-[?]**  
   `1·1 + 5·2 + 7·3 + 2·4 + 3·5 + 1·6 + 4·7 + 2·8 + 2·9 = 123`  
   `123 % 11 = 2` ⇒ Prüfziffer: **2**

> **Hinweis:** Wenn die ISBN-Prüfziffer nicht korrekt ist, **muss eine Ausnahme geworfen werden.**

## Datenimport

Erstellen Sie ein Konsolenprogramm welches die Datenbank erzeugt und die beigelegte csv-Datei in die Datenbank importiert. Falls es beim Import zu Fehlern kommt (z.B. ISBN-Prüfziffer falsch), muss eine entsprechende Fehlermeldung ausgegeben werden.

---

## Umsetzung der Aufgabe mit dem SETemplate

Mit dem `SETemplate` können Sie die Aufgabenstellung in wenigen Schritten umsetzen.

**Umsetzungstabelle:**

| Schritt | Beschreibung                                   |
|---------|------------------------------------------------|
| 1       | `SETemplate` klonen                            |
| 2       | `SETemplate` mit der IDE öffnen                |
| 3       | Starten der Anwendung `TemplateTools.ConApp`   |
| 4       | `SETemplate` kopieren => `SEBookStore`         |
| 5       | `SEBookStore` mit der IDE öffnen               |
| 6       | Starten der Anwendung `TemplateTools.ConApp`   |
| 7       | `Preprocessor` einstellen                      |
| 8       | Entity-`Book` erstellen                        |
| 9       | Validierung für das Entity-`Book` erstellen    |
| 10      | Starten der `CodeGenerierung`                  |

### Schritt 1: Repository klonen

```bash
git clone https://github.com/leoggehrer/SETemplate.git
cd SETemplate
```

### Schritt 2: `SETemplate` mit der IDE öffnen

Öffnen Sie das `SETemplate` mit der IDE (Visual Studio 2022, Rider oder Visual Studio Code).

### Schritt 3: Starten der Anwendung `TemplateTools.ConApp`

Nach dem Start von `TemplateTools.ConApp` wird folgendes Menü angezeigt:

```bash
==============
Template Tools
==============

Solution path: ...\SETemplate

[ ----] -----------------------------------------------------------------
[    1] Path................Change solution path
[ ----] -----------------------------------------------------------------
[    2] Copier..............Copy this solution to a domain solution
[    3] Preprocessor........Setting defines for project options
[    4] CodeGenerator.......Generate code for this solution
[    5] Synchronization.....Matches a project with the template
[    6] Cleanup.............Deletes the temporary directories
[-----] -----------------------------------------------------------------
[  x|X] Exit................Exits the application

Choose [n|n,n|a...all|x|X]:
```

---

### Schritt 4: `SETemplate` kopieren => `SEBookStore`

Wählen Sie die **Menü-Option:** 2 - `Copier` aus. Anschließend wird das folgende Menü angezeigt:

```bash
===============
Template Copier
===============

'SETemplate' from:   ...\SETemplate
  -> copy ->
'TargetSolution' to: ...\TargetSolution

[  ---] -----------------------------------------------------------------
[    1] 1...................Change max sub path depth
[    2] Source path.........Change the source solution path
[    3] Target path.........Change the target solution path
[    4] Target name.........Change the target solution name
[  ---] -----------------------------------------------------------------
[    5] Start...............Start copy process
[-----] -----------------------------------------------------------------
[  x|X] Exit................Exits the application

Choose [n|n,n|a...all|x|X]:
```

Wählen Sie die **Menü-Option:** 3 - `Target path` aus und geben Sie den Zielpfad an, in dem das Projekt erstellt werden soll. Zum Beispiel: `C:\Users\...\source\repos`.

Wählen Sie die **Menü-Option:** 4 - `Target name` aus und geben Sie den Namen des Projektes an. Zum Beispiel: `SEBookStore`. Das Ergebnis sollte wie folgt aussehen:

```bash
===============
Template Copier
===============

'SETemplate' from: ...\SETemplate
  -> copy ->
'SEBookStore' to:  C:\Users\...\source\repos\SEBookStore

[  ---] -----------------------------------------------------------------
[    1] 1...................Change max sub path depth
[    2] Source path.........Change the source solution path
[    3] Target path.........Change the target solution path
[    4] Target name.........Change the target solution name
[  ---] -----------------------------------------------------------------
[    5] Start...............Start copy process
[-----] -----------------------------------------------------------------
[  x|X] Exit................Exits the application

Choose [n|n,n|a...all|x|X]:
```

Wählen Sie die **Menü-Option:** 5 - `Start` aus. Das System kopiert nun die Projektmappe und ersetzt die entsprechenden Projektnamen mit `SEBookStore`. Wenn sie ein Windows-System verwenden, wird automatisch der Datei-Explorer mit dem Pfad 'C:\Users\...\source\repos\SEBookStore' geöffnet.

---

### Schritt 5: `SEBookStore` mit der IDE öffnen

Im nächsten Schritt öffnen Sie nun die Projektmappe `SEBookStore` mit der IDE (Visual Studio, VSCode oder Rider). Wenn alles erfolgreich war, sollte das Ergebnis wie folgt aussehen:

```bash
-|- SEBookStore
  |- Diagrams
  |- SEBookStore.CodeGenApp
   |- ...
  |- SEBookStore.Common
   |- Contracts
   |- Enums
   |- Extensions
   |- Models
   |- Modules
   |- ...
  |- SEBookStore.ConApp
   |- ...
  |- SEBookStore.Logic
   |- Contracts
   |- Enums
   |- DataContext
   |- Entities
   |- Exceptions
   |- Models
   |- Modules
   |- ...
  |- SEBookStore.MVVMApp
   |- Models
   |- ViewModels
   |- Views
   |- ...
  |- SEBookStore.WebApi
   |- Contracts
   |- Controllers
   |- Models
   |- ...
  |- TemplateTools.ConApp
   |- ...
  |- TemplateTools.Logic
   |- ...
  |- README.md
  |- SETemplate.sln
```

---

### Schritt 6: Starten der Anwendung `TemplateTools.ConApp`

Nach dem Start von `TemplateTools.ConApp` wird folgendes Menü angezeigt:

```bash
==============
Template Tools
==============

Solution path: C:\Users\...\repos\SEBookStore

[ ----] -----------------------------------------------------------------
[    1] Path................Change solution path
[ ----] -----------------------------------------------------------------
[    2] Copier..............Copy this solution to a domain solution
[    3] Preprocessor........Setting defines for project options
[    4] CodeGenerator.......Generate code for this solution
[    5] Synchronization.....Matches a project with the template
[    6] Cleanup.............Deletes the temporary directories
[-----] -----------------------------------------------------------------
[  x|X] Exit................Exits the application

Choose [n|n,n|a...all|x|X]:
```

---

### Schritt 7: `Preprocessor` einstellen

Wählen Sie die **Menü-Option:** 3 - `Preprocessor` aus. Anschließend wird das folgende Menü angezeigt:

```bash
========================
Template Setting Defines
========================

Solution path: C:\Users\...\source\repos\SEBookStore

[  ---] -----------------------------------------------------------------
[    1] Path................Change preprocessor solution path
[  ---] -----------------------------------------------------------------
[    2] Set definition ACCOUNT_OFF               ==> ACCOUNT_ON
[  ---] -----------------------------------------------------------------
[    3] Set definition IDINT_ON                  ==> IDINT_OFF
[    4] Set definition IDLONG_OFF                ==> IDLONG_ON
[    5] Set definition IDGUID_OFF                ==> IDGUID_ON
[  ---] -----------------------------------------------------------------
[    6] Set definition ROWVERSION_OFF            ==> ROWVERSION_ON
[    7] Set definition EXTERNALGUID_OFF          ==> EXTERNALGUID_ON
[  ---] -----------------------------------------------------------------
[    8] Set definition POSTGRES_OFF              ==> POSTGRES_ON
[    9] Set definition SQLSERVER_OFF             ==> SQLSERVER_ON
[   10] Set definition SQLITE_ON                 ==> SQLITE_OFF
[  ---] -----------------------------------------------------------------
[   11] Set definition DOCKER_OFF                ==> DOCKER_ON
[   12] Set definition DEVELOP_ON                ==> DEVELOP_OFF
[   13] Set definition DBOPERATION_ON            ==> DBOPERATION_OFF
[   14] Set definition GENERATEDCODE_ON          ==> GENERATEDCODE_OFF
[  ---] -----------------------------------------------------------------
[   15] Start...............Start assignment process
[-----] -----------------------------------------------------------------
[  x|X] Exit................Exits the application

Choose [n|n,n|a...all|x|X]:
```

Mit diesem Menü können die Projektparameter eingestellt werden.

### Schritt 8: Entity-`Book` erstellen

```bash
namespace SEBookStore.Logic.Entities
{
    /// <summary>
    /// Represents a book entity.
    /// </summary>
    [Table("Books")]
    [Index(nameof(ISBNNumber), IsUnique = true)]
    [Index(nameof(Author), nameof(Title), IsUnique = true)]
    public partial class Book : EntityObject
    {
        private string iSBNNumber = string.Empty;

        /// <summary>
        /// Gets or sets the ISBN number of the book.
        /// </summary>
        [Required, MaxLength(10)]
        public string ISBNNumber 
        { 
            get => iSBNNumber; 
            set => iSBNNumber = value.Replace("-", string.Empty); 
        }
        /// <summary>
        /// Gets or sets the author of the book.
        /// </summary>
        [Required, MaxLength(128)]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the title of the book.
        /// </summary>
        [Required, MaxLength(256)]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the book.
        /// </summary>
        [Required, MaxLength(2048)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the year of release of the book.
        /// </summary>
        public int YearOfRelease { get; set; }

        /// <summary>
        /// Gets or sets the price of the book.
        /// </summary>
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Author} - {Title}";
        }
    }
}
```

**Erstellungsrichtlinien:**

- Erstellen Sie sie Entität-`Book` als `public partial class Book`-Klasse.
- Leiten Sie die Entität-`Book` von `EntityObject` oder `VersionEntityObject` ab.
- Geben Sie den Tabellenname als Klassen-Attribut `[Table("...")]` über der Klasse an.
- Erstellen Sie alle Eigenschaften und deren Einschränkungen `[MaxLength(10)]`.
- Geben Sie alle Index-Einschränkungen als Klassen-Attribute `[Index(..., IsUnique = true)]` an.

**Prüfung der Entität-`Book`:**

| Klasse | Prüfung | Ergebnis | Beschreibung |
|--------|---------|----------|--------------|
| Book   | Ist die Klasse als `public partial class Book` deklariert? | Ja | Wenn nein, tritt ein Fehler beim Verbinden von der Schnittstelle `IBook` mit der `Book`-Klasse auf. |
| Book  | Ist die Klasse von `EntityObject` oder `VersionEntityObject` abgeleitet? | Ja |  Wenn nein, wird `Book` nicht als Entität erkannt. |
| Book  | Ist die Klasse mit dem Attribut [Table("...")] versehen? | Ja | Wenn nein, wird ein Standardnamen vergeben. |
| Book  | Sind die Eigenschaften mit den Attributen [MaxLength(...)] versehen? | Ja | Wenn nein, werden die Standardwerte verwendet. |
| Book  | Sind die Eigenschaften mit den Attributen [Required] versehen? | Ja | Wenn nein, werden die Standardwerte verwendet (Abhängig von der dotnet- Version). |
| Book  | Sind die Eigenschaften mit den Attributen [Index(..., IsUnique = true)] versehen? |Ja  | Wenn nein, wird kein Index erstellt. |

### Schritt 9: Validierung für das Entity-`Book` erstellen

In diesem Schritt werden die Geschäftsregeln für die Entität-`Book` implementiert.

>**Tipp:** Erstellen Sie eine weiter partial-Klasse und implementieren Sie die Schnittstelle IValidatableEntity. Diese Schnittstelle ist in der Datei SETemplate.Common/Contracts/IValidatableEntity.cs definiert. Verwenden Sie als Dateinamen BookValidation.cs.

```bash
using SEBookStore.Logic.Contracts;
using SEBookStore.Logic.Exceptions;

namespace SEBookStore.Logic.Entities
{
    /// <summary>
    /// Represents a book entity with validation logic.
    /// </summary>
    partial class Book : IValidatableEntity
    {
        /// <summary>
        /// Validates the properties of the book entity.o
        /// </summary>
        /// <param name="context">The context in which the validation is performed.</param>
        /// <exception cref="BusinessException">
        /// Thrown when any of the following validation rules are violated:
        /// <list type="bullet">
        /// <item><description>The ISBN number is invalid.</description></item>
        /// <item><description>The author's name is less than 3 characters long.</description></item>
        /// <item><description>The title is less than 5 characters long.</description></item>
        /// <item><description>The year of release is not between 1900 and the next calendar year.</description></item>
        /// <item><description>The price is not between EUR 1 and EUR 10,000.</description></item>
        /// </list>
        /// </exception>
        public void Validate(IContext context, EntityState entityState)
        {
            // A1
            if (CheckISBNNumber(ISBNNumber) == false)
            {
                throw new BusinessException("Invalid ISBN number");
            }
            // A2
            if (Author.Length < 3)
            {
                throw new BusinessException("The character length of the author must be at least 3 characters long.");
            }
            // A3
            if (Title.Length < 5)
            {
                throw new BusinessException("The character length of the title must be at least 5 characters long.");
            }
            // A4
            if (YearOfRelease < 1900 || YearOfRelease > DateTime.Now.Year + 1)
            {
                throw new BusinessException($"The publication must be between 1900 and {DateTime.Now.Year + 1}.");
            }
            // A5
            if (Price < 1.0 || Price > 10_000.0)
            {
                throw new BusinessException("The price must be between EUR 1 and EUR 10,000.");
            }
        }

        /// <summary>
        /// Validates the given ISBN number based on the ISBN-10 standard.
        /// </summary>
        /// <param name="number">The ISBN number to validate.</param>
        /// <returns>
        /// Returns <c>true</c> if the ISBN number is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// The method checks if the input string is 10 characters long, where the first 9 characters are digits,
        /// and the last character can be a digit or 'X' (case-insensitive). It also verifies the checksum
        /// using the modulo 11 algorithm.
        /// </remarks>
        public static bool CheckISBNNumber(string number)
        {
            var result = number != null && number.Where((c, i) => i == 9 ? (c == 'X' || c == 'x' || char.IsDigit(c)) : char.IsDigit(c)).Count() == 10;

            var sum = 0;
            var rest = 0;

            for (int i = 0; result && i < number?.Length - 1; i++)
            {
                sum += (number == null ? 0 : number[i] - '0') * (i + 1);
            }

            rest = sum % 11;

            return result && number != null && ((rest == 10 && char.ToUpper(number[^1]) == 'X') || (rest == number[^1] - '0'));
        }
    }
}
```

**Erstellungsrichtlinien:**

- Erstellen Sie eine weiter Klasse `partial class Book : IValidatableEntity` und implementieren Sie die Schnittstelle `IValidatableEntity`.
- Vergeben Sie für die Klasse den Dateinamen 'BookValidation.cs'.
- Implementieren Sie die Methode Validate(IContext context).

**Prüfung der Entität-`Book`-Validation:**

| Klasse | Prüfung | Ergebnis | Beschreibung |
|--------|---------|----------|--------------|
| Book  | Ist die Klasse als `partial class Book : IValidatableEntity` deklariert? | Ja | Wenn nein, wird die Validierung nicht mit der Entität-`Book` verbunden. |
| Book  | Ist die Geschäftsregel A1 implementiert? | Ja | Wenn nein, können ungültige Werte in der ISBNNummer eingetragen werden. |
| Book  | Ist die Geschäftsregel A2 implementiert? | Ja | Wenn nein, können Namen mit weniger als < 3 Zeichen eingetragen werden. |
| Book  | Ist die Geschäftsregel A3 implementiert? | Ja | Wenn nein, können Buchtiteln mit weniger als < 5 Zeichen eingetragen werden. |
| Book  | Ist die Geschäftsregel A4 implementiert? | Ja | Wenn nein, können ungültige Erscheinungsjahre eingetragen werden. |
| Book  | Ist die Geschäftsregel A5 implementiert? | Ja | Wenn nein, können ungültige Preise eingetragen werden. |

---

### Schritt 10: Starten der `CodeGenerierung`

Bevor mit der Code-Generierung begonnen werden kann, müssen die folgenden Schritte durchgeführt werden:

- Die Entität-`Book` ist erstellt und alle Prüfschritte haben das Ergebniss 'Ja'.
- Die Entität-`Book` ist validiert und alle Prüfschritte haben das Ergebnis 'Ja'.
- Die Projektmappe `SEBookStore` kann fehlerfrei erstellt werden.

Starten das Programm `TemplateTools.ConApp` und wählen Sie die **Menü-Option:** 4 - `CodeGenerator` aus. Anschließend wird das folgende Menü angezeigt:

```bash
=======================
Template Code Generator
=======================

Solution path:                    C:\Users\...\repos\SEBookStore
---------------------------------
Write generated source into:      Single files
Write info header into source:    True
Delete empty folders in the path: True
Exclude generated files from GIT: True

[-----] -----------------------------------------------------------------
[    1] Generation file.....Change generation file option
[    2] Add info header.....Change add info header option
[    3] Delete folders......Change delete empty folders option
[    4] Exclude files.......Change the exclusion of generated files from GIT
[    5] Source path.........Change the source solution path
[-----] -----------------------------------------------------------------
[    6] Compile.............Compile logic project
[    7] Delete files........Delete generated files
[    8] Delete folders......Delete empty folders in the path
[    9] Start...............Start code generation
[-----] -----------------------------------------------------------------
[  x|X] Exit................Exits the application

Choose [n|n,n|a...all|x|X]:
```

Bevor Sie mit der `CodeGenerierung` beginnen, können einige Einstellungen vorgenommen werden. Sie finden die Beschreibung in der Dokumentation für [**SE-Template**](https://github.com/leoggehrer/SETemplate).

Nachdem die Einstellung vorgenommen wurde, wählen Sie die **Menü-option:** 9 - `Start` aus. Die `CodeGenerierung` startet und hat folgende Code-Teile generiert:

| Module  | Ordner        | Komponente         | Dateiname                     | Beschreibung                                   |
|---------|---------------|--------------------|-------------------------------|------------------------------------------------|
| Common  | Contracts     | `IBook`            | Book.cs                       | Die Schnittstelle für das Entity-`Book` mit `CopyProperties(...)`. |
| Logic   | Entities      | `Book`             | BookGeneration.cs             | Die Schnittstelle wird mit dem Entity-`Book` verbunden. |
| Logic   | DataContext   | `BookSet`          | BookSet.cs                    | Die `Set`-Management Klasse für den Zugriff. |
| Logic   | Contracts     | `IBookSet`         | IBookSet.cs                   | Die Schnittstelle für die `BookSet`-Klasse. |
| Logic   | DataContext   | `ProjectDbContext` | ProjectDbContextGeneration.cs | Enthält die Eigenschaften `DbBookSet<Book>`, `BookSet` und die entsprechenden Methoden. |
| Logic   | Contracts     | `IContext`         | IContextGeneration.cs         | Die Schnittstelle für den öffentlichen Zugriff das `BookSet`. |
| WebApi  | Models        | `Book`             | Book.cs                       | Das Model-`Book` für die `WebApi`-Übertragung. |
| WebApi  | Models        | `BookEdit`         | BookEdit.cs                   | Das Model-`BookEdit`für die `WebApi`-Übertragung. |
| WebApi  | Models        | `Book`             | BookInheritance.cs            | Eine `partial`-Klasse in welche die Ableitung des Models definiert ist. |
| WebApi  | Controllers   | `BooksController`  | BooksController.cs            | Die standard Implementierung der `CRUD`-Operationen für das Entity `Book` |
| WebApi  | Controllers   | `ContextAccessor`  | ContextAccessorGeneration.cs  | Die Methode für den Zugriff auf die `BookSet`-Eigenschaft in der Logik. |
| MVVMApp | Models        | `Book`             | Book.cs                       | Das Model-`Book` für die `WebApi`-Übertragung. |
| MVVMApp | Models        | `Book`             | BookInheritance.cs            | Eine `partial`-Klasse in welche die Ableitung des Models definiert ist. |
| MVVMApp | ViewModels    | `BooksViewModel`   | BooksViewModel.cs             | Ein `ViewModel` für die `Book`-Tabellen-Ansicht. |
| MVVMApp | ViewModels    | `BookViewModel`    | BookViewModel.cs              | Ein `ViewModel` für die `Book`-Einzel-Ansicht. |

> **HINWEIS:** Die **Dateinamen** gelten nur für den Mode *Write generated source into: Single files*.

## Erweiterungen nach der CodeGenerierung

**Erweiterungsrichtlinien:**

- Wenn eine Klasse Members erweitert wird, dann erfolgt dies in einer `partial class Name`.
- Zusätzlich muss die Code-Generierung geprüft werden und mit der Konstanten `GENERATEDCODE_ON` eingeschaltet werden.
- Der folgende Programmausschnitt zeigt eine mögliche Erweiterung:

```csharp
#if GENERATEDCODE_ON
namespace SEBookStore.MVVMApp.Models
{
    partial class Book
    {
        public override string ToString()
        {
            return $"{Author} {Title}";
        }
    }
}
#endif
```

### Erweiterung: Datenimport

Für den Datenimport erweitern wir im Modul `SEBookStore.ConApp` die Klasse `Program`. Zu diesem Zweck wird eine partiele Klasse mit dem Dateinamen 'ProgramImport.cs' erstellt. In dieser Klasse erfolgt das Eomnlesen der csv-Daten und die Auswertung des Importes:

```csharp
#if GENERATEDCODE_ON
using SEBookStore.Logic.Exceptions;

namespace SEBookStore.ConApp
{
    /// <summary>
    /// This partial class of Program contains the ImportData method, which imports book data from a CSV file into the database.
    /// </summary>
    partial class Program
    {
        /// <summary>
        /// Imports book data from a CSV file located in the "Data" directory.
        /// Each line in the CSV file represents a book with its details separated by semicolons.
        /// The method reads the file, parses the data, and adds each book to the database.
        /// </summary>
        /// <remarks>
        /// The CSV file is expected to have the following columns in order:
        /// ISBNNumber, Author, Description, Price, Title, YearOfRelease.
        /// </remarks>
        /// <exception cref="BusinessException">Thrown when a business rule is violated while adding a book to the database.</exception>
        /// <exception cref="Exception">Thrown for any other errors during the import process.</exception>
        static partial void ImportData()
        {
            int index = 0;
            var filePath = Path.Combine("Data", "book_dataset.csv");
            var books = File.ReadAllLines(filePath).Skip(1)
                .Select(line => line.Split(';'))
                .Select(parts => new Logic.Entities.Book
                {
                    ISBNNumber = parts[0],
                    Author = parts[1],
                    Description = parts[2],
                    Price = double.Parse(parts[3]),
                    Title = parts[4],
                    YearOfRelease = int.Parse(parts[5])
                });

            foreach (var book in books)
            {
                using var context = CreateContext();

                try
                {
                    index++;
                    context.BookSet.Add(book);
                    context.SaveChanges();
                }
                catch (BusinessException ex)
                {
                    Console.WriteLine($"Error on line {index} {book}: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error on line {index}: {ex.InnerException}");
                }
            }
        }
    }
}
#endif
```

Das Programm `SEBookStore.ConApp` kann nun gestartet werden. Anschließend führen Sie die Auswahl: **InitDatabase...1** durch - der Import startet. In der Konsole werden die Fehler vom Import ausgegeben:

```bash
SEBookStore
==========================================
InitDatabase             ....1

Exit...............x

Your choice: 1
Error on line 3 Julie Watts - Ashes and Hope: Invalid ISBN number
Error on line 5 Sa - Paris in Spring: The character length of the author must be at least 3 characters long.
Error on line 7 Irene Arnold - Star: The character length of the title must be at least 5 characters long.
Error on line 18 Jorge Dixon - The Dreammaker: The publication must be between 1900 and 2026.
Error on line 21 Kara Rhodes - Tastes of Tuscany: The publication must be between 1900 and 2026.
Error on line 25 Moses Knight - The Painter AI: The price must be between EUR 1 and EUR 10,000.
Error on line 27 Stacey Parks - Melody of Me: The price must be between EUR 1 and EUR 10,000.
Error on line 31: Microsoft.Data.Sqlite.SqliteException (0x80004005): SQLite Error 19: 'UNIQUE constraint failed: Books.ISBNNumber'.
   at Microsoft.Data.Sqlite.SqliteException.ThrowExceptionForRC(Int32 rc, sqlite3 db)
   at Microsoft.Data.Sqlite.SqliteDataReader.NextResult()
   at Microsoft.Data.Sqlite.SqliteCommand.ExecuteReader(CommandBehavior behavior)
   at Microsoft.Data.Sqlite.SqliteCommand.ExecuteDbDataReader(CommandBehavior behavior)
   at Microsoft.EntityFrameworkCore.Storage.RelationalCommand.ExecuteReader(RelationalCommandParameterObject parameterObject)
   at Microsoft.EntityFrameworkCore.Update.ReaderModificationCommandBatch.Execute(IRelationalConnection connection)

Continue with Enter...
```

### Erweiterung MVVMApp

In der MVVMApp sind im Ordner `ViewModel` die beiden Klassen `BooksViewModel` und `BookViewModel` generiert worden. Das `BooksViewModel` ist für die Listen-Ansicht mit den Operationen `Add`, `Edit` und `Delete` generiert worden. Das `BookViewModel` dient für die Einzelbearbeitung `Add` und `Edit`. Allerdings müssen für die Verwendung noch ein paar Anpassungen durchgeführt werden.

#### Erweiterung MVVMApp-Listen-Ansicht

> **HINWEIS:** Für die Entwicklung von Ansichten wird die Verwendung von `UserControls` empfohlen. Diese unterstützen die Wiederverwendung auf der Ebene von Ansichten.

Erstellung der Listen-Ansicht mit einem `DataGrid`-Control  im Ordner `Views`:

> Fügen Sie mit der IDE ein `Avalonia UserControl` mit dem Namen **BooksUserControl** hinzu.

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:vm="using:SEBookStore.MVVMApp.ViewModels"
             mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="450"
             x:DataType="vm:BooksViewModel"
             x:Class="SEBookStore.MVVMApp.Views.BooksUserControl">

    <UserControl.DataContext>
        <vm:BooksViewModel></vm:BooksViewModel>
    </UserControl.DataContext>

    <Grid Margin="10">
        <Grid.ColumnDefinitions>
            <ColumnDefinition></ColumnDefinition>
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="*"></RowDefinition>
        </Grid.RowDefinitions>

        <Grid Grid.Column="0" Grid.Row="0" Margin="0 0 0 10">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="35"></ColumnDefinition>
                <ColumnDefinition Width="*"></ColumnDefinition>
                <ColumnDefinition Width="35"></ColumnDefinition>
            </Grid.ColumnDefinitions>

            <!-- Filter-Textbox nimmt die gesamte verfügbare Breite ein -->
            <Button Grid.Column="0"    Grid.Row="0"
                Background="Transparent"
                Command="{Binding AddItemCommand}">
                <StackPanel Orientation="Horizontal" Width="20" Height="20">
                    <Image Source="/Assets/add.png"></Image>
                </StackPanel>
            </Button>

            <TextBox Grid.Column="1" Grid.Row="0"
                Watermark="Geben Sie hier eine Filter ein"
                Text="{Binding Filter}"
                HorizontalAlignment="Stretch" />

            <!-- Button mit fester Breite -->
            <Button    Grid.Column="2"    Grid.Row="0"
                Background="Transparent"
                Command="{Binding LoadModelsCommand}">
                <StackPanel Orientation="Horizontal" Width="20" Height="20">
                    <Image Stretch="" Source="/Assets/reload.png"></Image>
                </StackPanel>
            </Button>
        </Grid>

        <DataGrid Grid.Column="0" Grid.Row="1"
            x:Name="dataGrid"
            BorderThickness="1"
            BorderBrush="Gray"
            IsReadOnly="True"
            ItemsSource="{Binding Models}"
            SelectedItem="{Binding SelectedItem, Mode=TwoWay}"
            AutoGenerateColumns="False">
            <DataGrid.Columns>
                <DataGridTextColumn Header="Author" Binding="{Binding Author}" />
                <DataGridTextColumn Header="Titel" Binding="{Binding Title}" />
                <DataGridTemplateColumn Width="Auto" Header="Actions">
                    <DataGridTemplateColumn.CellTemplate>
                        <DataTemplate>
                            <StackPanel Orientation="Horizontal">
                                <Button
                                    Background="Transparent"
                                    Command="{Binding #dataGrid.((vm:BooksViewModel)DataContext).EditItemCommand}"
                                    CommandParameter="{Binding}">
                                    <StackPanel Orientation="Horizontal" Width="20" Height="20">
                                        <Image Stretch="" Source="/Assets/edit.png"></Image>
                                    </StackPanel>
                                </Button>
                                <Button
                                    Background="Transparent"
                                    Command="{Binding #dataGrid.((vm:BooksViewModel)DataContext).DeleteItemCommand}"
                                    CommandParameter="{Binding}">
                                    <StackPanel Orientation="Horizontal" Width="20" Height="20">
                                        <Image Stretch="" Source="/Assets/delete.png"></Image>
                                    </StackPanel>
                                </Button>
                            </StackPanel>
                        </DataTemplate>
                    </DataGridTemplateColumn.CellTemplate>
                </DataGridTemplateColumn>
            </DataGrid.Columns>
        </DataGrid>
    </Grid>

</UserControl>
```

> **HINWEIS:** Sie können die Vorlage `ItemsUserControl` aus dem Ordner `Template` kopieren und die `DataGridColumns` anpassen.

Im nächsten Schritt fügen wir das `BooksUserControl` in das `MainWindow` ein. Das `MainWindow` wird wie im folgenden Abschnitt entsprechend angepasst:

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:vm="using:SEBookStore.MVVMApp.ViewModels"
        xmlns:views="using:SEBookStore.MVVMApp.Views"
        mc:Ignorable="d" d:DesignWidth="1000" d:DesignHeight="600"
        x:Class="SEBookStore.MVVMApp.Views.MainWindow"
        x:DataType="vm:MainWindowViewModel"
        Width="1000"
        Height="600"
        Icon="/Assets/avalonia-logo.ico"
        Title="BookStore">

    <Design.DataContext>
        <vm:MainWindowViewModel/>
    </Design.DataContext>

    <TabControl Margin="5">
        <TabItem Header="Bücher">
            <views:BooksUserControl />
        </TabItem>
    </TabControl>    

</Window>
```

#### Erweiterung MVVMApp-Einzel-Ansicht

Erstellung der Einzel-Ansicht mit einem `Grid`-Container  im Ordner `Views`:

> Fügen Sie mit der IDE ein `Avalonia UserControl` mit dem Namen **BookUserControl** hinzu.

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:vm="using:SEBookStore.MVVMApp.ViewModels"
             mc:Ignorable="d" d:DesignWidth="460" d:DesignHeight="320"
             x:Class="SEBookStore.MVVMApp.Views.BookUserControl"
              x:DataType="vm:BookViewModel">

    <UserControl.DataContext>
        <vm:BookViewModel />
    </UserControl.DataContext>

    <Grid Margin="20 10 20 10">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"></ColumnDefinition>
            <ColumnDefinition Width="*"></ColumnDefinition>
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="Auto"></RowDefinition>
            <RowDefinition Height="Auto"></RowDefinition>
        </Grid.RowDefinitions>

        <Label Grid.Row="0" Grid.Column="0" Content="ISBN:" VerticalAlignment="Center" HorizontalAlignment="Right" Margin="10 10 10 0"/>
        <TextBox Grid.Row="0" Grid.Column="1" Text="{Binding ISBNNumber}" Margin="0 10 10 0"/>

        <Label Grid.Row="1" Grid.Column="0" Content="Autor:" VerticalAlignment="Center" HorizontalAlignment="Right" Margin="10 10 10 0"/>
        <TextBox Grid.Row="1" Grid.Column="1" Text="{Binding Author}" Margin="0 10 10 0"/>

        <Label Grid.Row="2" Grid.Column="0" Content="Titel:" VerticalAlignment="Center" HorizontalAlignment="Right" Margin="10 10 10 0"/>
        <TextBox Grid.Row="2" Grid.Column="1" Text="{Binding Title}" Margin="0 10 10 0"/>

        <Label Grid.Row="3" Grid.Column="0" Content="Beschreibung:" VerticalAlignment="Center" HorizontalAlignment="Right" Margin="10 10 10 0"/>
        <TextBox Grid.Row="3" Grid.Column="1" Text="{Binding Description}" Margin="0 10 10 0"/>

        <Label Grid.Row="4" Grid.Column="0" Content="Erscheinung:" VerticalAlignment="Center" HorizontalAlignment="Right" Margin="10 10 10 0"/>
        <TextBox Grid.Row="4" Grid.Column="1" Text="{Binding YearOfRelease}" Margin="0 10 10 0"/>

        <Label Grid.Row="5" Grid.Column="0" Content="Preis:" VerticalAlignment="Center" HorizontalAlignment="Right" Margin="10 10 10 0"/>
        <TextBox Grid.Row="5" Grid.Column="1" Text="{Binding Price}" Margin="0 10 10 0"/>

        <StackPanel Grid.Row="6" Grid.Column="1" Orientation="Horizontal" HorizontalAlignment="Center" Margin="10">
            <Button Content="Abbrechen" Command="{Binding CancelCommand}" Width="120" HorizontalContentAlignment="Center" Margin="0 10 10 0"/>
            <Button Content="Speichern" Command="{Binding SaveCommand}" Width="120" HorizontalContentAlignment="Center" Margin="0 10 10 0"/>
        </StackPanel>
    </Grid>

</UserControl>
```

> **HINWEIS:** Sie können die Vorlage `ItemUserControl` aus dem Ordner `Template` kopieren und die `GridColumns` und `GridRows` entsprechend anpassen.

Das `ItemUserControl` benötigt nun ein `Avalonia Window` damit dieses Element dem Benutzer angezeigt werden kann. 

> Fügen Sie mit der IDE ein `Avalonia Window` mit dem Namen **BookWindow** hinzu.

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:vm="using:SEBookStore.MVVMApp.ViewModels"
        xmlns:views="using:SEBookStore.MVVMApp.Views"
        mc:Ignorable="d"
        d:DesignWidth="800"
        d:DesignHeight="350"
        Width="800"
        Height="350"
        x:Class="SEBookStore.MVVMApp.Views.BookWindow"
        x:DataType="vm:BookViewModel"
        WindowStartupLocation="CenterOwner"
        Title="Buch">

    <Window.DataContext>
        <vm:BookViewModel/>
    </Window.DataContext>

    <views:BookUserControl DataContext="{Binding}">
    </views:BookUserControl>

</Window>
```

Im letzten Schritt muss noch das `BooksViewModel` im Ordner `ViewModels` angepasst werden. Dazu muss eine partielle Klasse erstellt werden und die abstrakten Methoden definiert werden. Der Dateiname ist *BooksViewModelEx.cs*.

```csharp
#if GENERATEDCODE_ON
using Avalonia.Controls;
using SEBookStore.MVVMApp.Models;

namespace SEBookStore.MVVMApp.ViewModels
{
    partial class BooksViewModel
    {
        protected override GenericItemViewModel<Book> CreateViewModel()
        {
            return new BookViewModel();
        }

        protected override Window CreateWindow()
        {
            return new Views.BookWindow();
        }
    }
}
#endif
```

### Testen der MVVMApp

Damit die Anwendung gestartet werden kann, muss ein MultiStart eingerichtet werden. Für die Ausführung der `MVVMApp` muss zusätzlich die `WebApi` gestartet werden. Die basis-URL ist in der `appsettings.Development.json` konfiguriert. Als `Default`-Wert ist der Wert *https://localhost:7074/api/* eingestellt.

**Viel Spaß**
