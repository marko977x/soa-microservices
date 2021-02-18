# SOA - MICROSERVICES

Ideja sistema je da prati meteoroloske parametre i na osnovu njih obavestava o vremenskim prilikama. Od tehnologija koriscen je .NET CORE za implementaciju mikroservisa
i React za implementaciju grafickog interfejsa.

## Mikroservisi
### Device microservice
Device mikroservis cita podatke sa tri senzora(senzor za temperaturu, vlaznost i pritisak vazduha) i prosledjuje ih Data mikroservisu putem brokera. Citanje podataka sa senzora
simulira se periodicnim citanjem podaka iz fajla. Moguce je podesavanje parametara ocitavanja u vidu promene praga vrednosti ocitavanja ili periode ocitavanja.

### Data microservice
Data mikroservis prima podatke sa Device mikroservisa, upisuje ih u bazu(influxdb) i prosledjuje u Analytics mikroservis takodje pomocu brokera. 

### Analytics microservice
Analytics mikroservis prima podatke sa Data mikroservisa, analizira ih u cilju detektovanja bitne meteoroloske vesti, zatim takav podatak salje veb klijentu 
i smesta u bazu(influxdb). 

### Command microservice

### Gateway microservice
Gateway mikroservis predstavlja REST API za veb klijenta.

### Web dashboard
Web dashboard predstavlja graficki interfejs ove aplikacije, nudi prikaz podataka, vaznih obavestenja o vremenskim prilikama i promenu parametara ocitavanja podataka.

## Message Brokeri

Za komunikaciju izmedju mikroservisa koriste se brokeri, za slanje poruke veb klijentu od strane analytics mikroservisa koristi se SignalR biblioteka za socket komunikaciju. 
U aplikaciji postoje dva topic-a, jedan za komunikaciju izmedju Device i Data mikroservisa i drugi za komunikaciju izmedju Data mikroservisa i Analytics mikroservisa. 

## Endpoints
- http://localhost:5008/GetCommandList
- http://localhost:5008/GetSensorParams
- http://localhost:5008/GetAllSensorsParams
- http://localhost:5008/GetTimeout
- http://localhost:5008/GetThreshold
- http://localhost:5008/GetAllSensorsCurrentValues
- http://localhost:5008/GetSensorCurrentValue
- http://localhost:5008/GetMaxValue
- http://localhost:5008/GetAllSensorsMaxValues
- http://localhost:5008/GetMinValue
- http://localhost:5008/GetAllSensorsMinValues
- http://localhost:5008/GetLastNHoursMeanValue
- http://localhost:5008/GetAllSensorsLastNHoursMeanValues
- http://localhost:5008/GetLastNMinutesValues
- http://localhost:5008/GetAllSensorsLastNMinutesValues
- http://localhost:5008/SetTimeout
- http://localhost:5008/SetThreshold
- http://localhost:5008/TurnOnOffSensor

## Startovanje aplikacije

Windows:
- cd path-to-the-project
- docker-compose up --build
- cd ./web-dashboard
- npm install
- npm start

Linux:
- cd path-to-the-project
- sudo docker-compose up --build
- cd ./web-dashboard
- npm install
- npm start
