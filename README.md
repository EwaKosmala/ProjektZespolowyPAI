# Planer Posiłków

Planer Posiłków to aplikacja webowa umożliwiająca kompleksowe zarządzanie codziennym jadłospisem.
System pozwala użytkownikom na gromadzenie własnych przepisów, planowanie posiłków na poszczególne dni tygodnia oraz automatyczne generowanie list zakupów.

# Funkcje aplikacji

* tworzenie i zarządzanie własnymi przepisami,
* planowanie posiłków w układzie tygodniowym,
* automatyczne generowanie list zakupów,
* ręczne tworzenie list zakupów z bazy produktów,
* eksport list zakupów do pliku PDF,
* przeglądanie i filtrowanie przepisów innych użytkowników,
* bezpieczne zarządzanie kontem użytkownika.

# Zespół projektowy

* Ewa Kosmala
* Olha Tykhonchuk
* Nadia Puczka
* Ewelina Tybel

# Technologie

**Język:** C#

**Framework:** .NET, ASP.NET Core MVC (Razor Views)

**Baza danych:** SQL Server Express / LocalDB

**ORM:** Entity Framework Core

**Generowanie PDF:** QuestPDF

# Instalacja i konfiguracja
Wymagania systemowe

* .NET SDK 9
* Visual Studio Community 2022
* workload: ASP.NET and web development
* Git
* Entity Framework Core CLI

# Kroki instalacji
  
1. Klonowanie repozytorium
```
git clone https://github.com/EwaKosmala/ProjektZespolowyPAI.git
cd ProjektZespolowyPAI
```

2. Konfiguracja bazy danych
Domyślnie aplikacja korzysta z:
```
(localdb)\mssqllocaldb
```
Sprawdź poprawność Connection String w pliku appsettings.json.

3. Inicjalizacja bazy danych
```
dotnet ef database update
```
4. Uruchomienie aplikacji
```
dotnet run
```
Po uruchomieniu aplikacja będzie dostępna pod adresem wskazanym w logach konsoli.

# Struktura funkcjonalna

* Zarządzanie przepisami
  * Dodawanie dań z opisem, instrukcją przygotowania oraz listą składników.
* Planowanie tygodniowe
  * Przypisywanie przepisów do konkretnych dni tygodnia.
* Listy zakupów
  * automatyczne generowanie na podstawie planu posiłków (z sumowaniem ilości),
  * ręczne tworzenie list,
  * eksport do PDF.
* Społeczność
  * Przeglądanie i filtrowanie przepisów innych użytkowników (np. po składnikach).
* Konto użytkownika
  * Rejestracja, logowanie, zmiana hasła oraz usuwanie konta.

# Utrzymanie projektu
1. Backup
  * Zaleca się regularny eksport bazy danych.

2. Aktualizacja
  * Po pobraniu zmian:
```
git pull
dotnet ef database update
```
aby zsynchronizować strukturę bazy danych.
