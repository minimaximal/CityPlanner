# City Planner

## Disclaimer

Dieses Projekt entstand als Projektentwurf der Vorlesung "Grundlagen des Software-Engineering" des Kurses TINF21AI1 an der DHBW Mannheim.

**Sämtliche genannten Personen und Events sind frei erfunden!**

---

This project was part of the lecture "Software-Engineering Basics" at the DHBW (Baden-Wuerttemberg Coorperative State University) Mannheim. The project will be graded and was submitted by a team of 6 people from the course TINF21AI1.

**All named people or events are completely fictional!**

## Projektziel

Durch die Softwarelösung "City Planner" ermöglicht die Generierung eines Plans für eine organisch aufgebaute Wüstenstadt

## Funktionen in der Software

- Die Software verfügt über ein Hauptmenü. Von diesem gelangt man zum Erstellungsfenster einer neuen Simulation
- Das Erstellungsfenster für eine neue Simulation bietet folgende Möglichkeiten:
  - Einstellen der Parameter: Kartenbreite, Kartenläng, Zielbevölkerung und Importquote
  - Einstellen der erweiterten Parameter: Anzahl gleichzeitiger Simulationen und Mutationschance
  - Zurücksetzen der Parameter
  - Zurückkehren ins Hauptmenü
  - Öffnen des Karteneditors
  - Starten der Simulation
- Karteneditor
  - Der Karteneditor ermöglicht das Setzen von:
    - Startpunkten
    - Autobahnen
    - Sperrflächen
  - Durch setzen einer leern Fläche können gesetzte Felder wieder entfernt werden
  - Vom Karteneditor aus kann die Simulation dann auch direkt gestartet werden
  - Bei Bedarf kann man vom Karteneditor auch zurück zu den Generierungs Einstellungen gelangen
- Simulation
  - Die Simulation stellt während der Ausführung den besten Agent der Generation von Agents visuell auf der Karte dar
  - Zusätzlich werden zur visuellen Darstellung auf der Karte werden auch die Statistiken des Agents angegeben
  - Ebenfalls werden Statistiken zur Simulation selbst angegeben: aktuelle Generation und die zuletzt geladene Karte
  - Des Weiteren gibt es eine übersicht über Gesetzte Paraneter und eine Legende, in der die einzelnen Feldtypen der Karte erklärt sind
  - Die Simulation kann zu jeder Zeit pausiert werden
- Feldtypen
  - In der Simulaiton sind folgende Feldtypen vorhanden:
    - Wohngebiet (Level 1 bis 3)
    - Gewerbegebiert (Level 1 bis 3)
    - Industriegebiet (Level 1 bis 3)
    - Sehenswürdigkeit
    - U-Bahn
    - Straße
    - Autobahn
    - Sperrfläche
    - leere Fläche
 
