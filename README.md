# Recreatieve Zwemwedstrijden
Simpel tooltje om recreatieve zwemwedstrijden te automatiseren. Wedstrijd aanmaken, inschrijvingen beheren en automatisch programma's genereren.

Moet gehost worden in Azure als static web app, waardoor de kosten nihil zijn.

# Opslag
De volgende opslag wordt ondersteund:
* In Memory
* Cosmos Db
* Blob storage

# Configuratie en installatie
## Lokaal
Maak een `local.settings.json` file aan in het Api project, en vul deze met:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "Repository": "memory"
  }
}
```

Bij repository kan gekozen worden voor:
* memory
* cosmos
* storage

Bij `storage` wordt gebruik gemaakt van de storage van de function zelf, zoals aangegeven
in `AzureWebJobsStorage`. Als je deployt naar Azure wordt dit automatisch ingevuld.

Wanneer je kiest voor cosmos zijn er extra instellingen benodigd.
De cosmos database moet worden aangemaakt met een container waarbij de partition key wordt ingesteld op `/id`.

De instellingen bij Cosmos zien er als volgt uit:
```json
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "Repository": "cosmos",
    "CosmosConnectionString": "AccountEndpoint=https://.../;AccountKey=...",
    "CosmosDatabase": "...",
    "CosmosContainer": "..."
  }
}
```

## Azure
In Azure kan eenvoudig gedeployd worden naar een static web app.
De Api heet `Api` en de client heet `Client`.

In de Application Settings (Settings --> Configuration) moet worden aangegeven welke storage gebruikt moet worden.
Dit kan gedaan worden door de volgende waarde in te stellen:

| Name | Value |
| -- | -- |
| Repository | memory, cosmos of storage |

Indien gekozen wordt voor Cosmos moet uiteraard een Cosmos database aangemaakt worden (partition key ingesteld op `/id`).
Daarnaast moeten de volgende instellingen worden gedaan:

| Name | Value |
| -- | -- |
| Repository | cosmos |
| CosmosConnectionString | AccountEndpoint=https://.../;AccountKey=... |
| CosmosDatabase | ... |
| CosmosContainer | ... |


# Openstaande punten
* Aantal zwemmers per serie kunnen instellen
- Inschrijvingen kopiëren uit eerdere  wedstrijd
- Verwijderendeelnemers uit inschrijvingen
- Aanmaken gegenereerd programma geeft een error

# Screenshots
Wordt binnenkort aangevuld.

# Contact
Voor vragen over deze app kan contact opgenomen worden met:
* Jordi Jolink
* mail [at] jordijolink.nl