# ClientServerApplication-Zadatak1
## Konkurentno i mrežno programiranje zadaci

Zadatak 1. Na osnovu primera sa predavanja (RegistrationServer i RegistrationClient iz zip-a "Mrezno programiranje), napisati klijent/server aplikaciju koja registruje korisnike na serveru. Klijent implementirati kao konzolnu aplikaciju koja prima jednu od dve komande: ADD username i LIST. Prva komanda registruje korisnika na serveru (dodaje ga na spisak) ADD username, a druga lista sve prijavljene korisnike. Zadate komande proslediti server aplikaciji, koji će ih izvršiti na serverskoj strani i vratiti rezultat. Protokol komunikacije između klijenta i servera se prepušta studentu.

Dodatni zadatak:
Dodati komandu REMOVE koja briše korisnika iz spiska registrovanih korisnika na serveru.
Dodati komandu FIND username koja daje informaciju da li je navedeni korinsnik registrovan na serveru.
Dodati komandu ADD username1, username2, username3 ... koja omogućuje da se jednom komandom izvrši dodavanje proizvoljnog broja korisnika.

Prilikom FIND/REMOVE ako ne postoji korisnik sa zadatim username treba da se vrati poruka da korisnik ne postoji.

Prilikom ADD običnog i višestrukog ako već postoji korisnik sa zadatim username treba da se vrati poruka da korisnik već postoji.
