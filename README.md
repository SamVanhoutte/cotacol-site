# Cotacol Web App

An attempt to learn Blazor, by setting up a Cotacol web site

This web site has been published to https://www.cotacol.cc and can be used to register and link your Strava profile against the Cotacol Hunting backend.  This web site shares the same backend as the iOS app (and the Android app that is coming)

## Running the UI tests

### Cypress

Running cypress tests

```
npx cypress open
```

The tests are located here: [spec.cy.js](tests/cypress/cypress/e2e/spec.cy.js)

[More information](https://blog.apify.com/how-to-record-test-cypress-recorder-extension/#:~:text=In%20the%20Chrome%20DevTools%20Panel,in%20the%20drop%2Ddown%20list.&text=In%20the%20Cypress%20Recorder%20panel,recording%20your%20actions%20in%20Chrome).

### Playwright

Running playwright tests

Initial setup (will generate tests folder)

```
npm init playwright@latest
```

Run with UI / Studio
```
npx playwright test --ui
```

Command line running
```
npx playwright test
npx playwright show-report
```

The tests are located here: [spec.cy.js](tests/cypress/cypress/e2e/spec.cy.js)

[More information](https://playwright.dev/docs/intro).
