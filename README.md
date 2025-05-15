# Offline task - notification service
## Implementation
The solution supports sending notifications via both email and SMS channels. Each channel can have multiple providers, with individual feature toggles and priorities. When sending a notification, the service retrieves all providers, filters out any that are disabled and sorts the remaining ones by priority. It then attempts to send the notification using each provider, retrying up to three times per provider. If all providers fail, background job retries sending the notification later.

The service was implemented using controller-based HTTP endpoints, although RabbitMQ could offer a more resilient solution. To ensure notifications are eventually delivered to the user, the service uses Polly to implement retry logic for provider calls and Hangfire to schedule a background job that can run multiple times with different time intervals and can also be triggered manually if needed.

## Possible improvements
* Validating request (checking if all the necessarry data was sent in the request body)
* Introducing circuit breaker to stop making requests to provider if it's down
* Fully covering solution in tests
* Introducing RabbitMQ instead of making http calls

## Running the program
1. Run docker-compose to create database
2. Add providers secrets to appsettings.json
3. Start application
