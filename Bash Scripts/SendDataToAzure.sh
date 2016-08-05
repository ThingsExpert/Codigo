#!/bin/bash

#Python Helper Functions
alias urlencode='python -c "import sys, urllib as ul; print ul.quote_plus(sys.argv[1])"'
alias computehash='python -c "import sys, hmac, hashlib, base64; bytes_msg = str.encode(sys.argv[1] + '"'"'\n'"'"' + sys.argv[2]); bytes_key = str.encode(sys.argv[3]);  dig = hmac.new(bytes_key, msg=bytes_msg, digestmod=hashlib.sha256).digest();print(base64.b64encode(dig).decode());"'

#Parameters
Endpoint="https://eventhuballan-ns.servicebus.windows.net/"
EndpointMsg="$Endpoint/eventhuballan/messages"
SasKeyName="all"
SasKeyValue="y4hHJ6E25apmgbBz7/dSN7aXraJPPDELrGS0SCycFYA="
Authorization="SharedAccessSignature"
Duration=3600

#Param Processing
FromEpochStart=$(date +%s)
Expiry=$((FromEpochStart+Duration))
EndpointEncoded=$(urlencode $Endpoint 2>&1)
Signature=$(computehash $EndpointEncoded $Expiry $SasKeyValue 2>&1)
SignatureEncoded=$(urlencode $Signature 2>&1)
Token="$Authorization sr=$EndpointEncoded&sig=$SignatureEncoded&se=$Expiry&skn=$SasKeyName"

#Data in JSON format
Data='{"pkey1":"xyz","rkey1":"xyz","temp":32}'

#Sends Data to Azure
curl -H "Content-Type: application/json" -H "Authorization: $Token" -X POST -d $Data $EndpointMsg