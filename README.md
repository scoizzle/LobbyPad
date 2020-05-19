Lobby Management System (C) Scot Murphy 2020

In-house made lobby waiting system with sms contact capability.

Application depends on the Twilio Sid and Token being defined in secrets/appsettings.secrets.json, would recomend defining the file within a kubernetes secret.

kubectl create secret generic lobby-pad-secrets --from-file=./appsettings.secrets.json