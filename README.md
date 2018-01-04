Each merchant needs to define a specific URL to their backend (e.g., www.merchantportal.com/process_transaction.php). This URL will be called by Payconiq’s servers in order to send the status of the transaction to the merchant. This allows the merchant’s backend to process the data (mark the transaction in database, update the product count number, send email to the customer, etc.).

Since webhooks are asynchronous, their order is not guaranteed. Idempotency might lead to a duplicate notification of the same type of event. When such an event occurs, Payconiq issues a HTTP POST notification message to the merchant’s backend through the webhook listener URL which the merchant has defined in their merchant callback (see Prerequisites). Because anyone can theoretically POST a HTTPS payload to an app, Payconiq signs the request and sends it over a HTTPS (SSL/TLS) connection to the apps.

Event headers for notification messages contain the generated asymmetric signature and information that you can use to validate the signature. The JSON-formatted POST request contains event information.

X-Security-Signature: The Payconiq-generated asymmetric signature using the algorithm specified with the X-Security-Algorithm.
X-Security-Timestamp: When the request/event was generated.
X-Security-Key: The X509 public key certificate from Payconiq.
X-Security-Algorithm: The algorithm that Payconiq uses to generate the signature and that you can use to verify the signature.

To generate the signature, Payconiq concatenates and separates these items with the pipe (|) character. You can validate a signature through this input string: merchantId|timestamp|crc32 crc32 – is Cyclic Redundancy
