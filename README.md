# TransferaShipment
Shipment


Shpiments.Api:
U appsetings.json podesiti:
ConnectionStrings: pod nazivom DefaultConnection: podesiti naziv Shipemnts.db
Azure podesiti Connection string u appsetings.json kao i QueueName
Blob podesiti Connection string u appsetings.json kao i ContainerName

Nakon toga startovati projekat Shipments.Api
dotnet run --project Shipments/Shipments.Api.csproj


WorkerService:
Prvo podesiti konfiguraciju u Shipments.WorkerService/appsetings.json
Baza je SqlLite gde se u appsetings.json podesi naziv fajla (ShipmentsDb)
Azure podesiti Connection string u appsetings.json kao i QueueName
Blob podesiti Connection string u appsetings.json kao i ContainerName

Nakon toga startovati projekat Shipments.WorkerService


Konekcioni stringovi se podesavaju u Shipemnts.Api zatim appsetings.json
Konekcioni stringovi se podesavaju u Shipemnts.WorkerService zatim appsetings.json

{
  "info": {
    "_postman_id": "d5a0b3b2-8c1c-4c68-9b1d-6f4e4d0a2f10",
    "name": "Transfera Shipment API",
    "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
    "description": "Routes based on Shipments.Api.Controllers.ShipmentsController. Variables: baseUrl, shipmentId."
  },
  "item": [
    {
      "name": "Shipments",
      "item": [
        {
          "name": "Create Shipment",
          "request": {
            "method": "POST",
            "header": [
              { "key": "Content-Type", "value": "application/json" }
            ],
            "body": {
              "mode": "raw",
              "raw": "{\n  \"referenceNumber\": \"REF-001\",\n  \"sender\": \"Company A\",\n  \"recipient\": \"Company B\"\n}"
            },
            "url": {
              "raw": "{{baseUrl}}/api/shipments",
              "host": ["{{baseUrl}}"],
              "path": ["api", "shipments"]
            }
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "type": "text/javascript",
                "exec": [
                  "pm.test('Status is 201 Created', function () {",
                  "  pm.expect(pm.response.code).to.eql(201);",
                  "});",
                  "",
                  "// Save shipmentId from response (expects { id: \"guid\", ... })",
                  "try {",
                  "  const json = pm.response.json();",
                  "  if (json && json.id) {",
                  "    pm.collectionVariables.set('shipmentId', json.id);",
                  "    console.log('Saved shipmentId:', json.id);",
                  "  }",
                  "} catch (e) {",
                  "  console.log('Could not parse response JSON');",
                  "}"
                ]
              }
            }
          ]
        },
        {
          "name": "Get All Shipments (paged)",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/shipments?page=1&pageSize=20",
              "host": ["{{baseUrl}}"],
              "path": ["api", "shipments"],
              "query": [
                { "key": "page", "value": "1" },
                { "key": "pageSize", "value": "20" }
              ]
            }
          }
        },
        {
          "name": "Get Shipment By Id",
          "request": {
            "method": "GET",
            "header": [],
            "url": {
              "raw": "{{baseUrl}}/api/shipments/{{shipmentId}}",
              "host": ["{{baseUrl}}"],
              "path": ["api", "shipments", "{{shipmentId}}"]
            }
          }
        },
        {
          "name": "Upload Document (multipart/form-data)",
          "request": {
            "method": "POST",
            "header": [],
            "body": {
              "mode": "formdata",
              "formdata": [
                {
                  "key": "file",
                  "type": "file",
                  "src": []
                }
              ]
            },
            "url": {
              "raw": "{{baseUrl}}/api/shipments/{{shipmentId}}/document",
              "host": ["{{baseUrl}}"],
              "path": ["api", "shipments", "{{shipmentId}}", "document"]
            },
            "description": "Select a file in Body -> form-data -> key=file (type File). Controller consumes multipart/form-data and expects form field name 'file'."
          },
          "event": [
            {
              "listen": "test",
              "script": {
                "type": "text/javascript",
                "exec": [
                  "pm.test('Status is 200 OK', function () {",
                  "  pm.expect(pm.response.code).to.eql(200);",
                  "});",
                  "",
                  "// Response expected: { shipmentId, blobName, url }",
                  "try {",
                  "  const json = pm.response.json();",
                  "  pm.test('Has blobName', function(){ pm.expect(json.blobName).to.be.ok; });",
                  "  pm.test('Has url', function(){ pm.expect(json.url).to.be.ok; });",
                  "} catch (e) { }"
                ]
              }
            }
          ]
        }
      ]
    }
  ],
  "variable": [
    {
      "key": "baseUrl",
      "value": "https://localhost:7046",
      "type": "string"
    },
    {
      "key": "shipmentId",
      "value": "",
      "type": "string"
    }
  ]
}
