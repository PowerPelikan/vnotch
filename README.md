# Kerbschlagbiegeversuch-Simulation

Der Kerbschlagbiegeversuch ist eine weit verbreitete Methode zur Bewertung der Zähigkeit oder Schlagarbeit von Materialien. Er misst die Fähigkeit eines Materials, bei plötzlicher Schlagbelastung Energie aufzunehmen und Brüchen zu widerstehen. Diese Anwendung simuliert das Experiment und modelliert die Schritte, die erforderlich sind, um eine Schlagversuchsmaschine zu betreiben - sowie eine Methode, um die gemessenen Ergebnisse festzuhalten.

Die Simulation ist jedoch ausschließlich als Lernwerkzeug konzipiert!

Wesentliche Features:
* Simulation des Kerbschlagbiegeversuchs mit akuraten Ergebnissen
* Ausführliches Tutorial, sowohl für die Steuerung des Simulators, als auch die Durchführung des Experiments
* Deutsche und Englische Lokalisierung

### Projektstruktur

Dieses Repository hält das Unity-Projekt (zurzeit ver. 2022.2.17f), inklusive der zugehörigen Quellcode-Dateien und Grafiken. Es werden keine zusätzlichen Module aus dem Asset-Store benötigt, um das Projekt zu öffnen. Der Quellcode teilt sich dabei in zwei wesentliche Teile, als Assemblies umgesetzt: MEEP und VNotch

**MEEP** (Machine Experiment Engine Project) bildet den Kern der Anwendung. Die dort liegenden Systeme sind so weit möglich abstrakt konzipiert - mit der Absicht, diese für zukünftige andere Experimente weiter zu entwickeln.

**VNotch** befasst sich mit Code, der spezifisch zu diesem Projekt gehört.

### Issues

Noch offene Aufgabe und Mängel an der Anwendung wurden in Issues dokumentiert.