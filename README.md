﻿﻿
# POSE 6ABIF/6ACIF

## BookStore

### Lehrziele

- Umsetzung der SE-Architektur aus dem Unterricht
- Erstellen von Schnittstellen
- Erstellen des Datenbankzugriffs
- Prüfen von Geschäftsregeln

### Einleitung

Das Projekt ***BookStore*** ist eine datenzentrierte Anwendung zur Verwaltung von Büchern. 

Zu entwickeln ist das Backend-System mit der Datenbank-Anbindung und einer Schnittstelle für das Importieren von csv-Daten.

### Datenmodell und Datenbank

Das Datenmodell für ***BookStore*** hat folgenden Aufbau:

```txt

+-------+--------+ 
|                | 
|     Book       + 
|                | 
+-------+--------+ 

```

#### Definition von ***Book***

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

### Aufgaben  

#### Backend-System  

Entwickeln Sie ein **Backend-System** nach der im Unterricht vorgestellten SE-Architektur.

##### Business-Logik  

Das System muss einige Geschäftsregeln umsetzen. Diese Regeln werden im Backend implementiert und müssen mit UnitTests überprüft werden.

> **HINWEIS:** Unter **Geschäftsregeln** (Business-Rules) versteht man **elementare technische Sachverhalte** im Zusammenhang mit Computerprogrammen. Mit *WENN* *DANN* Scenarien werden die einzelnen Regeln beschrieben.  

Für den ***BookStore*** sind folgende Regeln definiert:

| Rule | Subject | Type   | Operation | Description | Note |
|------|---------|--------|-----------|-------------|------|
|**A1**| Book    |        |           |             |      |
|      |         |**WENN**|           | eine *Book* erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss die `ISBNNumber` festgelegt sein und gültig sein (die Regen finden Sie im Abschnitt **Prüfung der ISBN-Nummern**. | |
|**A2**| Book    |        |           |             |      |
|      |         |**WENN**|           | eine *Book* erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss der `Author` festgelegt sein und mindestens 3 Zeichen lang sein. | |
|**A3**| Book    |        |           |             |      |
|      |         |**WENN**|           | eine *Book* erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss der `Title` festgelegt sein und mindestens 5 Zeichen lang sein. | |
|**A4**| Book    |        |           |             |      |
|      |         |**WENN**|           | eine *Book* erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss die `YearOfRelease` festgelegt und im im Bereich von 1900 bis aktuelles Datum + 1 Jahr sein. | |
|**A5**| Book    |        |           |             |      |
|      |         |**WENN**|           | eine *Book* erstellt oder bearbeitet wird, |  |
|      |         |**DANN**|           | muss der `Price` festgelegt und im Bereich von 1 EUR bis 10.000 EUR sein. | |

**Hinweis:** Falls einer der Geschäftsregeln nicht erfüllt ist, muss eine **BusinessException** mit einer entsprechenden Fehlermeldung (in Englisch) geworfen werden.

##### Prüfung der ISBN-Nummern

Die **Prüfziffer (10. Ziffer)** der ISBN-Nummer wird so berechnet:

1. Multipliziere jede der ersten 9 Ziffern mit ihrer Position (1 bis 9).  
2. Summiere alle Produkte.  
3. Teile die Summe ganzzahlig durch 11.  
4. Der **Rest** ist die Prüfziffer. Falls der Rest 10 ist, ist die Prüfziffer **„X“**.

##### Beispiele:

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

**Hinweis:** Wenn die ISBN-Prüfziffer nicht korrekt ist, **muss eine Ausnahme geworfen werden.**

### Datenimport

Erstellen Sie ein Konsolenprogramm welches die Datenbank erzeugt und die beigelegte csv-Datei in die Datenbank importiert. Falls es beim Import zu Fehlern kommt (z.B. ISBN-Prüfziffer falsch), muss eine entsprechende Fehlermeldung ausgegeben werden.

**Viel Spaß beim Umsetzen der Aufgabe!**
