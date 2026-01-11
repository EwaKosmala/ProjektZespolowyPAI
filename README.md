Planer Posiłków to aplikacja webowa umożliwiająca użytkownikom kompleksowe zarządzanie codziennym jadłospisem. System pozwala na gromadzenie własnych przepisów, planowanie posiłków na poszczególne dni tygodnia oraz automatyczne generowanie list zakupów.
Zespół projektowy
  Ewa Kosmala
  Olha Tykhonchuk
  Nadia Puczka
  Ewelina Tybel
Technologie
  Język: C#
  Framework: .NET, ASP.NET Core MVC z Razor Views
  Baza danych: SQL Server Express/LocalDB
  ORM: Entity Framework Core
  Generowanie PDF: QuestPDF
Główne funkcjonalności
  Zarządzanie przepisami: Dodawanie własnych dań z opisem, instrukcją i listą składników.
  Planowanie tygodniowe: Przypisywanie przepisów do konkretnych dni tygodnia.
  Listy zakupów: * Automatyczne generowanie na podstawie planu posiłków (z sumowaniem ilości składników).
  Ręczne tworzenie list z bazy dostępnych produktów.
  Eksport listy do formatu PDF.
  Społeczność i odkrywanie: Przeglądanie i filtrowanie przepisów innych użytkowników (np. po posiadanych składnikach).
  Zarządzanie kontem: Bezpieczna rejestracja, logowanie, zmiana hasła oraz usuwanie konta.
Instalacja i konfiguracja
  Wymagania systemowe
    Środowisko: .NET SDK 9
    IDE: Visual Studio Community 2022 (z workloadem ASP.NET)
    Narzędzia: Git, Entity Framework Core CLI
  Kroki wdrożeniowe
    Klonowanie repozytorium:
      git clone https://github.com/EwaKosmala/ProjektZespolowyPAI.git
      cd ProjektZespolowyPAI
    Konfiguracja bazy danych: 
      Domyślnie aplikacja korzysta z (localdb)\mssqllocaldb. 
      Upewnij się, że Connection String w appsettings.json jest poprawny.
    Inicjalizacja bazy: 
      Wykonaj migracje, aby stworzyć strukturę tabel:
        dotnet ef database update
    Uruchomienie:
      dotnet run

Aplikacja będzie dostępna pod adresem wskazanym w logach konsoli.
Uwagi dot. utrzymania
  Backup: Zaleca się regularny eksport bazy do plików.
  Aktualizacja: Po pobraniu nowej wersji kodu (git pull) należy zawsze uruchomić dotnet ef database update, aby zsynchronizować model danych.

