const config = {
  username: process.env.CI ? Cypress.env('GMAIL_USER') : Cypress.env('gmail_user'),
  password: process.env.CI ? Cypress.env('GMAIL_PASSWORD') : Cypress.env('gmail_password'),
};

// describe('Unauthenticated user sees achievements', () => {
//   it('can log in', () => {
    
//     // Cypress.config('defaultCommandTimeout', 10000);
//     cy.visit('/achievements')
//     //cy.origin('https://www.strava.com', {args: {config} }, ({config}) => {
//       cy.get('#google-signin').click();  
//       cy.origin('https://accounts.google.com', {args: {config} }, ({config}) => {
//         cy.get('#identifierId').type(config.username);
//         cy.get('#identifierNext > .VfPpkd-dgl2Hf-ppHlrf-sM5MNb > .VfPpkd-LgbsSe > .VfPpkd-vQzf8d').click();
//         cy.get('[name="Passwd"]').type(config.password);
//       })
//     //})
//     // cy.origin('https://www.strava.com', {args: {config} }, ({config}) => {
//     // })
//     // cy.get("#existinglock0").click()
//     // cy.get('.mud-checkbox-input').check();
//     // cy.get('#registerbutton').click();
//     // cy.get("#external-ref").should('contain.text', 'AutoTestValue01')
//     // cy.get('#finish-button').click();
//     // cy.visit('/connections')
//     // cy.get('#connection-0').click();
//     // cy.get('#revoke-button').click();
//     // cy.get('.mud-dialog-actions > .mud-button-filled > .mud-button-label').click();
//   })
// })
