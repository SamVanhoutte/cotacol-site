describe('Cotacol information', () => {
  it('Col list to random detail', function() {
    const cotacolId = '1' + Math.random().toString().substr(2, 2)
    cy.visit('/climbs');
    cy.get(`#cotacol-${cotacolId}`).click();

    cy.get('#cotacolelevation').should('exist', 'cotacol elevation is required');
    cy.get('#cotacolgrade').should('exist');
    cy.get('#cotacoldistance').should('exist');
    cy.get('#cotacolsurface').should('exist');
    cy.get('#cotacoldone').should('not.exist');
    cy.get('#map1').should('exist');
    // cy.screenshot();
  });

  it('Get statistics', function() {
    cy.visit('/');
    cy.get(':nth-child(7) > .nav-link').click();
    cy.get(':nth-child(8) > .nav-link').click();
  });

  it('Col stats should return all cols', function() {
    cy.visit('/colstats');
    // We should have all cotacols listed
    cy.get('[id="cotacolitem"]').should('have.length', 1000)
  });

  it('User leaderboard exists', function() {
    cy.visit('/stats');
    // We should have all cotacols listed
    cy.get('[id="cotacoluser"]')
      .its('length')
      .should('be.gte', 50)
  });

  /* ==== Test Created with Cypress Studio ==== */
  it('ESPC Live test', function() {
    /* ==== Generated with Cypress Studio ==== */
    cy.visit('https://localhost:7259/map');
    cy.get(':nth-child(1) > .mud-chipset > :nth-child(4) > .mud-chip-content').click();
    cy.get(':nth-child(2) > .mud-chipset > :nth-child(1) > .mud-chip-content').click();
    cy.get('[style="z-index: 3; position: absolute; height: 100%; width: 100%; padding: 0px; border-width: 0px; margin: 0px; left: 0px; top: 0px; touch-action: pan-x pan-y;"]').click();
    /* ==== End Cypress Studio ==== */
  });
})